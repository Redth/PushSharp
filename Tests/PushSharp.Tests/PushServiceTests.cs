using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using PushSharp.Core;

namespace PushSharp.Tests
{
	[TestFixture]
	public class PushServiceTests
	{
		private Mock<INotification> MockUpNotification()
		{
			var mockNotification = new Mock<INotification>();
			mockNotification.Setup(n => n.EnqueuedTimestamp).Returns(DateTime.UtcNow.AddSeconds(-5));
			mockNotification.Setup(n => n.IsValidDeviceRegistrationId()).Returns(true);
			mockNotification.Setup(n => n.QueuedCount).Returns(1);
			mockNotification.Setup(n => n.Tag).Returns(null);

			return mockNotification;
		}

		private Mock<IPushChannel> MockUpChannel(Action<INotification, SendNotificationCallbackDelegate> callbackAction)
		{
			var mockChannel = new Mock<IPushChannel>();

			mockChannel.Setup(c => c.Dispose());
			mockChannel.Setup(c => c.SendNotification(It.IsAny<INotification>(), It.IsAny<SendNotificationCallbackDelegate>()))
				.Callback(callbackAction);

			return mockChannel;
		}

		private Mock<PushServiceBase> MockUpService(Action<INotification, SendNotificationCallbackDelegate> callback)
		{
			return MockUpService(new PushServiceSettings(), callback);
		}

		private Mock<PushServiceBase> MockUpService(PushServiceSettings serviceSettings,
		                                            Action<INotification, SendNotificationCallbackDelegate> callback)
		{
			var mockChanSettings = new Mock<IPushChannelSettings>();

			var mockChanFactory = new Mock<IPushChannelFactory>();
			mockChanFactory.Setup(cf => cf.CreateChannel(It.IsAny<IPushChannelSettings>()))
			               .Returns(() => MockUpChannel(callback).Object);

			var mockPushService = new Mock<PushServiceBase>(mockChanFactory.Object, mockChanSettings.Object, serviceSettings);
			mockPushService.Setup(ps => ps.BlockOnMessageResult).Returns(true);

			return mockPushService;
		}

		[SetUp]
		public void Setup()
		{
			Log.Level = LogLevel.Info;
		}

		[Test]
		public void SentFailedMatchesQueuedCount()
		{
			int count = 0;

			var svc = MockUpService((n, callback) =>
				{
					//Send some failed, some successful
					if ((count++) % 3 == 0) 
						callback(this, new SendNotificationResult(n, false, new Exception("Intentional Exception: " + Guid.NewGuid().ToString())));
					else
						callback(this, new SendNotificationResult(n));
				}).Object;
			
			
			int toSend = 10;
			int queued = 0;
			int success = 0;
			int failed = 0;
			
			svc.OnNotificationSent += (sender, notification) => Interlocked.Increment(ref success);
			svc.OnNotificationFailed += (sender, notification, error) => Interlocked.Increment(ref failed);
			
			for (var i = 0; i < toSend; i++)
			{
				svc.QueueNotification(MockUpNotification().Object);
				Interlocked.Increment(ref queued);
			}
			
			svc.Stop();
			svc.Dispose();

			Assert.IsTrue(queued > 0);
			Assert.AreEqual((success + failed), toSend);
		}


		[Test]
		public void RecoverFromLackOfChannelCallback()
		{
			int count = 0;

			var svc = MockUpService((n, callback) =>
			{
				//Sometimes, don't call back
				if ((count++) % 3 != 0)					
					callback(this, new SendNotificationResult(n));

			}).Object;

			svc.ServiceSettings.NotificationSendTimeout = 500;

			int toSend = 10;
			int queued = 0;
			int success = 0;
			int failed = 0;

			svc.OnNotificationSent += (sender, notification) => Interlocked.Increment(ref success);
			svc.OnNotificationFailed += (sender, notification, error) => Interlocked.Increment(ref failed);

			for (var i = 0; i < toSend; i++)
			{
				svc.QueueNotification(MockUpNotification().Object);
				Interlocked.Increment(ref queued);
			}

			svc.Stop();
			svc.Dispose();

			Assert.IsTrue(queued > 0);
			Assert.AreEqual((success + failed), toSend);
		}
	}
}
