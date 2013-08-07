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
			TestNotifications(100000, 100000, 0, null, false);
		}

		[Test]
		public void APNS_Some_ShouldFail_VeryMany()
		{
			var amt = 100000;
			var fail = new int[] { 5000, 15000, 25000, 35000, 45000, 55000, 65000, 75000, 85000, 95000 };

			TestNotifications (amt, amt - fail.Length, fail.Length, fail, false);
		}

		[Test]
		public void APNS_All_ShouldSucceed_VeryMany_AutoScale()
		{
			TestNotifications(10000, 10000, 0, null, false, true);
		}

		[Test]
		public void APNS_Some_ShouldFail_VeryMany_AutoScale()
		{
			var amt = 10000;
			var fail = new int[] { 500, 1500, 2500, 3500, 4500, 5500, 6500, 7500, 8500, 9500 };

			TestNotifications (amt, amt - fail.Length, fail.Length, fail, false, true);
		}


		[Test]
		public void APNS_All_ShouldSucceed()
		{
			TestNotifications(10, 10, 0);
		}

		[Test]
		public void APNS_All_ShouldFail()
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

		public void TestNotifications(int toQueue, int expectSuccessful, int expectFailed, int[] idsToFail = null, bool waitForScaling = false, bool autoScale = false)
		{
			var testServer = new ApnsNodeTestServer ("http://localhost:8888/");
			testServer.Reset ();
			testServer.Setup (idsToFail ?? new int[] {});

			var started = DateTime.UtcNow;

			int pushFailCount = 0;
			int pushSuccessCount = 0;

			AppleNotification.ResetIdentifier ();

			var settings = new ApplePushChannelSettings(false, appleCert, "pushsharp", true);
			settings.OverrideServer("localhost", 2195);
			settings.SkipSsl = true;
			settings.MillisecondsToWaitBeforeMessageDeclaredSuccess = 5000;

			var serviceSettings = new PushServiceSettings();

			if (!autoScale)
			{
				serviceSettings.AutoScaleChannels = false;
				serviceSettings.Channels = 1;
			}

			var push = new ApplePushService(settings, serviceSettings);
			push.OnNotificationFailed += (sender, notification1, error) => {
				Console.WriteLine("NOTIFICATION FAILED: " + ((AppleNotification)notification1).Identifier);
				pushFailCount++;
			};
			push.OnNotificationSent += (sender, notification1) => {
				pushSuccessCount++;
			};
			push.OnNotificationRequeue += (sender, e) => {
				Console.WriteLine("REQUEUE: " + ((AppleNotification)e.Notification).Identifier);
			};

			for (int i = 0; i < toQueue; i++)
			{
				var n = new AppleNotification("aff441e214b2b2283df799f0b8b16c17a59b7ac077e2867ea54ebf6086e55866").WithAlert("Test");

				push.QueueNotification(n);
			}

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

			Console.WriteLine("Avg Queue Wait Time: " + push.AverageQueueWaitTime + " ms");
			Console.WriteLine("Avg Send Time: " + push.AverageSendTime + " ms");

			var span = DateTime.UtcNow - started;

			var info = testServer.GetInfo ();

			Console.WriteLine ("Test Time: " + span.TotalMilliseconds + " ms");
			Console.WriteLine ("Client Failed: {0} Succeeded: {1} Sent: {2}", pushFailCount, pushSuccessCount, toQueue);
			Console.WriteLine ("Server Failed: {0} Succeeded: {1} Received: {2} Discarded: {3}", info.FailedIds.Length, info.SuccessIds.Length, info.Received, info.Discarded);

			//Assert.AreEqual(toQueue, info.Received, "Server - Received Count");
			Assert.AreEqual(expectFailed, info.FailedIds.Length, "Server - Failed Count");
			Assert.AreEqual(expectSuccessful, info.SuccessIds.Length, "Server - Success Count");

			Assert.AreEqual(expectFailed, pushFailCount, "Client - Failed Count");
			Assert.AreEqual(expectSuccessful, pushSuccessCount, "Client - Success Count");
		}

//		public void TestNotifications(int toQueue, int expectSuccessful, int expectFailed, int[] idsToFail = null, bool waitForScaling = false)
//		{
//			var started = DateTime.UtcNow;
//
//			testPort++;
//					
//			int pushFailCount = 0;
//			int pushSuccessCount = 0;
//			
//			int serverReceivedCount = 0;
//			int serverReceivedFailCount = 0;
//			int serverReceivedSuccessCount = 0;
//			int lastIdentifier = -1;
//
//			AppleNotification.ResetIdentifier ();
//
//			/*var notification = new AppleNotification("aff441e214b2b2283df799f0b8b16c17a59b7ac077e2867ea54ebf6086e55866").WithAlert("Test");
//
//			var len = notification.ToBytes().Length;
//
//			var server = new TestServers.ApnsTestServer();
//			
//			server.ResponseFilters.Add(new ApnsResponseFilter()
//			{
//				IsMatch = (identifier, token, payload) =>
//				{
//					var id = identifier;
//
//					if (token.StartsWith("b", StringComparison.InvariantCultureIgnoreCase))
//					{
//						Console.WriteLine("Failing: " + identifier);
//						return true;
//					}
//
//					return false;
//				},
//				Status = ApnsResponseStatus.InvalidToken
//			});
//			*/
//
//			/*var waitServerFinished = new ManualResetEvent(false);
//
//
//			Task.Factory.StartNew(() =>
//				{
//					try
//					{
//						server.Start(testPort, len, (success, identifier, token, payload) =>
//						{
//                            //Console.WriteLine("Server Received: id=" + identifier + ", payload= " + payload + ", token=" + token + ", success=" + success);
//							
//							if (identifier - lastIdentifier > 1)
//
//							serverReceivedCount++;
//
//							if (success)
//								serverReceivedSuccessCount++;
//							else
//								serverReceivedFailCount++;
//						});
//
//					}
//					catch (Exception ex)
//					{
//						Console.WriteLine(ex);										
//					}
//
//					waitServerFinished.Set();
//
//				}).ContinueWith(t =>
//					{
//						var ex = t.Exception;
//						Console.WriteLine(ex);
//					}, TaskContinuationOptions.OnlyOnFaulted);
//			*/
//
//			var settings = new ApplePushChannelSettings(false, appleCert, "pushsharp", true);
//			//settings.OverrideServer("localhost", testPort);
//			settings.OverrideServer("localhost", 2195);
//			settings.SkipSsl = true;
//			//settings.MillisecondsToWaitBeforeMessageDeclaredSuccess = 60000 * 5;
//
//
//			var push = new ApplePushService(settings, new PushServiceSettings() { AutoScaleChannels = false, Channels = 1 });
//			push.OnNotificationFailed += (sender, notification1, error) => {
//				Console.WriteLine("APPLEPUSHSERVICE NOTIFICATION FAILED: " + ((AppleNotification)notification1).Identifier);
//				pushFailCount++;
//			};
//			push.OnNotificationSent += (sender, notification1) => {
//				pushSuccessCount++;
//			};
//			push.OnNotificationRequeue += (sender, e) => {
//				Console.WriteLine("REQUEUE: " + ((AppleNotification)e.Notification).Identifier);
//			};
//
//			for (int i = 0; i < toQueue; i++)
//			{
//				//INotification n;
//
//				//if (idsToFail != null && idsToFail.Contains(i))
//				//	n = new AppleNotification("bff441e214b2b2283df799f0b8b16c17a59b7ac077e2867ea54ebf6086e55866").WithAlert("Test");
//				//else
//				var n = new AppleNotification("aff441e214b2b2283df799f0b8b16c17a59b7ac077e2867ea54ebf6086e55866").WithAlert("Test");
//				
//				push.QueueNotification(n);
//			}
//
//            Console.WriteLine("Avg Queue Wait Time: " + push.AverageQueueWaitTime + " ms");
//            Console.WriteLine("Avg Send Time: " + push.AverageSendTime + " ms");
//
//            if (waitForScaling)
//            {
//                while (push.QueueLength > 0)
//                    Thread.Sleep(500);
//
//                Console.WriteLine("Sleeping 3 minutes for autoscaling...");
//                Thread.Sleep(TimeSpan.FromMinutes(3));
//
//                Console.WriteLine("Channel Count: " + push.ChannelCount);
//                Assert.IsTrue(push.ChannelCount <= 1);
//            }
//
//			push.Stop();
//			push.Dispose();
//
//			//server.Dispose();
//			//waitServerFinished.WaitOne();
//
//			Console.WriteLine("TEST-> DISPOSE.");
//
//			var span = DateTime.UtcNow - started;
//
//			Console.WriteLine ("Test Time: " + span.TotalMilliseconds + " ms");
//			Console.WriteLine ("Client Failed: {0} Succeeded: {1} Sent: {2}", pushFailCount, pushSuccessCount, toQueue);
//			Console.WriteLine ("Server Failed: {0} Succeeded: {1} Received: {2}", serverReceivedFailCount, serverReceivedSuccessCount, serverReceivedCount);
//			
//			//Assert.AreEqual(toQueue, serverReceivedCount, "Server - Received Count");
//			//Assert.AreEqual(expectFailed, serverReceivedFailCount, "Server - Failed Count");
//			//Assert.AreEqual(expectSuccessful, serverReceivedSuccessCount, "Server - Success Count");
//
//			Assert.AreEqual(expectFailed, pushFailCount, "Client - Failed Count");
//			Assert.AreEqual(expectSuccessful, pushSuccessCount, "Client - Success Count");
//		}
	}
}
