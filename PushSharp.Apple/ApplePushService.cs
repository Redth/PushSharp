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
	public class ApplePushService : Core.PushServiceBase
	{
		FeedbackService feedbackService;
		CancellationTokenSource cancelTokenSource;
		Timer timerFeedback;
		
		public ApplePushService(Type pushChannelType, ApplePushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
			: base(pushChannelType, channelSettings, serviceSettings)
		{
			var appleChannelSettings = channelSettings as ApplePushChannelSettings;
			cancelTokenSource = new CancellationTokenSource();
			feedbackService = new FeedbackService();
			feedbackService.OnFeedbackReceived += new FeedbackService.FeedbackReceivedDelegate(feedbackService_OnFeedbackReceived);

			//allow control over feedback call interval, if set to zero, don't make feedback calls automatically
			if (appleChannelSettings.FeedbackIntervalMinutes > 0)
			{
				timerFeedback = new Timer(new TimerCallback((state) =>
				{
					try { feedbackService.Run(channelSettings as ApplePushChannelSettings, this.cancelTokenSource.Token); }
					catch (Exception ex) { this.Events.RaiseChannelException(this, ex); }

					//Timer will run first after 10 seconds, then every 10 minutes to get feedback!
				}), null, TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(appleChannelSettings.FeedbackIntervalMinutes));

			}
		}

		void feedbackService_OnFeedbackReceived(string deviceToken, DateTime timestamp)
		{
			this.Events.RaiseDeviceSubscriptionExpired(this, deviceToken);
		}
		
	}
}
