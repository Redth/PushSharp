using System;
using System.Collections.Generic;
using System.Text;

namespace PushSharp.Google
{
    public class GcmXmppConfiguration
    {
        public GcmXmppConfiguration ()
        {
            Production = true;
            SenderIDs = new List<string> ();
        }

        public const string GCM_XMPP_SERVER = "gcm.googleapis.com";
        public const string GCM_XMPP_PREPROD_SERVER = "gcm-preprod.googleapis.com";
        public const int GCM_XMPP_PORT = 5235;
        public const int GCM_XMPP_PREPROD_PORT = 5236;

        public List<string> SenderIDs { get; set; }
        public string AuthenticationToken { get; set; }

        public string SaslAuthToken { 
            get {
                var s = new StringBuilder ();
               
                foreach (var sender in SenderIDs)
                    s.Append ("\0" + sender + "@gcm.googleapis.com");
                
                s.Append ("\0" + AuthenticationToken);

                return Convert.ToBase64String (Encoding.UTF8.GetBytes (s.ToString ()));
            }
        }

        public bool Production { get;set; }

        string overrideHost = string.Empty;
        int? overridePort = null;

        public string Host { 
            get { 
                if (!string.IsNullOrEmpty (overrideHost))
                    return overrideHost;

                return Production ? GCM_XMPP_SERVER : GCM_XMPP_PREPROD_SERVER;
            }
        }

        public int Port {
            get { 
                if (overridePort.HasValue)
                    return overridePort.Value;

                return Production ? GCM_XMPP_PORT : GCM_XMPP_PREPROD_PORT;
            }
        }
    }
}

