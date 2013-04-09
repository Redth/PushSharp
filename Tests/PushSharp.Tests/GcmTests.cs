using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using PushSharp.Android;
using PushSharp.Core;
using PushSharp.Tests.TestServers;

namespace PushSharp.Tests
{
	public class GcmTests
	{
		private int testPort = 2000;

		[Test]
		public void GCM_All_ShouldSucceed()
		{
			TestNotifications(false, 10, 10, 0);
		}

		[Test]
		public void GCM_All_ShouldFail()
		{
			var toFail = new int[10];

			for (int i = 0; i < toFail.Length; i++)
				toFail[i] = i;

			TestNotifications(false, 10, 0, 10, toFail);
		}

		[Test]
		public void GCM_First_ShouldFail()
		{
			TestNotifications(false, 10, 9, 1, new int[] { 0 });
		}

		[Test]
		public void GCM_Last_ShouldFail()
		{
			TestNotifications(false, 10, 9, 1, new int[] { 9 });
		}

		[Test]
		public void GCM_Middles_ShouldFail()
		{
			TestNotifications(false, 10, 8, 2, new int[] { 3, 6 });
		}

		[Test]
		public void GCM_Single_ShouldFail()
		{
			TestNotifications(false, 1, 0, 1, new int[] { 0 });
		}

		[Test]
		public void GCM_Single_ShouldSucceed()
		{
			TestNotifications(false, 1, 1, 0);
		}


		[Test]
		public void GCM_Batched_All_ShouldSucceed()
		{
			TestNotifications(false, 10, 10, 0);
		}

		[Test]
		public void GCM_Batched_All_ShouldFail()
		{
			var toFail = new int[10];

			for (int i = 0; i < toFail.Length; i++)
				toFail[i] = i;

			TestNotifications(false, 10, 0, 10, toFail);
		}

		[Test]
		public void GCM_Batched_First_ShouldFail()
		{
			TestNotifications(false, 10, 9, 1, new int[] { 0 });
		}

		[Test]
		public void GCM_Batched_Last_ShouldFail()
		{
			TestNotifications(false, 10, 9, 1, new int[] { 9 });
		}

		[Test]
		public void GCM_Batched_Middles_ShouldFail()
		{
			TestNotifications(false, 10, 8, 2, new int[] { 3, 6 });
		}

		[Test]
		public void GCM_Batched_Single_ShouldFail()
		{
			TestNotifications(false, 1, 0, 1, new int[] { 0 });
		}

		[Test]
		public void GCM_Batched_Single_ShouldSucceed()
		{
			TestNotifications(false, 1, 1, 0);
		}

		[Test]
		public void GCM_Subscription_ShouldBeExpired()
		{
			int msgIdOn = 1;

			int pushFailCount = 0;
			int pushSuccessCount = 0;
			int subChangedCount = 0;
			int subExpiredCount = 0;

			var notifications = new List<GcmNotification>() {
				new GcmNotification().ForDeviceRegistrationId("NOTREGISTERED").WithJson(@"{""key"":""value""}")
			};

			TestNotifications(notifications,
				new List<GcmMessageResponseFilter>() {
					new GcmMessageResponseFilter()
					{
						IsMatch = (request, s) => {
							return s.Equals("NOTREGISTERED", StringComparison.InvariantCultureIgnoreCase);
						},
						Status = new GcmMessageResult() { 
							ResponseStatus = GcmMessageTransportResponseStatus.NotRegistered,
							MessageId = "1:" + msgIdOn++
						}
					}
				},
				(sender, notification) => pushSuccessCount++, //Success
				(sender, notification, error) => pushFailCount++, //Failed
				(sender, oldId, newId, notification) => subChangedCount++,
				(sender, id, expiryDate, notification) => subExpiredCount++
			);

			Assert.AreEqual(0, pushFailCount, "Client - Failed Count");
			Assert.AreEqual(0, pushSuccessCount, "Client - Success Count");
			Assert.AreEqual(0, subChangedCount, "Client - SubscriptionId Changed Count");
			Assert.AreEqual(notifications.Count, subExpiredCount, "Client - SubscriptionId Expired Count");
		}


		[Test]
		public void GCM_Subscription_ShouldBeChanged()
		{
			int msgIdOn = 1;
			
			int pushFailCount = 0;
			int pushSuccessCount = 0;
			int subChangedCount = 0;
			int subExpiredCount = 0;
			
			var notifications = new List<GcmNotification>() {
				new GcmNotification().ForDeviceRegistrationId("NOTREGISTERED").WithJson(@"{""key"":""value""}")
			};
			
			TestNotifications(notifications,
			                  new List<GcmMessageResponseFilter>() {
				new GcmMessageResponseFilter()
				{
					IsMatch = (request, s) => {
						return s.Equals("NOTREGISTERED", StringComparison.InvariantCultureIgnoreCase);
					},
					Status = new GcmMessageResult() { 
						ResponseStatus = GcmMessageTransportResponseStatus.NotRegistered,
						CanonicalRegistrationId = "NEWID",
						MessageId = "1:" + msgIdOn++
					}
				}
			},
			(sender, notification) => pushSuccessCount++, //Success
			(sender, notification, error) => pushFailCount++, //Failed
			(sender, oldId, newId, notification) => subChangedCount++,
			(sender, id, expiryDate, notification) => subExpiredCount++
			);
			
			Assert.AreEqual(0, pushFailCount, "Client - Failed Count");
			Assert.AreEqual(0, pushSuccessCount, "Client - Success Count");
			Assert.AreEqual(notifications.Count, subChangedCount, "Client - SubscriptionId Changed Count");
			Assert.AreEqual(0, subExpiredCount, "Client - SubscriptionId Expired Count");
		}


		public void TestNotifications(bool shouldBatch, int toQueue, int expectSuccessful, int expectFailed, int[] indexesToFail = null)
		{
			testPort++;

			int msgIdOn = 1000;

			int pushFailCount = 0;
			int pushSuccessCount = 0;

			int serverReceivedCount = 0;
			int serverReceivedFailCount = 0;
			int serverReceivedSuccessCount = 0;

			//var notification = new GcmNotification();

			var server = new TestServers.GcmTestServer();

			server.MessageResponseFilters.Add(new GcmMessageResponseFilter()
			{
				IsMatch = (request, s) => {
					return s.Equals("FAIL", StringComparison.InvariantCultureIgnoreCase);
				},
				Status = new GcmMessageResult() { 
					ResponseStatus = GcmMessageTransportResponseStatus.InvalidRegistration,
					MessageId = "1:" + msgIdOn++
				}
			});

			server.MessageResponseFilters.Add(new GcmMessageResponseFilter()
			                                  {
				IsMatch = (request, s) => {
					return s.Equals("NOTREGISTERED", StringComparison.InvariantCultureIgnoreCase);
				},
				Status = new GcmMessageResult() { 
					ResponseStatus = GcmMessageTransportResponseStatus.NotRegistered,
					MessageId = "1:" + msgIdOn++
				}
			});
			//var waitServerFinished = new ManualResetEvent(false);
			
			server.Start(testPort, response =>
						{
							serverReceivedCount += (int)response.NumberOfCanonicalIds;
							serverReceivedSuccessCount += (int) response.NumberOfSuccesses;
							serverReceivedFailCount += (int) response.NumberOfFailures;
						});

			

			var settings = new GcmPushChannelSettings("SENDERAUTHTOKEN");
			settings.OverrideUrl("http://localhost:" + (testPort) + "/");

			var push = new GcmPushService(settings);
			push.OnNotificationSent += (sender, notification1) => pushSuccessCount++;
			push.OnNotificationFailed += (sender, notification1, error) => {
				                                                               pushFailCount++;
			};

			var json = @"{""key"":""value1""}";

			if (shouldBatch)
			{
				var regIds = new List<string>();

				for (int i = 0; i < toQueue; i++)
					regIds.Add((indexesToFail != null && indexesToFail.Contains(i)) ? "FAIL" : "SUCCESS");

				var n = new GcmNotification();
				n.RegistrationIds.AddRange(regIds);
				n.WithJson(json);
				
				push.QueueNotification(n);
			}
			else
			{
				for (int i = 0; i < toQueue; i++)
					push.QueueNotification(new GcmNotification()
						.ForDeviceRegistrationId((indexesToFail != null && indexesToFail.Contains(i)) ? "FAIL" : "SUCCESS")
						.WithJson(json));
			}

			push.Stop();
			push.Dispose();

			server.Dispose();
			//waitServerFinished.WaitOne();

			Console.WriteLine("TEST-> DISPOSE.");

			Assert.AreEqual(toQueue, serverReceivedCount, "Server - Received Count");
			Assert.AreEqual(expectFailed, serverReceivedFailCount, "Server - Failed Count");
			Assert.AreEqual(expectSuccessful, serverReceivedSuccessCount, "Server - Success Count");

			Assert.AreEqual(expectFailed, pushFailCount, "Client - Failed Count");
			Assert.AreEqual(expectSuccessful, pushSuccessCount, "Client - Success Count");
		}




		public void TestNotifications(List<GcmNotification> notifications,
		                              List<GcmMessageResponseFilter> responseFilters,
		                              Action<object, INotification> sentCallback,
		                              Action<object, INotification, Exception> failedCallback,
		                              Action<object, string, string, INotification> subscriptionChangedCallback,
		                              Action<object, string, DateTime, INotification> subscriptionExpiredCallback)
		{
			testPort++;

			int pushFailCount = 0;
			int pushSuccessCount = 0;
			
			int serverReceivedCount = 0;
			int serverReceivedFailCount = 0;
			int serverReceivedSuccessCount = 0;

		
			var server = new TestServers.GcmTestServer();

			server.MessageResponseFilters.AddRange(responseFilters);

			server.Start(testPort, response => {
				serverReceivedCount += (int)response.NumberOfCanonicalIds;
				serverReceivedSuccessCount += (int) response.NumberOfSuccesses;
				serverReceivedFailCount += (int) response.NumberOfFailures;
			});
			
			
			
			var settings = new GcmPushChannelSettings("SENDERAUTHTOKEN");
			settings.OverrideUrl("http://localhost:" + (testPort) + "/");
			
			var push = new GcmPushService(settings);
			push.OnNotificationSent += (sender, notification1) => {
				pushSuccessCount++;
				sentCallback(sender, notification1);
			};
			push.OnNotificationFailed += (sender, notification1, error) => {
				pushFailCount++;
				failedCallback(sender, notification1, error);
			};
			push.OnDeviceSubscriptionChanged += (sender, oldSubscriptionId, newSubscriptionId, notification) => subscriptionChangedCallback(sender, oldSubscriptionId, newSubscriptionId, notification);
			push.OnDeviceSubscriptionExpired += (sender, expiredSubscriptionId, expirationDateUtc, notification) => subscriptionExpiredCallback(sender, expiredSubscriptionId, expirationDateUtc, notification);


			foreach (var n in notifications)
				push.QueueNotification(n);

			push.Stop();
			push.Dispose();
			
			server.Dispose();
			//waitServerFinished.WaitOne();
			
			Console.WriteLine("TEST-> DISPOSE.");
		}
	}
}
