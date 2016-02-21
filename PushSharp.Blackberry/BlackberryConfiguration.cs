using System;

namespace PushSharp.Blackberry
{
    public class BlackberryConfiguration
    {   
        const string SEND_URL = "https://pushapi.eval.blackberry.com/mss/PD_pushRequest";

        public BlackberryConfiguration ()
        {
            SendUrl = SEND_URL;
        }

        public BlackberryConfiguration (string applicationId, string password)
        {
            ApplicationId = applicationId;
            Password = password;
            SendUrl = SEND_URL;
        }

        public string ApplicationId { get; set; }
        public string Password { get; set; }
        public string Boundary { get { return "ASDFaslkdfjasfaSfdasfhpoiurwqrwm"; } }


        /// <summary>
        /// Push Proxy Gateway (PPG) Url is used for submitting push requests
        /// Default value is BIS PPG evaluation url 
        /// https://pushapi.eval.blackberry.com/mss/PD_pushRequest
        /// </summary>
        public string SendUrl { get; private set; }

        /// <summary>
        /// Overrides SendUrl with any PPG url: BIS or BES
        /// </summary>
        /// <param name="url">Push Proxy Gateway (PPG) Url,
        /// For BIS,it's PPG production url is in format: http://cpxxx.pushapi.na.blackberry.com
        /// where xxx should be replaced with CPID (Content Provider ID)</param>
        public void OverrideSendUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                if (url.EndsWith("pushapi.na.blackberry.com", StringComparison.InvariantCultureIgnoreCase) ||
                    url.EndsWith("pushapi.eval.blackberry.com", StringComparison.InvariantCultureIgnoreCase))
                    url = url + @"/mss/PD_pushRequest";
            }
            SendUrl = url;
        }
    
    }
}

