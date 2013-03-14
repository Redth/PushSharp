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


		public void TestNotifications(int toQueue, int expectSuccessful, int expectFailed, int[] indexesToFail = null)
		{
			int serverReceivedCount = 0;
			int serverReceivedFailCount = 0;
			int serverReceivedSuccessCount = 0;

			var notification = new AppleNotification("aff441e214b2b2283df799f0b8b16c17a59b7ac077e2867ea54ebf6086e55866").WithAlert("Test");

			var len = notification.ToBytes().Length;

			var server = new TestServers.AppleTestServer(len, (success, identifier, token, payload) =>
			{
				serverReceivedCount++;

				if (success)
					serverReceivedSuccessCount++;
				else
					serverReceivedFailCount++;
			});

			server.ResponseFilters.Add(new ApnsResponseFilter()
			{
				IsMatch = (identifier, token, payload) =>
				{
					if (token.StartsWith("x"))
						return true;

					return false;
				},
				Status = ApnsResponseStatus.InvalidToken
			});

			Task.Factory.StartNew(server.Start).ContinueWith(t =>
			{
				var ex = t.Exception;

				Console.WriteLine(ex.ToString());
			}, TaskContinuationOptions.OnlyOnFaulted);

			var settings = new ApplePushChannelSettings(false, appleCert, "pushsharp", true);
			settings.OverrideServer("localhost", 2195);
			settings.SkipSsl = true;


			var push = new ApplePushService(settings, new PushServiceSettings() { AutoScaleChannels = false, Channels = 1 });

			for (int i = 0; i < toQueue; i++)
			{
				if (indexesToFail != null && indexesToFail.Contains(i))
					push.QueueNotification(new AppleNotification("xff441e214b2b2283df799f0b8b16c17a59b7ac077e2867ea54ebf6086e55866").WithAlert("Test"));
				else
					push.QueueNotification(new AppleNotification("aff441e214b2b2283df799f0b8b16c17a59b7ac077e2867ea54ebf6086e55866").WithAlert("Test"));
			}

			push.Stop();
			push.Dispose();

			server.Stop();

			Assert.AreEqual(toQueue, serverReceivedCount);
			Assert.AreEqual(expectFailed, serverReceivedFailCount);
			Assert.AreEqual(expectSuccessful, serverReceivedSuccessCount);
		}
	}
}
