using System;
using NUnit.Framework;
using PushSharp.Google.Gcm;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace PushSharp.Tests
{
    [TestFixture]
    public class GcmTests
    {
        [Test]
        public void Gcm_Send_Single ()
        {
            var config = new GcmConfiguration ("785671162406", "AIzaSyCQ6mYcuT6XHbAPaPsm7RfycPhWBvuX_XA", null);

            var broker = new GcmServiceBroker (config);
            broker.OnNotificationFailed += (notification, exception) => {
                Console.WriteLine ("FAILED: " + exception);
            };
            broker.OnNotificationSucceeded += (notification) => {
                Console.WriteLine ("SUCCESS");
            };

            broker.Start ();

            broker.QueueNotification (new GcmNotification {
                RegistrationIds = new List<string> { 
                    "dCsRXe5aSKU:APA91bE2BbDi8f30j4IuMKCy8m8xBMZesT4UD7oBmOyaJWwr6LtR0PtXx2Kwlvo3IPuOQ4XsRxUiIuvbSThsinooNvbvqVCm1aw2uqyxg8mEyGQGzzU_SH8CjcT9aZ93qjJwPSmt0-Tk"
                },
                Data = JObject.Parse ("{ \"test\" : \"field\" }")
            });

            broker.Stop ();

        }
    }
}

