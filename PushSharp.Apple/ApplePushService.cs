using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PushSharp.Core;

namespace PushSharp.Apple
{
	public class ApplePushService : PushServiceBase
	{
		FeedbackService feedbackService;
		CancellationTokenSource cancelTokenSource;
		Timer timerFeedback;
		
		
		public ApplePushService(IApplePushChannelSettings channelSettings)
			: this(default(IPushChannelFactory), channelSettings, default(IPushServiceSettings))
		{
		}

		public ApplePushService(IApplePushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: this(default(IPushChannelFactory), channelSettings, serviceSettings)
		{
		}

		public ApplePushService(IPushChannelFactory pushChannelFactory, IApplePushChannelSettings channelSettings)
			: this(pushChannelFactory, channelSettings, default(IPushServiceSettings))
		{
		}
		
		public ApplePushService(IPushChannelFactory pushChannelFactory, IApplePushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: base(pushChannelFactory ?? new ApplePushChannelFactory(), channelSettings, serviceSettings)
		{
			var appleChannelSettings = channelSettings;
			cancelTokenSource = new CancellationTokenSource();
			feedbackService = new FeedbackService();
			feedbackService.OnFeedbackReceived += feedbackService_OnFeedbackReceived;

			//allow control over feedback call interval, if set to zero, don't make feedback calls automatically
			if (appleChannelSettings.FeedbackIntervalMinutes > 0)
			{
				timerFeedback = new Timer(new TimerCallback((state) =>
				{
					try { feedbackService.Run(channelSettings as ApplePushChannelSettings, this.cancelTokenSource.Token); }
					catch (Exception ex) { base.RaiseServiceException(ex); }

					//Timer will run first after 10 seconds, then every 10 minutes to get feedback!
				}), null, TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(appleChannelSettings.FeedbackIntervalMinutes));

			}
		}

		void feedbackService_OnFeedbackReceived(string deviceToken, DateTime timestamp)
		{
			base.RaiseSubscriptionExpired(deviceToken, timestamp.ToUniversalTime(), null);
		}
		
	}

	public class ApplePushChannelFactory : IPushChannelFactory
	{
		public IPushChannel CreateChannel(IPushChannelSettings channelSettings)
		{
			return new ApplePushChannel(channelSettings);
		}
	}
}
