using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Apple;
using PushSharp.Core;

namespace PushSharp
{
	public static class FluentNotification
	{
		public static AppleNotification ForDeviceToken(this AppleNotification n, string deviceToken)
		{
			n.DeviceToken = deviceToken;
			return n;
		}

		public static AppleNotification WithExpiry(this AppleNotification n, DateTime expiryDate)
		{
			n.Expiration = expiryDate;
			return n;
		}
		
		public static AppleNotification WithAlert(this AppleNotification n, string body)
		{
			if (n.Payload == null)
				n.Payload = new AppleNotificationPayload();

			n.Payload.Alert = new AppleNotificationAlert() { Body = body };
						
			return n;
		}

        public static AppleNotification WithAlert(this AppleNotification n, string body, string launchImage)
        {
            if (n.Payload == null)
                n.Payload = new AppleNotificationPayload();

            n.Payload.Alert = new AppleNotificationAlert() { Body = body, LaunchImage = launchImage };

            return n;
        }

		public static AppleNotification WithAlert(this AppleNotification n, AppleNotificationAlert alert)
		{
			if (n.Payload == null)
				n.Payload = new AppleNotificationPayload();

			n.Payload.Alert = alert;
			
			return n;
		}

		public static AppleNotification WithAlert(this AppleNotification n, string body, string localizedKey, string actionLocalizedKey, IEnumerable<object> localizedArgs)
		{
			if (n.Payload == null)
				n.Payload = new AppleNotificationPayload();

			n.Payload.Alert = new AppleNotificationAlert() { Body = body, LocalizedKey = localizedKey, ActionLocalizedKey = actionLocalizedKey, LocalizedArgs = localizedArgs.ToList() };

			return n;
		}

        public static AppleNotification WithAlert(this AppleNotification n, string body, string localizedKey, string actionLocalizedKey, IEnumerable<object> localizedArgs, string launchImage)
        {
            if (n.Payload == null)
                n.Payload = new AppleNotificationPayload();

            n.Payload.Alert = new AppleNotificationAlert() { Body = body, LocalizedKey = localizedKey, ActionLocalizedKey = actionLocalizedKey, LocalizedArgs = localizedArgs.ToList(), LaunchImage = launchImage };

            return n;
        }

		public static AppleNotification WithBadge(this AppleNotification n, int badge)
		{
			if (n.Payload == null)
				n.Payload = new AppleNotificationPayload();
			
			n.Payload.Badge = badge;

			return n;
		}

		public static AppleNotification WithSound(this AppleNotification n, string sound)
		{
			if (n.Payload == null)
				n.Payload = new AppleNotificationPayload();

			n.Payload.Sound = sound;
			
			return n;
		}

		public static AppleNotification WithContentAvailable(this AppleNotification n, int contentAvailableCount)
		{
			if (n.Payload == null)
				n.Payload = new AppleNotificationPayload();

			n.Payload.ContentAvailable = contentAvailableCount;

			return n;
		}

		public static AppleNotification WithPayload(this AppleNotification n, AppleNotificationPayload payload)
		{
			n.Payload = payload;

			return n;
		}

		public static AppleNotification WithCustomItem(this AppleNotification n, string key, params object[] values)
		{
			if (n.Payload == null)
				n.Payload = new AppleNotificationPayload();

			n.Payload.AddCustom(key, values);

			return n;
		}

        public static AppleNotification WithPasskitUpdate(this AppleNotification n)
        {
            var payLoad = new AppleNotificationPayload();
            payLoad.AddCustom("aps", string.Empty);

            n.Payload = payLoad;

            return n;
        }

        public static AppleNotification WithTag(this AppleNotification n, object tag)
        {
            n.Tag = tag;

            return n;
        }

		public static AppleNotification WithCategory(this AppleNotification n, string category)
		{
			if (n.Payload == null)
				n.Payload = new AppleNotificationPayload();

			n.Payload.Category = category;

			return n;
		}

		public static AppleNotification HideActionButton(this AppleNotification n)
		{
			if (n.Payload == null)
				n.Payload = new AppleNotificationPayload();

			n.Payload.HideActionButton = true;

			return n;
		}

	}
}
