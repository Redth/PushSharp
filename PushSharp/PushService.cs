using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PushSharp.Common;

namespace PushSharp
{
	public class PushService : IDisposable
	{
		//public PushSettings Settings { get; private set; }

		public ChannelEvents Events;
		bool stopping;

		Apple.ApplePushService appleService = null;
		Android.AndroidPushService androidService = null;
		WindowsPhone.WindowsPhonePushService wpService = null;
		Blackberry.BlackberryPushService bbService = null;

		public PushService()
		{
			//this.Settings = settings;
			stopping = false;
			this.Events = new ChannelEvents();
		}

		public void StartApplePushService(Apple.ApplePushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			appleService = new Apple.ApplePushService(channelSettings, serviceSettings);
			appleService.Events.RegisterProxyHandler(this.Events);
		}

		public void StopApplePushService(bool waitForQueueToFinish = true)
		{
			if (appleService != null)
				appleService.Stop(waitForQueueToFinish);
		}

		public void StartAndroidPushService(Android.AndroidPushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			androidService = new Android.AndroidPushService(channelSettings, serviceSettings);
			androidService.Events.RegisterProxyHandler(this.Events);
		}

		public void StopAndroidPushService(bool waitForQueueToFinish = true)
		{
			if (androidService != null)
				androidService.Stop(waitForQueueToFinish);
		}

		public void StartWindowsPhonePushService(WindowsPhone.WindowsPhonePushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			wpService = new WindowsPhone.WindowsPhonePushService(channelSettings, serviceSettings);
			wpService.Events.RegisterProxyHandler(this.Events);
		}

		public void StopWindowsPhonePushService(bool waitForQueueToFinish = true)
		{
			if (wpService != null)
				wpService.Stop(waitForQueueToFinish);
		}

		public void StartBlackberryPushService(Blackberry.BlackberryPushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			bbService = new Blackberry.BlackberryPushService(channelSettings, serviceSettings);
			bbService.Events.RegisterProxyHandler(this.Events);
		}
		
		public void StopBlackberryPushService(bool waitForQueueToFinish = true)
		{
			if (bbService != null)
				bbService.Stop(waitForQueueToFinish);
		}

		public void QueueNotification(Notification notification)
		{
			switch (notification.Platform)
			{
				case PlatformType.Apple:
					appleService.QueueNotification(notification);
					break;
				case PlatformType.Android:
					androidService.QueueNotification(notification);
					break;
				case PlatformType.WindowsPhone:
					wpService.QueueNotification(notification);
					break;
				case PlatformType.Blackberry:
					bbService.QueueNotification(notification);
					break;
			}
		}

		public void StopAllServices(bool waitForQueuesToFinish = true)
		{
			var tasks = new List<Task>();

			if (appleService != null && !appleService.IsStopping)
				tasks.Add(Task.Factory.StartNew(() => appleService.Stop(waitForQueuesToFinish)));

			if (androidService != null && !androidService.IsStopping)
				tasks.Add(Task.Factory.StartNew(() => androidService.Stop(waitForQueuesToFinish)));

			if (wpService != null && !wpService.IsStopping)
				tasks.Add(Task.Factory.StartNew(() => wpService.Stop(waitForQueuesToFinish)));

			if (bbService != null && !bbService.IsStopping)
				tasks.Add(Task.Factory.StartNew(() => bbService.Stop(waitForQueuesToFinish)));

			Task.WaitAll(tasks.ToArray());
		}

		void IDisposable.Dispose()
		{
			StopAllServices(false);
		}
	}

	
}
