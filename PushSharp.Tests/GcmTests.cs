using System;
using NUnit.Framework;
using PushSharp.Google;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace PushSharp.Tests
{
    [Category ("GCM")]
    [TestFixture]
    public class GcmTests
    {
        [Test]
        public void GcmNotification_Priority_Should_Serialize_As_String_High ()
        {
            var n = new GcmNotification ();
            n.Priority = GcmNotificationPriority.High;

            var str = n.ToString ();

            Assert.IsTrue (str.Contains ("high"));
        }

        [Test]
        public void GcmNotification_Priority_Should_Serialize_As_String_Normal ()
        {
            var n = new GcmNotification ();
            n.Priority = GcmNotificationPriority.Normal;

            var str = n.ToString ();

            Assert.IsTrue (str.Contains ("normal"));
        }
    }
}

