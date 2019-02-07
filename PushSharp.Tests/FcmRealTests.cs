using System;
using NUnit.Framework;
using PushSharp.Google;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace PushSharp.Tests
{
    [TestFixture]
    [Category ("Real")]
    public class FcmRealTests
    {
        [Test]
        public void Fcm_Send_Single ()
        {
            var succeeded = 0;
            var failed = 0;
            var attempted = 0;

            var config = new FcmConfiguration (Settings.Instance.FcmSenderId, Settings.Instance.FcmAuthToken, null);
            var broker = new FcmServiceBroker (config);
            broker.OnNotificationFailed += (notification, exception) => {
                failed++;        
            };
            broker.OnNotificationSucceeded += (notification) => {
                succeeded++;
            };

            broker.Start ();

            foreach (var regId in Settings.Instance.FcmRegistrationIds) {
                attempted++;

                broker.QueueNotification (new FcmNotification {
                    RegistrationIds = new List<string> { 
                        regId
                    },
                    Data = JObject.Parse ("{ \"somekey\" : \"somevalue\" }")
                });
            }

            broker.Stop ();

            Assert.AreEqual (attempted, succeeded);
            Assert.AreEqual (0, failed);
        }
    }
}

