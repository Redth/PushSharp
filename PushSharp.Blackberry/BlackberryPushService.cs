namespace PushSharp.Blackberry
{
	public class BlackberryPushService : Common.PushServiceBase
	{
		public BlackberryPushService(BlackberryPushChannelSettings channelSettings, Common.PushServiceSettings serviceSettings)
			: base(channelSettings, serviceSettings)
		{
		}

		protected override Common.PushChannelBase CreateChannel(Common.PushChannelSettings channelSettings)
		{
			return new BlackberryPushChannel(channelSettings);
		}
	}
}
