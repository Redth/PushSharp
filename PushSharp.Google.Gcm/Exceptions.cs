using System;
using System.Collections.Generic;

namespace PushSharp.Google.Gcm
{
    public class GcmConnectionException : Exception
    {
        public GcmConnectionException (string msg ) : base (msg)
        {
        }
    }

    public class GcmMulticastResultException : Exception
    {
        public GcmMulticastResultException () : base ("One or more Registration Id's failed in the multicast notification")
        {
            Succeeded = new List<GcmNotification> ();
            Failed = new Dictionary<GcmNotification, Exception> ();
        }

        public List<GcmNotification> Succeeded { get;set; }

        public Dictionary<GcmNotification, Exception> Failed { get;set; }
    }
}

