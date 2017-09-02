﻿using System;
using PushSharp.Common;

namespace PushSharp.Firefox
{
    public class FirefoxNotification : INotification
    {        
        /// <summary>
        /// Gets or sets Unique URL to be used by the AppServer to initiate a response from the App.
        /// </summary>
        /// 
        /// <remarks>
        /// This is generated by SimplePush and sent to the App.
        /// The App will need to relay this to the AppServer.
        /// </remarks>
        public Uri EndPointUrl { get; set; }

        /// <summary>
        /// Gets or sets notification version.
        /// </summary>
        /// 
        /// <remarks>
        /// SimplePush does not carry information, only versions.
        /// A version is a number that keeps increasing.
        /// The AppServer tells the Endpoint about a new version whenever it wants an App to be notified.
        /// </remarks>
        public string Version { get; set; }

        public override string ToString()
        {
            return string.Format("version={0}", Version);
        }

        #region INotification implementation
        public bool IsDeviceRegistrationIdValid ()
        {
            return true;
        }

        public object Tag { get; set; }
        #endregion
    }
}

