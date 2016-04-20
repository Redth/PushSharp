using System;
using PushSharp.Core;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace PushSharp.Apple
{
    public class ApnsNotification : INotification
    {
        static readonly object nextIdentifierLock = new object ();
        static int nextIdentifier = 1;

        static int GetNextIdentifier ()
        {
            lock (nextIdentifierLock) {
                if (nextIdentifier >= int.MaxValue - 10)
                    nextIdentifier = 1;

                return nextIdentifier++;
            }
        }

        /// <summary>
        /// DO NOT Call this unless you know what you are doing!
        /// </summary>
        public static void ResetIdentifier ()
        {
            lock (nextIdentifierLock)
                nextIdentifier = 0;
        }

        public object Tag { get; set; }

        public int Identifier { get; private set; }

        public string DeviceToken { get; set; }

        public JObject Payload { get; set; }

        /// <summary>
        /// The expiration date after which Apple will no longer store and forward this push notification.
        /// If no value is provided, an assumed value of one year from now is used.  If you do not wish
        /// for Apple to store and forward, set this value to Notification.DoNotStore.
        /// </summary>
        public DateTime? Expiration { get; set; }

        public bool LowPriority { get; set; }

        public const int DEVICE_TOKEN_BINARY_MIN_SIZE = 32;
        public const int DEVICE_TOKEN_STRING_MIN_SIZE = 64;
        public const int MAX_PAYLOAD_SIZE = 2048; //will be 4096 soon
        public static readonly DateTime DoNotStore = DateTime.MinValue;
        private static readonly DateTime UNIX_EPOCH = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public ApnsNotification () : this (string.Empty, new JObject ())
        {
        }

        public ApnsNotification (string deviceToken) : this (deviceToken, new JObject ())
        {
        }

        public ApnsNotification (string deviceToken, JObject payload)
        {
            if (!string.IsNullOrEmpty (deviceToken) && deviceToken.Length < DEVICE_TOKEN_STRING_MIN_SIZE)
                throw new NotificationException ("Invalid DeviceToken Length", this);

            DeviceToken = deviceToken;
            Payload = payload;

            Identifier = GetNextIdentifier ();
        }

        public bool IsDeviceRegistrationIdValid ()
        {
            var r = new System.Text.RegularExpressions.Regex (@"^[0-9A-F]+$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return r.Match (this.DeviceToken).Success;
        }

        public override string ToString ()
        {
            try { 
                if (Payload != null)
                     return Payload.ToString(Newtonsoft.Json.Formatting.None);
            } catch {
            }

            return "{}";
        }

        public byte[] ToBytes ()
        {
            var builder = new List<byte> ();

            // 1 - Device Token
            if (string.IsNullOrEmpty (this.DeviceToken))
                throw new NotificationException ("Missing DeviceToken", this);

            if (!IsDeviceRegistrationIdValid ())
                throw new NotificationException ("Invalid DeviceToken", this);

            // Turn the device token into bytes
            byte[] deviceToken = new byte[DeviceToken.Length / 2];
            for (int i = 0; i < deviceToken.Length; i++) {
                try {
                    deviceToken [i] = byte.Parse (DeviceToken.Substring (i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                } catch (Exception) {
                    throw new NotificationException ("Invalid DeviceToken", this);
                }
            }

            if (deviceToken.Length < DEVICE_TOKEN_BINARY_MIN_SIZE)
                throw new NotificationException ("Invalid DeviceToken Length", this);

            builder.Add (0x01); // Device Token ID
            builder.AddRange (BitConverter.GetBytes (IPAddress.HostToNetworkOrder (Convert.ToInt16 (deviceToken.Length))));
            builder.AddRange (deviceToken);

            // 2 - Payload
            var payload = Encoding.UTF8.GetBytes (ToString ());
            if (payload.Length > MAX_PAYLOAD_SIZE)
                throw new NotificationException ("Payload too large (must be " + MAX_PAYLOAD_SIZE + " bytes or smaller", this);

            builder.Add (0x02); // Payload ID
            builder.AddRange (BitConverter.GetBytes (IPAddress.HostToNetworkOrder (Convert.ToInt16 (payload.Length))));
            builder.AddRange (payload);

            // 3 - Identifier
            builder.Add (0x03);
            builder.AddRange (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((Int16)4)));
            builder.AddRange (BitConverter.GetBytes (IPAddress.HostToNetworkOrder (Identifier)));

            // 4 - Expiration
            // APNS will not store-and-forward a notification with no expiry, so set it one year in the future
            // if the client does not provide it.
            int expiryTimeStamp = -1;
            if (Expiration != DoNotStore) {
                DateTime concreteExpireDateUtc = (Expiration ?? DateTime.UtcNow.AddMonths (1)).ToUniversalTime ();
                TimeSpan epochTimeSpan = concreteExpireDateUtc - UNIX_EPOCH;
                expiryTimeStamp = (int)epochTimeSpan.TotalSeconds;
            }

            builder.Add (0x04); // 4 - Expiry ID
            builder.AddRange (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((Int16)4)));
            builder.AddRange (BitConverter.GetBytes (IPAddress.HostToNetworkOrder (expiryTimeStamp)));

            // 5 - Priority
            //TODO: Add priority
            var priority = LowPriority ? (byte)5 : (byte)10;
            builder.Add (0x05); // 5 - Priority
            builder.AddRange (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((Int16)1)));
            builder.Add (priority);

            var frameLength = builder.Count;

            builder.Insert (0, 0x02); // COMMAND 2 for new format

            // Insert the frame length
            builder.InsertRange (1, BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((Int32)frameLength)));

            return builder.ToArray ();
        }

    }
}

