using System;
using NUnit.Framework;
using PushSharp.Google.Gcm;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace PushSharp.Tests
{
    [TestFixture]
    [Category ("Real")]
    public class GcmRealTests
    {
        [Test]
        public void Gcm_Send_Single ()
        {
            var succeeded = 0;
            var failed = 0;
            var attempted = 0;

            var config = new GcmConfiguration (Settings.Instance.GcmSenderId, Settings.Instance.GcmAuthToken, null);
            var broker = new GcmServiceBroker (config);
            broker.OnNotificationFailed += (notification, exception) => {
                failed++;        
            };
            broker.OnNotificationSucceeded += (notification) => {
                succeeded++;
            };

            broker.Start ();

            foreach (var regId in Settings.Instance.GcmRegistrationIds) {
                attempted++;

                broker.QueueNotification (new GcmNotification {
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

