using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Net.Sockets;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace PushSharp.Common
{
	public abstract class PushChannelBase : IDisposable
	{
		public ChannelEvents Events = new ChannelEvents();
		
		public PushChannelSettings ChannelSettings { get; private set; }
		public PushServiceSettings ServiceSettings { get; private set; }

		internal event Action<double> OnQueueTimed;

		object queuedNotificationsLock = new object();
		ConcurrentQueue<Notification> queuedNotifications;
		
		protected bool stopping;
		protected Task taskSender;
		protected CancellationTokenSource CancelTokenSource;
		protected CancellationToken CancelToken;

		protected abstract void SendNotification(Notification notification);

		public PushChannelBase(PushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			this.stopping = false;
			this.CancelTokenSource = new CancellationTokenSource();
			this.CancelToken = CancelTokenSource.Token;

			this.queuedNotifications = new ConcurrentQueue<Notification>();
		
			this.ChannelSettings = channelSettings;
			this.ServiceSettings = serviceSettings ?? new PushServiceSettings();

			//Start our sending task
			taskSender = new Task(() => Sender(), TaskCreationOptions.LongRunning);
			taskSender.ContinueWith((t) => { var ex = t.Exception; }, TaskContinuationOptions.OnlyOnFaulted);
			taskSender.Start();
		}

		public virtual void Stop(bool waitForQueueToDrain)
		{
			stopping = true;

			//See if we want to wait for the queue to drain before stopping
			if (waitForQueueToDrain)
			{
				while (QueuedNotificationCount > 0)
					Thread.Sleep(50);
			}

			//Sleep a bit to prevent any race conditions
			Thread.Sleep(2000);

			if (!CancelTokenSource.IsCancellationRequested)
				CancelTokenSource.Cancel();

			//Wait on our tasks for a maximum of 30 seconds
			Task.WaitAll(new Task[] { taskSender }, 30000);
		}

		public virtual void Dispose()
		{
			//Stop without waiting
			if (!stopping)
				Stop(false);
		}

		public int QueuedNotificationCount
		{
			get { return queuedNotifications.Count; }
		}

		public void QueueNotification(Notification notification, bool countsAsRequeue = true)
		{
			//If the count is -1, it can be queued infinitely, otherwise check that it's less than the max
			if (this.ServiceSettings.MaxNotificationRequeues < 0 || notification.QueuedCount <= this.ServiceSettings.MaxNotificationRequeues)
			{
				//Increase the queue counter
				if (countsAsRequeue)
					notification.QueuedCount++;

				queuedNotifications.Enqueue(notification);
			}
			else
				Events.RaiseNotificationSendFailure(notification, new MaxSendAttemptsReachedException());
		}

		void Sender()
		{
			while (!this.CancelToken.IsCancellationRequested)
			{
				Notification notification = null;

				if (!queuedNotifications.TryDequeue(out notification))
				{
					//No notifications in queue, sleep a bit!
					Thread.Sleep(250);
					continue;
				}

				//Report back the time in queue
				var timeInQueue = DateTime.UtcNow - notification.EnqueuedTimestamp;
				if (OnQueueTimed != null)
					OnQueueTimed(timeInQueue.TotalMilliseconds);

				//Send it
				this.SendNotification(notification);
			}
		}

	}
}
