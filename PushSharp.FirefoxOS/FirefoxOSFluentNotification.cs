using System;

namespace PushSharp.FirefoxOS
{
    /// <summary>
    /// Fluent extension for configuring FirefoxOS notification.
    /// </summary>
    public static class FirefoxOSFluentNotification
    {
        /// <summary>
        /// Adds an endpoint url to the notification.
        /// </summary>
        /// 
        /// <param name="n">Notification object.</param>
        /// <param name="endpoint">Endpoint url.</param>
        public static FirefoxOSNotification ForEndpointUrl(this FirefoxOSNotification n, Uri endpoint)
        {
            n.EndPointUrl = endpoint;

            return n;
        }

        /// <summary>
        /// Adds a version to the notification.
        /// </summary>
        /// 
        /// <param name="n">Notification object.</param>
        /// <param name="version">Version to set.</param>
        public static FirefoxOSNotification WithVersion(this FirefoxOSNotification n, string version)
        {
            n.Version = version;

            return n;
        }
    }
}