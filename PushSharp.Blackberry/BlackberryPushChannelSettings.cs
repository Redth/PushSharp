namespace PushSharp.Blackberry
{
	public class BlackberryPushChannelSettings : Common.PushChannelSettings
	{
		public string BESAddress { get; set; }
		public int BESWebServerListenPort { get; set; }
		public int BESPushPort { get; set; }

		public string PushUsername { get; set; }
		public string PushPassword { get; set; }
	}
}
