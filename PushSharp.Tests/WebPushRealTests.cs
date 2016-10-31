using System;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using PushSharp.Web;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PushSharp.Web.Exceptions;

namespace PushSharp.Tests
{
    [TestFixture]
    public class WebPushRealTests
    {
        [Test]
        public void Chrome_Notification_With_Payload_Test()
        {
            var settings = Settings.Instance;
            var ctx = new BrokerContext();
            ctx.Start();

            var subsription = settings.WebPush.ChromeSubscription;

            var notif = new WebPushNotification(subsription);
            notif.Payload = JObject.Parse("{ \"somekey\" : \"somevalue\" }");
            ctx.Broker.QueueNotification(notif);

            ctx.Broker.Stop();

            Assert.AreEqual(1, ctx.Succeeded);
            Assert.AreEqual(0, ctx.Failed);
        }

        [Test]
        public void Chrome_Notification_Should_Throw_Error_If_Payload_Exceed_Limit()
        {
            var settings = Settings.Instance;
            var ctx = new BrokerContext();
            ctx.Start();

            var subsription = settings.WebPush.ChromeSubscription;

            var notif = new WebPushNotification(subsription);
            var value = new string('a', 1024 * 5);
            notif.Payload = JObject.Parse($"{{ \"somekey\" : \"{value}\" }}");
            ctx.Broker.QueueNotification(notif);

            ctx.Broker.Stop();

            ctx.AssertPayloadExceedLimit();
        }

        [Test]
        public void Chrome_Should_Throw_Correct_Error_For_OutDated_Subscription()
        {
            var settings = Settings.Instance;
            var ctx = new BrokerContext();
            ctx.Start();

            var subsription = settings.WebPush.ChromeOutdatedSubscription;

            var notif = new WebPushNotification(subsription);
            notif.Payload = JObject.Parse("{ \"somekey\" : \"somevalue\" }");
            ctx.Broker.QueueNotification(notif);

            ctx.Broker.Stop();

            Assert.AreEqual(0, ctx.Succeeded);
            ctx.AssertSubscriptionExpired();
        }

        [Test]
        public void Firefox_Notification_With_Payload_Test()
        {
            var settings = Settings.Instance;
            var ctx = new BrokerContext();
            ctx.Start();

            var subsription = settings.WebPush.FirefoxSubscription;
           
            var notif = new WebPushNotification(subsription);
            notif.Payload = JObject.Parse("{ \"somekey\" : \"somevalue\" }");
            ctx.Broker.QueueNotification(notif);

            ctx.Broker.Stop();

            Assert.AreEqual(1, ctx.Succeeded);
            Assert.AreEqual(0, ctx.Failed);
        }

        [Test]
        public void Firefox_Notification_Should_Throw_Error_If_Payload_Exceed_Limit()
        {
            var settings = Settings.Instance;
            var ctx = new BrokerContext();
            ctx.Start();

            var subsription = settings.WebPush.FirefoxSubscription;

            var notif = new WebPushNotification(subsription);
            var value = new string('a', 1024 * 5);
            notif.Payload = JObject.Parse($"{{ \"somekey\" : \"{value}\" }}");
            ctx.Broker.QueueNotification(notif);

            ctx.Broker.Stop();

            ctx.AssertPayloadExceedLimit();
        }

        [Test]
        public void Firefox_Should_Throw_Correct_Error_For_OutDated_Subscription()
        {
            var settings = Settings.Instance;
            var ctx = new BrokerContext();
            ctx.Start();

            var subsription = settings.WebPush.FirefoxOutdatedSubscription;
           
            var notif = new WebPushNotification(subsription);
            notif.Payload = JObject.Parse("{ \"somekey\" : \"somevalue\" }");
            ctx.Broker.QueueNotification(notif);

            ctx.Broker.Stop();

            Assert.AreEqual(0, ctx.Succeeded);
            ctx.AssertSubscriptionExpired();
        }

        private class BrokerContext
        {
            public int Failed { get; private set; }
            public List<Exception> Exceptions { get; set; }
            public int Succeeded { get; private set; }
            public WebPushServiceBroker Broker { get; }

            public BrokerContext()
            {
                var settings = Settings.Instance;
                var configuration = new WebPushConfiguration
                {
                    GcmAPIKey = settings.GcmAuthToken
                };

                Exceptions = new List<Exception>();
                Broker = new WebPushServiceBroker(configuration);
            }

            public void Start()
            {
                Failed = 0;
                Succeeded = 0;
                Broker.OnNotificationFailed += (notification, exception) =>
                {
                    Failed++;
                    Exceptions.AddRange(exception.InnerExceptions);
                };
                Broker.OnNotificationSucceeded += (notification) => { Succeeded++; };

                Broker.Start();
            }

            public void AssertSubscriptionExpired()
            {
                Assert.AreEqual(1, Exceptions.Count, "only one exception should be registered");
                Assert.IsInstanceOf<WebPushNotificationException>(Exceptions[0]);

                var error = Exceptions[0] as WebPushNotificationException;
                Assert.IsTrue(error.IsExpiredSubscription, "subscription should be expired");
            }

            public void AssertPayloadExceedLimit()
            {
                Assert.AreEqual(1, Exceptions.Count, "only one exception should be registered");
                Assert.IsInstanceOf<WebPushNotificationException>(Exceptions[0]);

                var error = Exceptions[0] as WebPushNotificationException;
                Assert.IsTrue(error.IsPayloadExceedLimit, "Payload exceed limit should be true");
            }
        }
    }
}