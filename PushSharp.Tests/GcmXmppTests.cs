using System;
using NUnit.Framework;
using System.Threading.Tasks;
using PushSharp.Google;
using PushSharp.Core;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace PushSharp.Tests
{
    [TestFixture]
    [Category ("Disabled")]
    public class GcmXmppTests
    {
        [Test]
        public async Task GCMXMPP_Connect ()
        {
            var succeeded = 0;
            var failed = 0;
            var attempted = 0;

            var c = new GcmXmppConfiguration {
                Production = false,
                AuthenticationToken = Settings.Instance.GcmAuthToken,
                SenderIDs = new List<string> { Settings.Instance.GcmSenderId }
            };

            var gcm = new GcmXmppConnection (c);
            await gcm.Connect ();

            foreach (var regId in Settings.Instance.GcmRegistrationIds) {
                gcm.Send (new GcmXmppConnection.CompletableNotification (new GcmXmppNotification {
                    To = regId,
                    Data = JObject.Parse ("{ \"somekey\" : \"somevalue\" }")
                }));
            }

            gcm.Close ();

            Assert.AreEqual (attempted, succeeded);
            Assert.AreEqual (0, failed);
        }
    }
}

