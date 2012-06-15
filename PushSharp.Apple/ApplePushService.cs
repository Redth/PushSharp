using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PushSharp.Common;

namespace PushSharp.Apple
{
	public class ApplePushService : Common.PushServiceBase, IDisposable
	{
		FeedbackService feedbackService;
		CancellationTokenSource cancelTokenSource;
		Timer timerFeedback;

		public ApplePushService(PushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
			: base(channelSettings, serviceSettings)
		{
			cancelTokenSource = new CancellationTokenSource();
			feedbackService = new FeedbackService();
			feedbackService.OnFeedbackReceived += new FeedbackService.FeedbackReceivedDelegate(feedbackService_OnFeedbackReceived);
			timerFeedback = new Timer(new TimerCallback((state) =>
			{
				feedbackService.Run(channelSettings as ApplePushChannelSettings, this.cancelTokenSource.Token);

				//Timer will run first after 10 seconds, then every 10 minutes to get feedback!
			}),null, TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(10));

		}

		void feedbackService_OnFeedbackReceived(string deviceToken, DateTime timestamp)
		{
			this.Events.RaiseDeviceSubscriptionExpired(PlatformType.Apple, deviceToken);
		}

		protected override Common.PushChannelBase CreateChannel(Common.PushChannelSettings channelSettings)
		{
			return new ApplePushChannel(channelSettings as ApplePushChannelSettings);
		}

		public new void Dispose()
		{
			if (!cancelTokenSource.IsCancellationRequested)
				cancelTokenSource.Cancel();

			base.Dispose();
		}

		
	}
}
