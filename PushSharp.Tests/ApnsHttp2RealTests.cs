using System;
using NUnit.Framework;
using PushSharp.Apple;
using Newtonsoft.Json.Linq;

namespace PushSharp.Tests
{
    [TestFixture]
    [Category("Disabled")]
    public class ApnsHttp2RealTests
    {
        [Test]
        public void APNSHTTP2_Send_Single()
        {
            var succeeded = 0;
            var failed = 0;
            var attempted = 0;

            var config = new ApnsHttp2Configuration(ApnsHttp2Configuration.ApnsServerEnvironment.Sandbox, Settings.Instance.ApnsCertificateFile, Settings.Instance.ApnsCertificatePassword);
            var broker = new ApnsHttp2ServiceBroker(config);
            broker.OnNotificationFailed += (notification, exception) => {
                failed++;
            };
            broker.OnNotificationSucceeded += (notification) => {
                succeeded++;
            };
            broker.Start();

            foreach (var dt in Settings.Instance.ApnsDeviceTokens)
            {
                attempted++;
                broker.QueueNotification(new ApnsHttp2Notification
                {
                    DeviceToken = dt,
                    Topic = "com.pushsharp.sample",
                    Payload = JObject.Parse("{ \"aps\" : { \"alert\" : \"Hello PushSharp!\", \"badge\" : 5, \"sound\" : \"blank.aiff\" } }")
                });
            }

            broker.Stop();

            Assert.AreEqual(attempted, succeeded);
            Assert.AreEqual(0, failed);
        }
    }
}

