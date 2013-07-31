using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.ServiceModel.Web;

namespace PushSharp.Apple
{
	public class AppleNotification : Core.Notification
	{
		static object nextIdentifierLock = new object();
		static int nextIdentifier = 0;

		static int GetNextIdentifier()
		{
			lock(nextIdentifierLock)
			{
				if (nextIdentifier >= int.MaxValue - 10)
					nextIdentifier = 1;

				return nextIdentifier++;
			}
		}

		/// <summary>
		/// DO NOT Call this unless you know what you are doing!
		/// </summary>
		public static void ResetIdentifier()
		{
			lock(nextIdentifierLock)
				nextIdentifier = 0;
		}

		public int Identifier { get; private set; }
		public string DeviceToken { get; set; }
		public AppleNotificationPayload Payload { get; set; }
		/// <summary>
		/// The expiration date after which Apple will no longer store and forward this push notification.
		/// If no value is provided, an assumed value of one year from now is used.  If you do not wish
		/// for Apple to store and forward, set this value to Notification.DoNotStore.
		/// </summary>
		public DateTime? Expiration { get; set; }
		public const int DEVICE_TOKEN_BINARY_SIZE = 32;
		public const int DEVICE_TOKEN_STRING_SIZE = 64;
		public const int MAX_PAYLOAD_SIZE = 256;
		public static readonly DateTime DoNotStore = DateTime.MinValue;
		private static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public AppleNotification()
		{
			DeviceToken = string.Empty;
			Payload = new AppleNotificationPayload();

			Identifier = GetNextIdentifier();
		}

		public AppleNotification(string deviceToken)
		{
			if (!string.IsNullOrEmpty(deviceToken) && deviceToken.Length != DEVICE_TOKEN_STRING_SIZE)
				throw new NotificationFailureException(5, this); //Invalid DeviceToken Length

			DeviceToken = deviceToken;
			Payload = new AppleNotificationPayload();
			
			Identifier = GetNextIdentifier();
		}

		public AppleNotification(string deviceToken, AppleNotificationPayload payload)
		{
			if (!string.IsNullOrEmpty(deviceToken) && deviceToken.Length != DEVICE_TOKEN_STRING_SIZE)
				throw new NotificationFailureException(5, this); //Invalid DeviceToken Length

			DeviceToken = deviceToken;
			Payload = payload;

			Identifier = GetNextIdentifier();
		}

		public override bool IsValidDeviceRegistrationId()
		{
			var r = new System.Text.RegularExpressions.Regex(@"^[0-9A-F]+$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			return r.Match(this.DeviceToken).Success;
		}

		public override string ToString()
		{
			try
			{ 
				if (Payload != null)
					return Payload.ToJson();
			}
			catch { }

			return "{}";
		}

		public byte[] ToBytes()
		{
			// Without reading the response which would make any identifier useful, it seems silly to
			// expose the value in the object model, although that would be easy enough to do. For
			// now we'll just use zero.
			byte[] identifierBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Identifier));

			// APNS will not store-and-forward a notification with no expiry, so set it one year in the future
			// if the client does not provide it.
			int expiryTimeStamp = -1;
			if (Expiration != DoNotStore)
			{
				DateTime concreteExpireDateUtc = (Expiration ?? DateTime.UtcNow.AddMonths(1)).ToUniversalTime();
				TimeSpan epochTimeSpan = concreteExpireDateUtc - UNIX_EPOCH;
				expiryTimeStamp = (int)epochTimeSpan.TotalSeconds;
			}

			byte[] expiry = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(expiryTimeStamp));

			if (string.IsNullOrEmpty (this.DeviceToken))
				throw new NotificationFailureException (2, this);

			if (!IsValidDeviceRegistrationId ())
				throw new NotificationFailureException (8, this);

			byte[] deviceToken = new byte[DeviceToken.Length / 2];
			for (int i = 0; i < deviceToken.Length; i++)
			{
				try { deviceToken[i] = byte.Parse(DeviceToken.Substring(i*2, 2), System.Globalization.NumberStyles.HexNumber); }
				catch (Exception) { throw new NotificationFailureException(8, this); }
			}

			if (deviceToken.Length != DEVICE_TOKEN_BINARY_SIZE)
				throw new NotificationFailureException(5, this);


			byte[] deviceTokenSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(deviceToken.Length)));

			byte[] payload = Encoding.UTF8.GetBytes(Payload.ToJson());
			if (payload.Length > MAX_PAYLOAD_SIZE)
			{
				int newSize = Payload.Alert.Body.Length - (payload.Length - MAX_PAYLOAD_SIZE);
				if (newSize > 0)
				{
					Payload.Alert.Body = Payload.Alert.Body.Substring(0, newSize);
					payload = Encoding.UTF8.GetBytes(Payload.ToString());
				}
				else
				{
					do
					{
						Payload.Alert.Body = Payload.Alert.Body.Remove(Payload.Alert.Body.Length - 1);
						payload = Encoding.UTF8.GetBytes(Payload.ToString());
					}
					while (payload.Length > MAX_PAYLOAD_SIZE && !string.IsNullOrEmpty(Payload.Alert.Body));
				}

				if (payload.Length > MAX_PAYLOAD_SIZE)
					throw new NotificationFailureException(7, this);
			}
			byte[] payloadSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(payload.Length)));

			//int bufferSize = sizeof(Byte) + deviceTokenSize.Length + deviceToken.Length + payloadSize.Length + payload.Length;
			//byte[] buffer = new byte[bufferSize];

			List<byte[]> notificationParts = new List<byte[]>();

			notificationParts.Add(new byte[] { 0x01 }); // Enhanced notification format command
			notificationParts.Add(identifierBytes);
			notificationParts.Add(expiry);
			notificationParts.Add(deviceTokenSize);
			notificationParts.Add(deviceToken);
			notificationParts.Add(payloadSize);
			notificationParts.Add(payload);

			return BuildBufferFrom(notificationParts);
		}

		private byte[] BuildBufferFrom(IList<byte[]> bufferParts)
		{
			int bufferSize = 0;
			for (int i = 0; i < bufferParts.Count; i++)
				bufferSize += bufferParts[i].Length;

			byte[] buffer = new byte[bufferSize];
			int position = 0;
			for (int i = 0; i < bufferParts.Count; i++)
			{
				byte[] part = bufferParts[i];
				Buffer.BlockCopy(bufferParts[i], 0, buffer, position, part.Length);
				position += part.Length;
			}
			return buffer;
		}
	}
}
