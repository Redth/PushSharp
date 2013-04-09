using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PushSharp.Core
{
	public delegate void ChannelCreatedDelegate(object sender, IPushChannel pushChannel);
	public delegate void ChannelDestroyedDelegate(object sender);
	public delegate void NotificationSentDelegate(object sender, INotification notification);
	public delegate void NotificationFailedDelegate(object sender, INotification notification, Exception error);
	public delegate void NotificationRequeueDelegate(object sender, NotificationRequeueEventArgs e);
	public delegate void ChannelExceptionDelegate(object sender, IPushChannel pushChannel, Exception error);
	public delegate void ServiceExceptionDelegate(object sender, Exception error);
	public delegate void DeviceSubscriptionExpiredDelegate(object sender, string expiredSubscriptionId, DateTime expirationDateUtc, INotification notification);
	public delegate void DeviceSubscriptionChangedDelegate(object sender, string oldSubscriptionId, string newSubscriptionId, INotification notification);

	public abstract class PushServiceBase : IPushService
	{
		public event ChannelCreatedDelegate OnChannelCreated;
		public event ChannelDestroyedDelegate OnChannelDestroyed;
		public event NotificationSentDelegate OnNotificationSent;
		public event NotificationFailedDelegate OnNotificationFailed;
		public event NotificationRequeueDelegate OnNotificationRequeue;
		public event ChannelExceptionDelegate OnChannelException;
		public event ServiceExceptionDelegate OnServiceException;
		public event DeviceSubscriptionExpiredDelegate OnDeviceSubscriptionExpired;
		public event DeviceSubscriptionChangedDelegate OnDeviceSubscriptionChanged;

		protected void RaiseSubscriptionExpired(string expiredSubscriptionId, DateTime expirationDateUtc, INotification notification)
		{
			var evt = OnDeviceSubscriptionExpired;
			if (evt != null)
				evt(this, expiredSubscriptionId, expirationDateUtc, notification);
		}

		public void RaiseServiceException(Exception error)
		{
			var evt = OnServiceException;
			if (evt != null)
				evt(this, error);
		}

		public virtual bool BlockOnMessageResult
		{
			get { return true; }
		}

		public IPushChannelFactory PushChannelFactory { get; private set; }
		public IPushServiceSettings ServiceSettings { get; private set; }
		public IPushChannelSettings ChannelSettings { get; private set; }

		public bool IsStopping
		{
			get { return stopping; }
		}

		private Timer timerCheckScale;
		private volatile bool stopping;
		private List<ChannelWorker> channels = new List<ChannelWorker>();
		private ConcurrentQueue<INotification> queuedNotifications;
		private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
		private List<WaitTimeMeasurement> measurements = new List<WaitTimeMeasurement>();

		private long trackedNotificationCount = 0;

		ManualResetEvent waitQueuedNotifications = new ManualResetEvent(false);

		protected PushServiceBase(IPushChannelFactory pushChannelFactory, IPushChannelSettings channelSettings)
			: this(pushChannelFactory, channelSettings, default(IPushServiceSettings))
		{
		}

		protected PushServiceBase(IPushChannelFactory pushChannelFactory, IPushChannelSettings channelSettings,
		                          IPushServiceSettings serviceSettings)
		{
			this.PushChannelFactory = pushChannelFactory;
			this.ServiceSettings = serviceSettings ?? new PushServiceSettings();
			this.ChannelSettings = channelSettings;

			this.queuedNotifications = new ConcurrentQueue<INotification>();

			timerCheckScale = new Timer(CheckScale, null, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(15));

			CheckScale();
			
			stopping = false;
		}

		public void QueueNotification(INotification notification)
		{
			QueueNotification(notification, false);
		}


		private void QueueNotification(INotification notification, bool countsAsRequeue = true,
		                               bool ignoreStoppingChannel = false)
		{
			Interlocked.Increment(ref trackedNotificationCount);

			//Measure when the message entered the queue
			notification.EnqueuedTimestamp = DateTime.UtcNow;

			if (this.cancelTokenSource.IsCancellationRequested)
				throw new ObjectDisposedException("Service", "Service has already been signaled to stop");

			if (this.ServiceSettings.MaxNotificationRequeues < 0 ||
			    notification.QueuedCount <= this.ServiceSettings.MaxNotificationRequeues)
			{
				//Reset the Enqueued time in case this is a requeue
				notification.EnqueuedTimestamp = DateTime.UtcNow;

				//Increase the queue counter
				if (countsAsRequeue)
					notification.QueuedCount++;

				queuedNotifications.Enqueue(notification);

				//Allow anything waiting on a queued notification to continue faster
				waitQueuedNotifications.Set();
			}
			else
			{
				var evt = this.OnNotificationFailed;
				if (evt != null)
					evt(this, notification, new MaxSendAttemptsReachedException());

				Log.Info("Notification ReQueued Too Many Times: {0}", notification.QueuedCount);
			}
		}

		public void Stop(bool waitForQueueToFinish = true)
		{
			stopping = true;
			var started = DateTime.UtcNow;

			//Stop the timer for checking scale
			if (this.timerCheckScale != null)
				this.timerCheckScale.Change(Timeout.Infinite, Timeout.Infinite);
			

			if (waitForQueueToFinish)
			{
				while (this.queuedNotifications.Count > 0 || Interlocked.Read(ref trackedNotificationCount) > 0)
					Thread.Sleep(100);
			}
			
			//Stop all channels
			Parallel.ForEach(channels, c => c.Dispose());
			
			this.channels.Clear();

			this.cancelTokenSource.Cancel();

			Log.Info("PushServiceBase->DISPOSE.");
		}

		public void Dispose()
		{
			if (!stopping)
				Stop(false);
		}
		
		private void CheckScale(object state = null)
		{
			if (stopping)
				return;

			Log.Info("{0} -> Checking Scale ({1} Channels Currently)", this, channels.Count);

			if (ServiceSettings.AutoScaleChannels && !this.cancelTokenSource.IsCancellationRequested)
			{
				if (channels.Count <= 0)
				{
					Log.Info("{0} -> Creating Channel", this);
					ScaleChannels(ChannelScaleAction.Create);
					return;
				}

				var avgTime = (int)GetAverageQueueWait();

				Log.Info("{0} -> Avg Queue Wait Time {1} ms", this, avgTime);

				if (avgTime < ServiceSettings.MinAvgTimeToScaleChannels && channels.Count > 1)
				{
					Log.Info("{0} -> Destroying Channel", this);
					ScaleChannels(ChannelScaleAction.Destroy);
				}
				else if (channels.Count < this.ServiceSettings.MaxAutoScaleChannels)
				{
					var numChannelsToSpinUp = 0;

					//Depending on the wait time, let's spin up more than 1 channel at a time
					if (avgTime > 5000)
						numChannelsToSpinUp = 3;
					else if (avgTime > 1000)
						numChannelsToSpinUp = 2;
					else if (avgTime > ServiceSettings.MinAvgTimeToScaleChannels)
						numChannelsToSpinUp = 1;

					if (numChannelsToSpinUp > 0)
					{
						Log.Info("{0} -> Creating {1} Channel(s)", this, numChannelsToSpinUp);
						ScaleChannels(ChannelScaleAction.Create, numChannelsToSpinUp);
					}
				}
			}
			else
			{
				while (channels.Count > ServiceSettings.Channels && !this.cancelTokenSource.IsCancellationRequested && !stopping)
				{
					Log.Info("{0} -> Destroying Channel", this);
					ScaleChannels(ChannelScaleAction.Destroy);
				}
				while (channels.Count < ServiceSettings.Channels && !this.cancelTokenSource.IsCancellationRequested && !stopping)
				{
					Log.Info("{0} -> Creating Channel", this);
					ScaleChannels(ChannelScaleAction.Create);
				}
			}
		}

		private double GetAverageQueueWait()
		{
			if (measurements == null || measurements.Count <= 0)
				return 0;

			lock (measurements)
			{
				//Remove old measurements
				measurements.RemoveAll(m => m.Timestamp < DateTime.UtcNow.AddSeconds(-5));

				if (measurements.Count > 0)
					return (from m in measurements select m.Milliseconds).Average();
			}

			return 0;
		}

		private void ScaleChannels(ChannelScaleAction action, int count = 1)
		{
			if (stopping)
				return;

			for (int i = 0; i < count; i++)
			{
				if (stopping)
					break;

				var newCount = 0;
				bool? destroyed = null;
				IPushChannel newChannel = default(IPushChannel);

				lock (channels)
				{
					if (action == ChannelScaleAction.Create)
					{
						newChannel = this.PushChannelFactory.CreateChannel(this.ChannelSettings);

						var chanWorker = new ChannelWorker(newChannel, DoChannelWork);
						chanWorker.WorkerTask.ContinueWith(t =>
						{
							var ex = t.Exception;
							Log.Error("Channel Worker Failed Task: " + ex.ToString());
						}, TaskContinuationOptions.OnlyOnFaulted);
							
						channels.Add(chanWorker);

						newCount = channels.Count;
						destroyed = false;
					}
					else if (action == ChannelScaleAction.Destroy && channels.Count > 1)
					{
						var channelOn = channels[0];
						channels.RemoveAt(0);

						//Now stop the channel but let it finish
						channelOn.Channel.Dispose();

						newCount = channels.Count;
						destroyed = true;
					}
				}

				if (destroyed.HasValue && !destroyed.Value)
				{
					var evt = OnChannelCreated;
					if (evt != null)
						evt(this, newChannel);
				}
				else if (destroyed.HasValue && destroyed.Value)
				{
					var evt = this.OnChannelDestroyed;
					if (evt != null)
						evt(this);
				}
			}
		}

		private void DoChannelWork(IPushChannel channel, CancellationTokenSource cancelTokenSource)
		{
			while (!cancelTokenSource.IsCancellationRequested)
			{
				var waitForNotification = new ManualResetEvent(false);

				INotification notification;

				if (!queuedNotifications.TryDequeue(out notification))
				{
					Thread.Sleep(100);
					continue;
				}

				var msWaited = (DateTime.UtcNow - notification.EnqueuedTimestamp).TotalMilliseconds;

				lock (measurements)
				{
					measurements.Add(new WaitTimeMeasurement((long) msWaited));
				}

				channel.SendNotification(notification, (sender, result) =>
					{
						//Trigger 
						if (this.BlockOnMessageResult)	
							waitForNotification.Set();

						Interlocked.Decrement(ref trackedNotificationCount);

						//Handle the notification send callback here
						if (result.ShouldRequeue)
						{
							var eventArgs = new NotificationRequeueEventArgs(result.Notification);
							var evt = this.OnNotificationRequeue;
							if (evt != null)
								evt(this, eventArgs);

							//See if the requeue was cancelled in the event args
							if (!eventArgs.Cancel)
								this.QueueNotification(result.Notification, result.CountsAsRequeue, true);
						}
						else
						{
							//Result was a success, but there are still more possible outcomes than an outright success
							if (!result.IsSuccess)
							{
								//Check if the subscription was expired
								if (result.IsSubscriptionExpired)
								{
									//If there is a new id, the subscription must have changed
									//This is a fairly special case that only GCM should really ever raise
									if (!string.IsNullOrEmpty(result.NewSubscriptionId))
									{
										var evt = this.OnDeviceSubscriptionChanged;
										if (evt != null)
											evt(this, result.OldSubscriptionId, result.NewSubscriptionId, result.Notification);
									}
									else
									{
										var evt = this.OnDeviceSubscriptionExpired;
										if (evt != null)
											evt(this, result.OldSubscriptionId, result.SubscriptionExpiryUtc, result.Notification);
									}
								}
								else //Otherwise some general failure
								{
									var evt = this.OnNotificationFailed;
									if (evt != null)
										evt(this, result.Notification, result.Error);
								}
							}
							else
							{
								var evt = this.OnNotificationSent;
								if (evt != null)
									evt(this, result.Notification);
							}
						}
					});

				
				if (this.BlockOnMessageResult && !waitForNotification.WaitOne(ServiceSettings.NotificationSendTimeout))
				{
					Interlocked.Decrement(ref trackedNotificationCount);

					var evt = this.OnNotificationFailed;
					if (evt != null)
						evt(this, notification, new TimeoutException("Notification send timed out"));
				}
			}

			channel.Dispose();
		}

		internal class WaitTimeMeasurement
		{
			public WaitTimeMeasurement(long milliseconds)
			{
				this.Timestamp = DateTime.UtcNow;
				this.Milliseconds = milliseconds;
			}

			public DateTime Timestamp { get; set; }
			public long Milliseconds { get; set; }
		}

		internal class ChannelWorker : IDisposable
		{
			public ChannelWorker(IPushChannel channel, Action<IPushChannel, CancellationTokenSource> worker)
			{
				this.CancelTokenSource = new CancellationTokenSource();
				this.Channel = channel;
				this.WorkerTask = Task.Factory.StartNew(() => worker(channel, this.CancelTokenSource),
				                                        TaskCreationOptions.LongRunning);
			}

			public void Dispose()
			{
				CancelTokenSource.Cancel();
			}

			public Task WorkerTask { get; private set; }
			
			public IPushChannel Channel { get; set; }

			public CancellationTokenSource CancelTokenSource { get; set; }
		}
	}

	public enum ChannelScaleAction
	{
		Create,
		Destroy
	}

	public class NotificationRequeueEventArgs : EventArgs
	{
		public NotificationRequeueEventArgs(INotification notification) 
		{
			this.Cancel = false;
			this.Notification = notification;
		}

		public bool Cancel { get;set; }
		public INotification Notification { get; private set; }
	}
}
