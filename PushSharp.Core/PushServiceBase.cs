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
	public delegate void ChannelExceptionDelegate(object sender, IPushChannel pushChannel, Exception error);
	public delegate void ServiceExceptionDelegate(object sender, Exception error);
	public delegate void DeviceSubscriptionExpiredDelegate(object sender, string expiredSubscriptionId, DateTime expirationDateUtc, INotification notification);
	
	public abstract class PushServiceBase : IPushService
	{
		public event ChannelCreatedDelegate OnChannelCreated;
		public event ChannelDestroyedDelegate OnChannelDestroyed;
		public event NotificationSentDelegate OnNotificationSent;
		public event NotificationFailedDelegate OnNotificationFailed;
		public event ChannelExceptionDelegate OnChannelException;
		public event ServiceExceptionDelegate OnServiceException;
		public event DeviceSubscriptionExpiredDelegate OnDeviceSubscriptionExpired;

		protected void RaiseSubscriptionExpired(string expiredSubscriptionId, DateTime expirationDateUtc, INotification notification)
		{
			var evt = OnDeviceSubscriptionExpired;
			if (evt != null)
				evt(this, expiredSubscriptionId, expirationDateUtc, notification);
		}

		protected void RaiseServiceException(Exception error)
		{
			var evt = OnServiceException;
			if (evt != null)
				evt(this, error);
		}

		public IPushChannelFactory PushChannelFactory { get; private set; }
		public IPushServiceSettings ServiceSettings { get; private set; }
		public IPushChannelSettings ChannelSettings { get; private set; }
		public bool IsStopping { get { return stopping; } }

		Timer timerCheckScale;
		Task distributerTask;
		bool stopping;
		List<IPushChannel> channels = new List<IPushChannel>();
		private ConcurrentQueue<INotification> queuedNotifications;
		CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
		List<double> measurements = new List<double>();

		protected PushServiceBase(IPushChannelFactory pushChannelFactory, IPushChannelSettings channelSettings)
			: this(pushChannelFactory, channelSettings, default(IPushServiceSettings))
		{			
		}

		protected PushServiceBase(IPushChannelFactory pushChannelFactory, IPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
		{
			this.PushChannelFactory = pushChannelFactory;
			this.ServiceSettings = serviceSettings ?? new PushServiceSettings();
			this.ChannelSettings = channelSettings;

			this.queuedNotifications = new ConcurrentQueue<INotification>();

			timerCheckScale = new Timer(CheckScale, null, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(15));
			
			CheckScale();

			distributerTask = new Task(Distributer, TaskCreationOptions.LongRunning);
			distributerTask.ContinueWith(ft =>
			{
				var ex = ft.Exception;

				var evt = this.OnServiceException;
				if (evt != null)
					evt(this, ex);

			}, TaskContinuationOptions.OnlyOnFaulted);
			distributerTask.Start();

			stopping = false;
		}

		public void QueueNotification(INotification notification)
		{
			QueueNotification(notification, false);
		}


		void QueueNotification(INotification notification, bool countsAsRequeue = true, bool ignoreStoppingChannel = false)
		{
			if (this.cancelTokenSource.IsCancellationRequested)
				throw new ObjectDisposedException("Service", "Service has already been signaled to stop");
				
			if (this.ServiceSettings.MaxNotificationRequeues < 0 || notification.QueuedCount <= this.ServiceSettings.MaxNotificationRequeues)
			{
				//Reset the Enqueued time in case this is a requeue
				notification.EnqueuedTimestamp = DateTime.UtcNow;

				//Increase the queue counter
				if (countsAsRequeue)
					notification.QueuedCount++;

				queuedNotifications.Enqueue(notification);
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
				while (this.queuedNotifications.Count > 0)
					Thread.Sleep(100);				
			}
			
			//Stop all channels
			Parallel.ForEach(channels, (channel) => channel.Dispose());

			this.channels.Clear();
			
			this.cancelTokenSource.Cancel();
		}

		public void Dispose()
		{
			if (!stopping)
				Stop(false);
		}

		void Distributer()
		{
			var rnd = new Random();

			while (!this.cancelTokenSource.IsCancellationRequested)
			{
				if (channels == null || channels.Count <= 0)
				{
					Thread.Sleep(250);
					continue;
				}

				INotification notification;

				if (!queuedNotifications.TryDequeue(out notification))
				{
					//No notifications in queue, sleep a bit!
					Thread.Sleep(250);
					continue;
				}

				IPushChannel channelOn = null;

				lock (channels)
				{
					//Get a random channel to use
					var next = rnd.Next(0, channels.Count - 1);

					//Get the channel with the smallest queue
					if (channels.Count == 1)
						channelOn = channels[0];
					else
						channelOn = channels[next];

					if (channelOn != null)
					{
						//Measure when the message entered the queue
						notification.EnqueuedTimestamp = DateTime.UtcNow;

						Task.Factory.StartNew(() => channelOn.SendNotification(notification, (sender, result) =>
							{
								//Handle the notification send callback here
								if (result.ShouldRequeue)
									this.QueueNotification(result.Notification, true, true);

								if (!result.IsSuccess)
								{
									var evt = this.OnNotificationFailed;
									if (evt != null)
										evt(this, result.Notification, result.Error);
								}
								else
								{
									var evt = this.OnNotificationSent;
									if (evt != null)
										evt(this, result.Notification);
								}

							})).ContinueWith(t =>
								{
									var ex = t.Exception;
									var evt = this.OnChannelException;
									if (evt != null && ex != null)
										evt(this, channelOn, ex);

								}, TaskContinuationOptions.OnlyOnFaulted);
						
						//Task.Factory.StartNew(() => channelOn.SendNotification(notification));
					}
				}
			}
		}

		void CheckScale(object state = null)
		{
			Log.Info("{0} -> Checking Scale", this);

			if (ServiceSettings.AutoScaleChannels && !this.cancelTokenSource.IsCancellationRequested && !stopping)
			{
				if (channels.Count <= 0)
				{
					Log.Info("{0} -> Creating Channel", this);
					ScaleChannels(ChannelScaleAction.Create);
					return;
				}

				var avgTime = GetAverageQueueWait();

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
						numChannelsToSpinUp = 5;
					else if (avgTime > 1000)
						numChannelsToSpinUp = 2;
					else if (avgTime > ServiceSettings.MinAvgTimeToScaleChannels)
						numChannelsToSpinUp = 1;

					Log.Info("{0} -> Creating {1} Channel(s)", this, numChannelsToSpinUp);
					ScaleChannels(ChannelScaleAction.Create, numChannelsToSpinUp);
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
		
		double GetAverageQueueWait()
		{
			if (measurements == null)
				return 0;

			lock (measurements)
			{
				if (measurements.Count > 0)
					return measurements.Average();
				else
					return 0;
			}
		}

		void ScaleChannels(ChannelScaleAction action, int count = 1)
		{
			for (int i = 0; i < count; i++)
			{
				var newCount = 0;
				bool? destroyed= null;
				IPushChannel newChannel = default (IPushChannel);

				lock (channels)
				{
					if (action == ChannelScaleAction.Create)
					{
						newChannel = this.PushChannelFactory.CreateChannel(this.ChannelSettings);
					
						channels.Add(newChannel);
						
						newCount = channels.Count;
						destroyed = false;
					}
					else if (action == ChannelScaleAction.Destroy && channels.Count > 1)
					{
						var channelOn = channels[0];
						channels.RemoveAt(0);

						//Now stop the channel but let it finish
						channelOn.Dispose();

						newCount = channels.Count;
						destroyed = true;
					}
				}

				if (destroyed.HasValue && !destroyed.Value)
				{
					var evt = this.OnChannelCreated;
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
	}

	public enum ChannelScaleAction
	{
		Create,
		Destroy
	}
}
