using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Amazon.Adm;

namespace PushSharp
{
    public static class AdmFluentNotification
    {
        public static AdmNotification ForRegistrationId(this AdmNotification n, string registrationId)
        {
            n.RegistrationId = registrationId;
			return n;
		}
        
		public static AdmNotification WithConsolidationKey(this AdmNotification n, string consolidationKey)
		{
			n.ConsolidationKey = consolidationKey;
			return n;
		}

        public static AdmNotification WithExpiresAfter(this AdmNotification n, int expiresAfterSeconds)
        {
            n.ExpiresAfter = expiresAfterSeconds;
            return n;
        }

        public static AdmNotification WithData(this AdmNotification n, IDictionary<string, string> data)
        {
            if (n.Data == null)
                n.Data = new Dictionary<string, string>();

            foreach (var item in data)
            {
                if (!n.Data.ContainsKey(item.Key))
                    n.Data.Add(item.Key, item.Value);
                else
                    n.Data[item.Key] = item.Value;
            }

            return n;
        }
        
		public static AdmNotification WithData(this AdmNotification n, string key, string value)
		{
			if (n.Data == null)
                n.Data = new Dictionary<string, string>();

		    if (!n.Data.ContainsKey(key))
		        n.Data.Add(key, value);
		    else
		        n.Data[key] = value;

			return n;
		}
	}
}
