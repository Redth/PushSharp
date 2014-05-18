using System;

using PushSharp.Core;

namespace PushSharp.FirefoxOS
{
    /// <summary>
    /// FirefoxOS push channel.
    /// </summary>
    public class FirefoxOSPushService : PushServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FirefoxOSPushService"/> class.
        /// </summary>
        public FirefoxOSPushService()
            : this (default(IPushChannelFactory)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirefoxOSPushService"/> class.
        /// </summary>
        /// 
        /// <param name="pushChannelFactory">Channel factory.</param>
        public FirefoxOSPushService(IPushChannelFactory pushChannelFactory)
            : base(pushChannelFactory ?? new FirefoxOSPushChannelFactory(), default(IPushChannelSettings), default(IPushServiceSettings)) { }
    }

    /// <summary>
    /// FirefoxOS push channel factory.
    /// </summary>
    public class FirefoxOSPushChannelFactory : IPushChannelFactory
    {
        /// <summary>
        /// Creates a FirefoxOS push channel.
        /// </summary>
        /// <param name="channelSettings"></param>
        /// <returns></returns>
        public IPushChannel CreateChannel(IPushChannelSettings channelSettings)
        {
            return new FirefoxOSPushChannel();
        }
    }
}