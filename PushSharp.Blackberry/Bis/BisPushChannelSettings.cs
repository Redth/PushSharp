using PushSharp.Core;

namespace PushSharp.Blackberry
{
    public class BisPushChannelSettings : IPushChannelSettings
	{
		public string BbApplicationId { get; set; }
		public string PushPassword { get; set; }
        public string Boundary { get { return "ASDFaslkdfjasfaSfdasfhpoiurwqrwm"; } }

        private const string BbSendUrl = "https://pushapi.eval.blackberry.com/mss/PD_pushRequest";

        public BisPushChannelSettings()
        {
            BbUrl = BbSendUrl;
        }

        public BisPushChannelSettings(string applicationId,string password)
		{
			BbApplicationId = applicationId;
            PushPassword = password;
			BbUrl = BbSendUrl;
		}

		public string BbUrl { get; set; }
	}
}
