using PushSharp.Core;

namespace PushSharp.Blackberry
{
    public class BISPushChannelSettings : IPushChannelSettings
	{
		public string BbApplicationId { get; set; }
		public string PushPassword { get; set; }
        public string Boundary { get { return "ASDFaslkdfjasfaSfdasfhpoiurwqrwm"; } }

        private const string BbSendUrl = "https://pushapi.eval.blackberry.com/mss/PD_pushRequest";

        public BISPushChannelSettings()
        {
            BbUrl = BbSendUrl;
        }

        public BISPushChannelSettings(string applicationId,string password)
		{
			BbApplicationId = applicationId;
            PushPassword = password;
			BbUrl = BbSendUrl;
		}

		public string BbUrl { get; set; }
	}
}
