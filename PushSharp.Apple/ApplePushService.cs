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
		
		
		public ApplePushService(ApplePushChannelSettings channelSettings)
			: this(default(IPushChannelFactory), channelSettings, default(IPushServiceSettings))
		{
		}

		public ApplePushService(ApplePushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: this(default(IPushChannelFactory), channelSettings, serviceSettings)
		{
		}

		public ApplePushService(IPushChannelFactory pushChannelFactory, ApplePushChannelSettings channelSettings)
			: this(pushChannelFactory, channelSettings, default(IPushServiceSettings))
		{
		}

		public ApplePushService(IPushChannelFactory pushChannelFactory, ApplePushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: base(pushChannelFactory ?? new ApplePushChannelFactory(), channelSettings, serviceSettings)
		{
			var appleChannelSettings = channelSettings;
			cancelTokenSource = new CancellationTokenSource();

			//allow control over feedback call interval, if set to zero, don't make feedback calls automatically
			if (appleChannelSettings.FeedbackIntervalMinutes > 0)
			{
				feedbackService = new FeedbackService();
				feedbackService.OnFeedbackReceived += feedbackService_OnFeedbackReceived;

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

		public override bool BlockOnMessageResult
		{
			get { return false; }
		}
	}

	public class ApplePushChannelFactory : IPushChannelFactory
	{
		public IPushChannel CreateChannel(IPushChannelSettings channelSettings)
		{
			if (!(channelSettings is ApplePushChannelSettings))
				throw new ArgumentException("Channel Settings must be of type ApplePushChannelSettings");

			return new ApplePushChannel(channelSettings as ApplePushChannelSettings);
		}
	}
}
