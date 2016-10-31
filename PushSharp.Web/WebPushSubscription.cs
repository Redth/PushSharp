using System;

namespace PushSharp.Web
{
    public class WebPushSubscription
    {
        public string EndPoint { get; set; }

        public WebSubscriptionKeys Keys { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(EndPoint))
            {
                throw new Exception("Empty EndPoint.");
            }

            if (string.IsNullOrEmpty(Keys.Auth))
            {
                throw new Exception("Empty Auth key.");
            }

            if (string.IsNullOrEmpty(Keys.P256dh))
            {
                throw new Exception("Empty P256dh key.");
            }
        }
    }
}