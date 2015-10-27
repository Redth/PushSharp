using System;
using NUnit.Framework;
using System.Threading.Tasks;
using PushSharp.Google.Gcm;
using PushSharp.Core;
using System.Collections.Generic;
using PushSharp.Google.Gcm.Xmpp;
using Newtonsoft.Json.Linq;

namespace PushSharp.Tests
{
    [TestFixture]
    public class GcmXmppTests
    {
        const string SENDER_ID = "785671162406";
        const string AUTH_TOKEN = "AIzaSyCQ6mYcuT6XHbAPaPsm7RfycPhWBvuX_XA";

        [Test]
        public async Task GCMXMPP_Connect ()
        {
            var c = new GcmXmppConfiguration {
                Production = false,
                AuthenticationToken = AUTH_TOKEN,
                SenderIDs = new List<string> { SENDER_ID }
            };

            var gcm = new GcmXmppConnection (c);
            await gcm.Connect ();

            gcm.Send (new GcmXmppConnection.CompletableNotification (new GcmXmppNotification {
                To = "dCsRXe5aSKU:APA91bE2BbDi8f30j4IuMKCy8m8xBMZesT4UD7oBmOyaJWwr6LtR0PtXx2Kwlvo3IPuOQ4XsRxUiIuvbSThsinooNvbvqVCm1aw2uqyxg8mEyGQGzzU_SH8CjcT9aZ93qjJwPSmt0-Tk",
                Data = JObject.Parse ("{ \"somekey\" : \"somevalue\" }")
            }));

            gcm.Close ();

            //await Task.Delay (5000).ConfigureAwait (false);
            Log.Debug ("Done");
        }
    }
}

