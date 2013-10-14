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
		private int scaleSync;
		private volatile bool stopping;
		private List<ChannelWorker> channels = new List<ChannelWorker>();
		private NotificationQueue queuedNotifications;
		private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
		private List<WaitTimeMeasurement> measurements = new List<WaitTimeMeasurement>();
        private List<WaitTimeMeasurement> sendTimeMeasurements = new List<WaitTimeMeasurement>();
		private DateTime lastNotificationQueueTime = DateTime.MinValue;
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

			this.queuedNotifications = new NotificationQueue();

			scaleSync = 0;

			timerCheckScale = new Timer(CheckScale, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));

			CheckScale();
			
			stopping = false;
		}

		public void QueueNotification(INotification notification)
		{
			QueueNotification(notification, false);
		}


		private void QueueNotification(INotification notification, bool countsAsRequeue = true,
		                               bool ignoreStoppingChannel = false, bool queueToFront = false)
		{
			lastNotificationQueueTime = DateTime.UtcNow;

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

				if (queueToFront)
					queuedNotifications.EnqueueAtStart(notification);
				else
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

			if (waitForQueueToFinish)
			{
				Log.Info ("Waiting for Queue to Finish");

				while (this.queuedNotifications.Count > 0 || Interlocked.Read(ref trackedNotificationCount) > 0)
					Thread.Sleep(100);

				Log.Info("Queue Emptied.");
			}

			Log.Info ("Stopping CheckScale Timer");

			//Stop the timer for checking scale
			if (this.timerCheckScale != null)
				this.timerCheckScale.Change(Timeout.Infinite, Timeout.Infinite);

			Log.Info ("Stopping all Channels");

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
			int sync = -1;

			try
			{
				sync = Interlocked.CompareExchange(ref scaleSync, 1, 0);
				if (sync == 0)
				{
					var avgQueueTime = (int)this.AverageQueueWaitTime.TotalMilliseconds;
					var avgSendTime = (int)this.AverageSendTime.TotalMilliseconds;

					Log.Info("{0} -> Avg Queue Wait Time {1} ms, Avg Send Time {2} ms", this, avgQueueTime, avgSendTime);

					//if (stopping)
					//	return;

					Log.Info("{0} -> Checking Scale ({1} Channels Currently)", this, channels.Count);

					if (ServiceSettings.AutoScaleChannels && !this.cancelTokenSource.IsCancellationRequested)
					{
						if (channels.Count <= 0 && QueueLength > 0)
						{
							Log.Info("{0} -> Creating Channel {1}", this, channels.Count);
							ScaleChannels(ChannelScaleAction.Create);
							return;
						}

						//Check to see if the Idle timeout is being used.  If so, we want to destroy all the channels if we're in idle mode
						// Only do this if we haven't sent in a long time (greater than the IdleTimeout) and we have nothing in the queue
						// and we have no tracked notification count
						if (ServiceSettings.IdleTimeout > TimeSpan.Zero && 
						    channels.Count > 0 && QueueLength <= 0 
						    && (DateTime.UtcNow - lastNotificationQueueTime) > ServiceSettings.IdleTimeout
						    && Interlocked.Read(ref trackedNotificationCount) <= 0)
						{
							Log.Info("{0} -> Service Idle, Destroying all Channels", this, channels.Count);
							while (channels.Count > 0 && !this.cancelTokenSource.IsCancellationRequested)
								ScaleChannels(ChannelScaleAction.Destroy);

							return;
						}

						if (avgQueueTime < ServiceSettings.MinAvgTimeToScaleChannels && channels.Count > 1)
						{
							var numChannelsToSpinDown = 1;

							if (avgQueueTime <= 0)
								numChannelsToSpinDown = 5;

							if (channels.Count - numChannelsToSpinDown <= 0)
								numChannelsToSpinDown = 1;

							Log.Info("{0} -> Destroying Channel", this);
							ScaleChannels(ChannelScaleAction.Destroy, numChannelsToSpinDown);
						}
						else if (channels.Count < this.ServiceSettings.MaxAutoScaleChannels)
						{
							var numChannelsToSpinUp = 0;

							//Depending on the wait time, let's spin up more than 1 channel at a time
							if (avgQueueTime > 5000)
								numChannelsToSpinUp = 3;
							else if (avgQueueTime > 1000)
								numChannelsToSpinUp = 2;
							else if (avgQueueTime > ServiceSettings.MinAvgTimeToScaleChannels)
								numChannelsToSpinUp = 1;

							if (numChannelsToSpinUp > 0)
							{
								//Don't spin up more than the max!
								if (channels.Count + numChannelsToSpinUp > ServiceSettings.MaxAutoScaleChannels)
									numChannelsToSpinUp = ServiceSettings.MaxAutoScaleChannels - channels.Count;

								if (numChannelsToSpinUp > 0)
								{
									Log.Info("{0} -> Creating {1} Channel(s)", this, numChannelsToSpinUp);
									ScaleChannels(ChannelScaleAction.Create, numChannelsToSpinUp);
								}
							}
						}
					}
					else
					{
						//Check to see if the Idle timeout is being used.  If so, we want to destroy all the channels if we're in idle mode
						// Only do this if we haven't sent in a long time (greater than the IdleTimeout) and we have nothing in the queue
						// and we have no tracked notification count
						if (ServiceSettings.IdleTimeout > TimeSpan.Zero && 
						    channels.Count > 0 && QueueLength <= 0 
						    && (DateTime.UtcNow - lastNotificationQueueTime) > ServiceSettings.IdleTimeout
						    && Interlocked.Read(ref trackedNotificationCount) <= 0)
						{
							Log.Info("{0} -> Service Idle, Destroying all Channels", this, channels.Count);
							while (channels.Count > 0 && !this.cancelTokenSource.IsCancellationRequested)
								ScaleChannels(ChannelScaleAction.Destroy);

							return;
						}

						while (channels.Count > ServiceSettings.Channels && !this.cancelTokenSource.IsCancellationRequested)
						{
							Log.Info("{0} -> Destroying Channel", this);
							ScaleChannels(ChannelScaleAction.Destroy);
						}
                        while (channels.Count < ServiceSettings.Channels && !this.cancelTokenSource.IsCancellationRequested
                            && (DateTime.UtcNow - lastNotificationQueueTime) <= ServiceSettings.IdleTimeout
                            && Interlocked.Read(ref trackedNotificationCount) > 0)
						{
							Log.Info("{0} -> Creating Channel", this);
							ScaleChannels(ChannelScaleAction.Create);
						}
					}
				}
			}
			finally
			{
				if (sync == 0)
					scaleSync = 0;
			}
		}

        public TimeSpan AverageQueueWaitTime
        {
            get 
			{ 
				if (measurements == null || measurements.Count <= 0)
					return TimeSpan.Zero;

				lock (measurements)
				{
					//Remove old measurements
                    while (measurements.Count > 1000)
                        measurements.RemoveAt(0);

                    measurements.RemoveAll(m => m.Timestamp < DateTime.UtcNow.AddSeconds(-30));
                    
					//If there aren't even 20 measurements, there's not really any waiting happening so don't scale up!
					if (measurements.Count < 20)
						return TimeSpan.Zero;

				    var avg = from m in measurements select m.Milliseconds;
				    try { return TimeSpan.FromMilliseconds(avg.Average()); }
				    catch { return TimeSpan.Zero; }
				}
			}
        }

        public TimeSpan AverageSendTime
        {
            get
            {
				if (sendTimeMeasurements == null || sendTimeMeasurements.Count <= 0)
					return TimeSpan.Zero;

                lock (sendTimeMeasurements)
                {
                    while (sendTimeMeasurements.Count > 1000)
                        sendTimeMeasurements.RemoveAt(0);

                    sendTimeMeasurements.RemoveAll(m => m.Timestamp < DateTime.UtcNow.AddSeconds(-30));

                    var avg = from s in sendTimeMeasurements select s.Milliseconds;
					
	                try { return TimeSpan.FromMilliseconds(avg.Average()); }
                    catch { return TimeSpan.Zero; }
                }
            }
        }

	    public long QueueLength
	    {
	        get
	        {
	            lock (queuedNotifications)
	                return queuedNotifications.Count;
	        }
	    }

	    public long ChannelCount
	    {
	        get
	        {
	            lock (channels)
	                return channels.Count;
	        }
	    }

		private void ScaleChannels(ChannelScaleAction action, int count = 1)
		{
			//if (stopping)
			//	return;

			for (int i = 0; i < count; i++)
			{
				if (cancelTokenSource.IsCancellationRequested)
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

						//Stop the channel worker, which will dispose the channel too
						channelOn.Dispose ();

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


	    private long totalSendCount = 0;

		private void DoChannelWork(IPushChannel channel, CancellationTokenSource cancelTokenSource)
		{
		    string id = Guid.NewGuid().ToString();

		    long sendCount = 0;

			while (!cancelTokenSource.IsCancellationRequested)
			{
				var waitForNotification = new ManualResetEvent(false);

				var notification = queuedNotifications.Dequeue ();

				if (notification == null)
				{
					Thread.Sleep(100);
					continue;
				}

				var msWaited = (DateTime.UtcNow - notification.EnqueuedTimestamp).TotalMilliseconds;

				lock (measurements)
				{
					measurements.Add(new WaitTimeMeasurement((long) msWaited));
				}

				//Log.Info("Waited: {0} ms", msWaited);
				 
                var sendStart = DateTime.UtcNow;

			    sendCount++;

			    Interlocked.Increment(ref totalSendCount);

                if (sendCount % 1000 == 0)
                    Log.Info("{0}> Send Count: {1} ({2})", id, sendCount, Interlocked.Read(ref totalSendCount));

				channel.SendNotification(notification, (sender, result) =>
					{
						Interlocked.Decrement(ref trackedNotificationCount);
                        
						var sendTime = DateTime.UtcNow - sendStart;

                        lock (sendTimeMeasurements)
                        {
                            sendTimeMeasurements.Add(new WaitTimeMeasurement((long)sendTime.TotalMilliseconds));
                        }

						//Log.Info("Send Time: " + sendTime.TotalMilliseconds + " ms");

						//Trigger 
						if (this.BlockOnMessageResult)	
							waitForNotification.Set();						

						//Handle the notification send callback here
						if (result.ShouldRequeue)
						{
							var eventArgs = new NotificationRequeueEventArgs(result.Notification, result.Error);
							var evt = this.OnNotificationRequeue;
							if (evt != null)
								evt(this, eventArgs);

							//See if the requeue was cancelled in the event args
							if (!eventArgs.Cancel)
								this.QueueNotification(result.Notification, result.CountsAsRequeue, true, true);
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

					Log.Info("Notification send timeout");

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
			    this.Id = Guid.NewGuid().ToString();
				this.CancelTokenSource = new CancellationTokenSource();
				this.Channel = channel;
				this.WorkerTask = Task.Factory.StartNew(() => worker(channel, this.CancelTokenSource),
				                                        TaskCreationOptions.LongRunning);
			}

			public void Dispose()
			{
				CancelTokenSource.Cancel();
			}

            public string Id { get; private set; }

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
		public NotificationRequeueEventArgs(INotification notification, Exception cause) 
		{
			this.Cancel = false;
			this.Notification = notification;
			this.RequeueCause = cause;
		}

		public bool Cancel { get;set; }
		public INotification Notification { get; private set; }
		public Exception RequeueCause { get; private set; }
	}
}
