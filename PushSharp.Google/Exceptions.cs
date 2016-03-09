using System;
using System.Collections.Generic;

namespace PushSharp.Google
{
    public class GcmConnectionException : Exception
    {
        public GcmConnectionException (string msg) : base (msg)
        {
        }

        public GcmConnectionException (string msg, string description) : base (msg)
        {
            Description = description;
        }

        public string Description { get; private set; }
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

