using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PushSharp.Common
{
	public abstract class PushServiceBase : IDisposable
	{
		public ChannelEvents Events = new ChannelEvents();
		Timer timerCheckScale;
		Task distributerTask;
		bool stopping;

		public PushServiceBase(PushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			this.ServiceSettings = serviceSettings ?? new PushServiceSettings();
			this.ChannelSettings = channelSettings;

			this.queuedNotifications = new ConcurrentQueue<Notification>();

			timerCheckScale = new Timer(new TimerCallback((state) =>
			{
				CheckScale();
			}), null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
			
			CheckScale();

			distributerTask = new Task(() => { Distributer(); }, TaskCreationOptions.LongRunning);
			distributerTask.ContinueWith((ft) =>
			{
				var ex = ft.Exception;

			}, TaskContinuationOptions.OnlyOnFaulted);
			distributerTask.Start();

			stopping = false;
		}

		public PushServiceSettings ServiceSettings { get; private set; }
		public PushChannelSettings ChannelSettings { get; private set; }

		public bool IsStopping { get { return stopping; } }

		protected abstract PushChannelBase CreateChannel(PushChannelSettings channelSettings);
		
		List<PushChannelBase> channels = new List<PushChannelBase>();
		ConcurrentQueue<Notification> queuedNotifications = new ConcurrentQueue<Notification>();

		CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

		public void QueueNotification(Notification notification)
		{
			queuedNotifications.Enqueue(notification);
		}

		public void Stop(bool waitForQueueToFinish)
		{
			stopping = true;

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
					Thread.Sleep(50);
					continue;
				}

				Notification notification = null;

				if (!queuedNotifications.TryDequeue(out notification))
				{
					//No notifications in queue, sleep a bit!
					Thread.Sleep(50);
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
			if (ServiceSettings.AutoScaleChannels)
			{
				if (channels.Count <= 0)
				{
					SpinupChannel();
					return;
				}

				var avgTime = GetAverageQueueWait();

				if (avgTime < 1 && channels.Count > 1)
				{
					TeardownChannel();
				}
				else if (avgTime > 5 && channels.Count < this.ServiceSettings.MaxAutoScaleChannels)
				{
					SpinupChannel();
				}
			}
			else
			{
				while (channels.Count > ServiceSettings.Channels)
					TeardownChannel();

				while (channels.Count < ServiceSettings.Channels)
					SpinupChannel();
			}
		}


		List<double> measurements = new List<double>();

		void newChannel_OnQueueTimed(double queueTimeMilliseconds)
		{
			//We got a measurement for how long a message waited in the queue

			lock (measurements)
			{
				measurements.Add(queueTimeMilliseconds);

				while (measurements.Count > 1000)
					measurements.RemoveAt(0);
			}				
		}

		double GetAverageQueueWait()
		{
			lock (measurements)
			{
				return measurements.Average();
			}
		}

		void SpinupChannel()
		{
			lock (channels)
			{
				var newChannel = this.CreateChannel(this.ChannelSettings);

				newChannel.Events.RegisterProxyHandler(this.Events);

				newChannel.OnQueueTimed += new Action<double>(newChannel_OnQueueTimed);

				channels.Add(newChannel);
			}
		}

		void TeardownChannel()
		{
			lock (channels)
			{
				var channelOn = channels[0];
				channels.RemoveAt(0);

				channelOn.Events.UnRegisterProxyHandler(this.Events);
			}
		}

	}
}
