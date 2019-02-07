using System;
using NUnit.Framework;
using PushSharp.Google;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace PushSharp.Tests
{
    [Category ("FCM")]
    [TestFixture]
    public class FcmTests
    {
        [Test]
        public void FcmNotification_Priority_Should_Serialize_As_String_High ()
        {
            var n = new FcmNotification ();
            n.Priority = FcmNotificationPriority.High;

            var str = n.ToString ();

            Assert.IsTrue (str.Contains ("high"));
        }

        [Test]
        public void FcmNotification_Priority_Should_Serialize_As_String_Normal ()
        {
            var n = new FcmNotification ();
            n.Priority = FcmNotificationPriority.Normal;

            var str = n.ToString ();

            Assert.IsTrue (str.Contains ("normal"));
        }
    }
}

