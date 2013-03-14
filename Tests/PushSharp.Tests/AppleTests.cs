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

		private PushBroker broker;
		private byte[] appleCert;

		[SetUp]
		public void Setup()
		{
			Log.Level = LogLevel.Info;

			appleCert = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../Resources/PushSharp.Apns.Sandbox.p12"));
			broker = new PushBroker();
		}
		
		[Test]
		public void TestAllSuccessfulNotifications()
		{
			TestNotifications(10, 10, 0);
		}

		[Test]
		public void TestAllFailedNotifications()
		{
			var toFail = new int[10];

			for (int i = 0; i < toFail.Length; i++)
				toFail[i] = i;

			TestNotifications(10, 0, 10, toFail);
		}

		[Test]
		public void TestFailFirstNotifications()
		{
			TestNotifications(10, 9, 1, new int[] { 0 });
		}

		[Test]
		public void TestFailLastNotifications()
		{
			TestNotifications(10, 9, 1, new int[] { 9 });
		}

		[Test]
		public void TestFailMiddleNotifications()
		{
			TestNotifications(10, 8, 2, new int[] { 3, 6 });
		}

		[Test]
		public void TestSingleFailedNotifications()
		{
			TestNotifications(1, 0, 1, new int[] { 0 });
		}

		[Test]
		public void TestSingleSuccessfulNotifications()
		{
			TestNotifications(1, 1, 0);
		}

		public void TestNotifications(int toQueue, int expectSuccessful, int expectFailed, int[] indexesToFail = null)
		{
			testPort++;

			int pushFailCount = 0;
			int pushSuccessCount = 0;
			
			int serverReceivedCount = 0;
			int serverReceivedFailCount = 0;
			int serverReceivedSuccessCount = 0;

			var notification = new AppleNotification("aff441e214b2b2283df799f0b8b16c17a59b7ac077e2867ea54ebf6086e55866").WithAlert("Test");

			var len = notification.ToBytes().Length;

			var server = new TestServers.AppleTestServer();
			
			server.ResponseFilters.Add(new ApnsResponseFilter()
			{
				IsMatch = (identifier, token, payload) =>
				{
					Console.WriteLine("Server Received: id=" + identifier + ", token=" + token);

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


			var push = new ApplePushService(settings, new PushServiceSettings() { AutoScaleChannels = false, Channels = 1 });
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
