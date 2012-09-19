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
		public ChannelEvents Events;

		public bool WaitForQueuesToFinish { get; private set; }
				
		Apple.ApplePushService appleService = null;
		Android.C2dmPushService androidService = null;
		WindowsPhone.WindowsPhonePushService wpService = null;
		Windows.WindowsPushService winService = null;
		Blackberry.BlackberryPushService bbService = null;
		Android.GcmPushService gcmService = null;

		static PushService instance = null;
		public static PushService Instance
		{
			get
			{
				if (instance == null)
					instance = new PushService();

				return instance;
			}
		}

		public PushService()
		{
			this.Events = new ChannelEvents();
		}

		public PushService(bool waitForQueuesToFinish) : this()
		{
			this.WaitForQueuesToFinish = waitForQueuesToFinish;
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

		[Obsolete("Google has Deprecated C2DM, and you should now use GCM Instead.  See the StartGoogleCloudMessagingPushService(...) method!")]
		public void StartAndroidPushService(Android.C2dmPushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			androidService = new Android.C2dmPushService(channelSettings, serviceSettings);
			androidService.Events.RegisterProxyHandler(this.Events);
		}

		[Obsolete("Google has Deprecated C2DM, and you should now use GCM Instead.  See the StopGoogleCloudMessagingPushService() method!")]
		public void StopAndroidPushService(bool waitForQueueToFinish = true)
		{
			if (androidService != null)
				androidService.Stop(waitForQueueToFinish);
		}

		public void StartGoogleCloudMessagingPushService(Android.GcmPushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			gcmService = new Android.GcmPushService(channelSettings, serviceSettings);
			gcmService.Events.RegisterProxyHandler(this.Events);
		}

		public void StopGoogleCloudMessagingPushService(bool waitForQueueToFinish = true)
		{
			if (gcmService != null)
				gcmService.Stop(waitForQueueToFinish);
		}

		public void StartWindowsPhonePushService(WindowsPhone.WindowsPhonePushChannelSettings channelSettings = null, PushServiceSettings serviceSettings = null)
		{
			wpService = new WindowsPhone.WindowsPhonePushService(channelSettings, serviceSettings);
			wpService.Events.RegisterProxyHandler(this.Events);
		}

		public void StopWindowsPhonePushService(bool waitForQueueToFinish = true)
		{
			if (wpService != null)
				wpService.Stop(waitForQueueToFinish);
		}

		public void StartWindowsPushService(Windows.WindowsPushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			winService = new Windows.WindowsPushService(channelSettings, serviceSettings);
			winService.Events.RegisterProxyHandler(this.Events);
		}

		public void StopWindowsPushService(bool waitForQueueToFinish = true)
		{
			if (winService != null)
				winService.Stop(waitForQueueToFinish);
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
				case PlatformType.AndroidC2dm:
					androidService.QueueNotification(notification);
					break;
				case PlatformType.AndroidGcm:
					gcmService.QueueNotification(notification);
					break;
				case PlatformType.WindowsPhone:
					wpService.QueueNotification(notification);
					break;
				case PlatformType.Windows:
					winService.QueueNotification(notification);
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

			if (gcmService != null && !gcmService.IsStopping)
				tasks.Add(Task.Factory.StartNew(() => gcmService.Stop(waitForQueuesToFinish)));

			if (wpService != null && !wpService.IsStopping)
				tasks.Add(Task.Factory.StartNew(() => wpService.Stop(waitForQueuesToFinish)));

			if (winService != null && !winService.IsStopping)
				tasks.Add(Task.Factory.StartNew(() => winService.Stop(waitForQueuesToFinish)));

			if (bbService != null && !bbService.IsStopping)
				tasks.Add(Task.Factory.StartNew(() => bbService.Stop(waitForQueuesToFinish)));

			Task.WaitAll(tasks.ToArray());
		}

		void IDisposable.Dispose()
		{
			StopAllServices(this.WaitForQueuesToFinish);
		}
	}	
}
