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
		internal event Action<double> OnQueueTimed;

		public ChannelEvents Events = new ChannelEvents();

		public PushChannelSettings Settings { get; private set; }
		protected bool stopping;

		protected abstract void SendNotification(Notification notification);

		public PushChannelBase(PushChannelSettings settings)
		{
			this.stopping = false;
			this.CancelTokenSource = new CancellationTokenSource();
			this.CancelToken = CancelTokenSource.Token;

			this.queuedNotifications = new ConcurrentQueue<Notification>();
		
			this.Settings = settings;

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
				{
					Console.WriteLine("Waiting for Queue: " + QueuedNotificationCount);
					Thread.Sleep(50);
				}
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

		
		object queuedNotificationsLock = new object();
		ConcurrentQueue<Notification> queuedNotifications;

		protected Task taskSender;

		protected CancellationTokenSource CancelTokenSource;
		protected CancellationToken CancelToken;


		public int QueuedNotificationCount
		{
			get { return queuedNotifications.Count; }
		}

		public void QueueNotification(Notification notification)
		{
			queuedNotifications.Enqueue(notification);
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
