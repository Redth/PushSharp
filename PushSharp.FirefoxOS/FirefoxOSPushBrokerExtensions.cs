using System;

using PushSharp.FirefoxOS;

namespace PushSharp
{
    /// <summary>
    /// Extensions for registering FirefoxOS service on the push broker.
    /// </summary>
    public static class FirefoxOSPushBrokerExtensions
    {
        /// <summary>
        /// Registers FirefoxOS service.
        /// </summary>
        /// 
        /// <param name="broker">Registration broker.</param>
        public static void RegisterFirefoxOSService(this PushBroker broker)
        {
            broker.RegisterService<FirefoxOSNotification>(new FirefoxOSPushService());
        }

        /// <summary>
        /// Create a new instance of the <see cref="FirefoxOSNotification"/> class.
        /// </summary>
        /// 
        /// <param name="broker">Registration broker.</param>
        /// 
        /// <returns>Created notification object.</returns>
        public static FirefoxOSNotification FirefoxOSNotification(this PushBroker broker)
        {
            return new FirefoxOSNotification();
        }
    }
}