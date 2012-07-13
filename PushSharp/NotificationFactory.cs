﻿using System;

namespace PushSharp
{
	public class NotificationFactory
	{
		public static Common.Notification Create(Common.PlatformType platform)
		{
			switch (platform)
			{
				case Common.PlatformType.Apple:
					return Apple();
				case Common.PlatformType.AndroidC2dm:
					return Android();
				case Common.PlatformType.WindowsPhone:
					return WindowsPhone();
				case Common.PlatformType.Blackberry:
					return Blackberry();
				default:
					return null;
			}
		}

		public static Apple.AppleNotification Apple()
		{
			return new Apple.AppleNotification();
		}

		[Obsolete("Google has Deprecated C2DM, and you should now use GCM Instead.  See the AndroidGcm() factory method!")]
		public static Android.AndroidNotification Android()
		{
			return new Android.AndroidNotification();
		}

		public static Android.GcmNotification AndroidGcm()
		{
			return new Android.GcmNotification();
		}

		public static WindowsPhone.WindowsPhoneNotificationFactory WindowsPhone()
		{
			return new WindowsPhone.WindowsPhoneNotificationFactory();
		}

		public static Blackberry.BlackberryNotification Blackberry()
		{
			return new Blackberry.BlackberryNotification();
		}
	}
}
