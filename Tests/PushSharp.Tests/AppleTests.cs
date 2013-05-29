using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PushSharp.Apple;
using PushSharp.Core;
using PushSharp.Tests.TestServers;

namespace PushSharp.Tests
{
	[TestFixture]
	public class AppleTests
	{
		private int testPort = 2197;
	
		private byte[] appleCert;

		[SetUp]
		public void Setup()
		{
			Log.Level = LogLevel.Info;

			appleCert = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../Resources/PushSharp.Apns.Sandbox.p12"));
		}

		[Test]
		public void APNS_All_ShouldSucceed_VeryMany()
		{
			TestNotifications(100000, 100000, 0, null, true);
		}

		[Test]
		public void APNS_All_ShouldSucceed()
		{
			TestNotifications(10, 10, 0);
		}

		[Test]
		public void APNS_All_ShouldFaile()
		{
			var toFail = new int[10];

			for (int i = 0; i < toFail.Length; i++)
				toFail[i] = i;

			TestNotifications(10, 0, 10, toFail);
		}

		[Test]
		public void APNS_First_ShouldFail()
		{
			TestNotifications(10, 9, 1, new int[] { 0 });
		}

		[Test]
		public void APNS_Last_ShouldFail()
		{
			TestNotifications(10, 9, 1, new int[] { 9 });
		}

		[Test]
		public void APNS_Middles_ShouldFail()
		{
			TestNotifications(10, 8, 2, new int[] { 3, 6 });
		}

		[Test]
		public void APNS_Single_ShouldFail()
		{
			TestNotifications(1, 0, 1, new int[] { 0 });
		}

		[Test]
		public void APNS_Single_ShouldSucceed()
		{
			TestNotifications(1, 1, 0);
		}

		public void TestNotifications(int toQueue, int expectSuccessful, int expectFailed, int[] indexesToFail = null, bool waitForScaling = false)
		{
			testPort++;

			int pushFailCount = 0;
			int pushSuccessCount = 0;
			
			int serverReceivedCount = 0;
			int serverReceivedFailCount = 0;
			int serverReceivedSuccessCount = 0;

			var notification = new AppleNotification("aff441e214b2b2283df799f0b8b16c17a59b7ac077e2867ea54ebf6086e55866").WithAlert("Test");

			var len = notification.ToBytes().Length;

			var server = new TestServers.ApnsTestServer();
			
			server.ResponseFilters.Add(new ApnsResponseFilter()
			{
				IsMatch = (identifier, token, payload) =>
				{
					var id = identifier;

					

					if (token.StartsWith("b", StringComparison.InvariantCultureIgnoreCase))
						return true;

					return false;
				},
				Status = ApnsResponseStatus.InvalidToken
			});

			var waitServerFinished = new ManualResetEvent(false);


			Task.Factory.StartNew(() =>
				{
					try
					{
						server.Start(testPort, len, (success, identifier, token, payload) =>
						{
                            //Console.WriteLine("Server Received: id=" + identifier + ", payload= " + payload + ", token=" + token + ", success=" + success);

							serverReceivedCount++;

							if (success)
								serverReceivedSuccessCount++;
							else
								serverReceivedFailCount++;
						});

					}
					catch (Exception ex)
					{
						Console.WriteLine(ex);										
					}

					waitServerFinished.Set();

				}).ContinueWith(t =>
					{
						var ex = t.Exception;
						Console.WriteLine(ex);
					}, TaskContinuationOptions.OnlyOnFaulted);

			var settings = new ApplePushChannelSettings(false, appleCert, "pushsharp", true);
			settings.OverrideServer("localhost", testPort);
			settings.SkipSsl = true;


			var push = new ApplePushService(settings, new PushServiceSettings() { AutoScaleChannels = true, Channels = 1 });
			push.OnNotificationFailed += (sender, notification1, error) => pushFailCount++;
			push.OnNotificationSent += (sender, notification1) => pushSuccessCount++;

			for (int i = 0; i < toQueue; i++)
			{
				INotification n;

				if (indexesToFail != null && indexesToFail.Contains(i))
					n = new AppleNotification("bff441e214b2b2283df799f0b8b16c17a59b7ac077e2867ea54ebf6086e55866").WithAlert("Test");
				else
					n = new AppleNotification("aff441e214b2b2283df799f0b8b16c17a59b7ac077e2867ea54ebf6086e55866").WithAlert("Test");
				
				push.QueueNotification(n);
			}

            Console.WriteLine("Avg Queue Wait Time: " + push.AverageQueueWaitTime + " ms");
            Console.WriteLine("Avg Send Time: " + push.AverageSendTime + " ms");

            if (waitForScaling)
            {
                while (push.QueueLength > 0)
                    Thread.Sleep(500);

                Console.WriteLine("Sleeping 3 minutes for autoscaling...");
                Thread.Sleep(TimeSpan.FromMinutes(3));

                Console.WriteLine("Channel Count: " + push.ChannelCount);
                Assert.IsTrue(push.ChannelCount <= 1);
            }

			push.Stop();
			push.Dispose();

			server.Dispose();
			waitServerFinished.WaitOne();

			Console.WriteLine("TEST-> DISPOSE.");
			
			Assert.AreEqual(toQueue, serverReceivedCount, "Server - Received Count");
			Assert.AreEqual(expectFailed, serverReceivedFailCount, "Server - Failed Count");
			Assert.AreEqual(expectSuccessful, serverReceivedSuccessCount, "Server - Success Count");

			Assert.AreEqual(expectFailed, pushFailCount, "Client - Failed Count");
			Assert.AreEqual(expectSuccessful, pushSuccessCount, "Client - Success Count");
		}
	}
}
