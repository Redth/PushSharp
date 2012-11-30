using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PushSharp.Common
{
	public abstract class PushServiceBase<TChannelSettings> : IDisposable where TChannelSettings : PushChannelSettings
	{
		public ChannelEvents Events = new ChannelEvents();

		public abstract PlatformType Platform { get; }
		
		public PushServiceSettings ServiceSettings { get; private set; }
		public TChannelSettings ChannelSettings { get; private set; }
		public bool IsStopping { get { return stopping; } }

		Timer timerCheckScale;
		Task distributerTask;
		bool stopping;
		List<PushChannelBase> channels = new List<PushChannelBase>();
		ConcurrentQueue<Notification> queuedNotifications = new ConcurrentQueue<Notification>();
		CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
		List<double> measurements = new List<double>();

		protected abstract PushChannelBase CreateChannel(PushChannelSettings channelSettings);

		public PushServiceBase(TChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			this.ServiceSettings = serviceSettings ?? new PushServiceSettings();
			this.ChannelSettings = channelSettings;

			this.queuedNotifications = new ConcurrentQueue<Notification>();

			timerCheckScale = new Timer(new TimerCallback((state) =>
			{
				CheckScale();
			}), null, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(15));
			
			CheckScale();

			distributerTask = new Task(Distributer, TaskCreationOptions.LongRunning);
			distributerTask.ContinueWith((ft) =>
			{
				var ex = ft.Exception;

			}, TaskContinuationOptions.OnlyOnFaulted);
			distributerTask.Start();

			stopping = false;
		}
				
		public void QueueNotification(Notification notification)
		{
			notification.EnqueuedTimestamp = DateTime.UtcNow;

			queuedNotifications.Enqueue(notification);
		}

		public void Stop(bool waitForQueueToFinish)
		{
			stopping = true;

			//Stop the timer for checking scale
			if (this.timerCheckScale != null)
				this.timerCheckScale.Change(Timeout.Infinite, Timeout.Infinite);

			//Stop all channels
			Parallel.ForEach<PushChannelBase>(channels,
				(channel) =>
				{
					channel.Stop(waitForQueueToFinish);
					channel.Events.UnRegisterProxyHandler(this.Events);
				});

			this.channels.Clear();
			
			//Sleep a bit to avoid race conditions
			Thread.Sleep(2000);

			this.cancelTokenSource.Cancel();
		}

		public void Dispose()
		{
			if (!stopping)
				Stop(false);
		}

		void Distributer()
		{
			while (!this.cancelTokenSource.IsCancellationRequested)
			{
				if (channels == null || channels.Count <= 0)
				{
					Thread.Sleep(250);
					continue;
				}

				Notification notification = null;

				if (!queuedNotifications.TryDequeue(out notification))
				{
					//No notifications in queue, sleep a bit!
					Thread.Sleep(250);
					continue;
				}

				PushChannelBase channelOn = null;

				//Get the channel with the smallest queue
				if (channels.Count == 1)
					channelOn = channels[0];
				else
					channelOn = (from c in channels
								 orderby c.QueuedNotificationCount
								 select c).FirstOrDefault();

				if (channelOn != null)
				{
					//Measure when the message entered the queue
					notification.EnqueuedTimestamp = DateTime.UtcNow;

					channelOn.QueueNotification(notification);
				}
			}
		}

		void CheckScale()
		{
			if (ServiceSettings.AutoScaleChannels && !this.cancelTokenSource.IsCancellationRequested && !stopping)
			{
				if (channels.Count <= 0)
				{
					ScaleChannels(ChannelScaleAction.Create);
					return;
				}

				var avgTime = GetAverageQueueWait();

                if (avgTime < ServiceSettings.MinAvgTimeToScaleChannels && channels.Count > 1)
				{
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

					ScaleChannels(ChannelScaleAction.Create, numChannelsToSpinUp);
				}
			}
			else
			{
				while (channels.Count > ServiceSettings.Channels && !this.cancelTokenSource.IsCancellationRequested && !stopping)
					ScaleChannels(ChannelScaleAction.Destroy);

				while (channels.Count < ServiceSettings.Channels && !this.cancelTokenSource.IsCancellationRequested && !stopping)
					ScaleChannels(ChannelScaleAction.Create);
			}
		}

		void newChannel_OnQueueTimed(double queueTimeMilliseconds)
		{
			//We got a measurement for how long a message waited in the queue
			lock (measurements)
			{
				measurements.Add(queueTimeMilliseconds);

				//Remove old measurements
				while (measurements.Count > 1000)
					measurements.RemoveAt(0);
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

				lock (channels)
				{
					if (action == ChannelScaleAction.Create)
					{
						var newChannel = this.CreateChannel(this.ChannelSettings);

						newChannel.Events.RegisterProxyHandler(this.Events);

						newChannel.OnQueueTimed += new Action<double>(newChannel_OnQueueTimed);

						channels.Add(newChannel);

						newCount = channels.Count;
						destroyed = false;
					}
					else if (action == ChannelScaleAction.Destroy && channels.Count > 1)
					{
						var channelOn = channels[0];
						channels.RemoveAt(0);

						//Now stop the channel but let it finish
						channelOn.Stop(true);

						channelOn.Events.UnRegisterProxyHandler(this.Events);

						newCount = channels.Count;
						destroyed = true;
					}
				}

				if (destroyed.HasValue && !destroyed.Value)
					this.Events.RaiseChannelCreated(this.Platform, newCount);
				else if (destroyed.HasValue && destroyed.Value)
					this.Events.RaiseChannelDestroyed(this.Platform, newCount);
			}
		}
	}

	public enum ChannelScaleAction
	{
		Create,
		Destroy
	}
}
