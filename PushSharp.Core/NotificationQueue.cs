using System;
using System.Collections.Generic;
using System.Linq;

namespace PushSharp.Core
{
	public class NotificationQueue
	{
		public NotificationQueue ()
		{
			notifications = new List<INotification> ();
			lockObj = new object ();
		}

		object lockObj;
		List<INotification> notifications;

		public void Enqueue(INotification notification)
		{
			lock (lockObj)
				notifications.Add (notification);
		}

		public void EnqueueAtStart(INotification notification)
		{
			lock(lockObj)
				notifications.Insert(0, notification);
		}

		public void EnqueueAt(INotification notification, int index = 0)
		{
			lock(lockObj)
				notifications.Insert (index, notification);
		}

		public INotification Dequeue()
		{
			var n = default(INotification);

			lock (lockObj)
			{
				if (notifications.Count > 0)
				{
					n = notifications [0];
					notifications.RemoveAt (0);
				}
			}

			return n;
		}

		public int Count
		{
			get 
			{
				lock (lockObj)
					return notifications.Count;
			}
		}
	}
}

