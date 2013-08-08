using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PushSharp;
using PushSharp.Apple;

namespace PushSharp.Tests
{
	[TestFixture]
    public class CoreTests
	{
		[Test]
		public void Test_ApplicationId_Registrations()
		{
			var b = new PushBroker ();
			b.RegisterGcmService (new PushSharp.Android.GcmPushChannelSettings (""), "APP1");
			b.RegisterGcmService (new PushSharp.Android.GcmPushChannelSettings (""), "APP2");


			Assert.AreEqual (b.GetRegistrations ("APP1").Count (), 1, "Expected 1 APP1 Registration");
			Assert.AreEqual (b.GetRegistrations ("APP2").Count (), 1, "Expected 1 APP2 Registration");

			b.StopAllServices ("APP1");
			Assert.AreEqual (b.GetRegistrations ("APP1").Count (), 0, "Expected 0 APP1 Registrations");
			Assert.AreEqual (b.GetAllRegistrations ().Count (), 1, "Expected 1 Registration");

			b.StopAllServices ("APP2");
			Assert.AreEqual (b.GetRegistrations ("APP2").Count (), 0, "Expected 0 APP2 Registrations");
			Assert.AreEqual (b.GetAllRegistrations ().Count (), 0, "Expected 0 Registrations");
		}

		[Test]
		public void Test_NotificationType_Registrations()
		{
			var b = new PushBroker ();

			var gcm = new PushSharp.Android.GcmPushService (new PushSharp.Android.GcmPushChannelSettings (""));

			b.RegisterService<PushSharp.Android.GcmNotification> (gcm);
			Assert.AreEqual (b.GetRegistrations<PushSharp.Android.GcmNotification> ().Count (), 1, "Expected 1 GcmNotification Registration");

			b.RegisterService<PushSharp.Android.C2dmNotification> (gcm);
			Assert.AreEqual (b.GetRegistrations<PushSharp.Android.C2dmNotification> ().Count (), 1, "Expected 1 C2dmNotification Registration");

			b.StopAllServices<PushSharp.Android.GcmNotification> ();
			Assert.AreEqual (b.GetRegistrations<PushSharp.Android.GcmNotification> ().Count (), 0, "Expected 0 GcmNotification Registrations");

			b.StopAllServices<PushSharp.Android.C2dmNotification> ();
			Assert.AreEqual (b.GetRegistrations<PushSharp.Android.C2dmNotification> ().Count (), 0, "Expected 0 C2dmNotification Registrations");

			Assert.AreEqual (b.GetAllRegistrations ().Count (), 0, "Expected 0 Registrations");
		}

		[Test]
		public void Test_ApplicationId_And_NotificationType_Registrations()
		{
			var b = new PushBroker ();

			var gcm = new PushSharp.Android.GcmPushService (new PushSharp.Android.GcmPushChannelSettings (""));

			b.RegisterService<PushSharp.Android.GcmNotification> (gcm, "APP1");
			Assert.AreEqual (b.GetRegistrations<PushSharp.Android.GcmNotification> ("APP1").Count (), 1, "Expected 1 GcmNotification APP1 Registration");

			b.RegisterService<PushSharp.Android.GcmNotification> (gcm, "APP2");
			Assert.AreEqual (b.GetRegistrations<PushSharp.Android.GcmNotification> ("APP2").Count (), 1, "Expected 1 GcmNotification APP2 Registration");

			Assert.AreEqual (b.GetRegistrations<PushSharp.Android.GcmNotification> ().Count (), 2, "Expected 2 GcmNotification Registrations");


			b.RegisterService<PushSharp.Android.C2dmNotification> (gcm, "APP1");
			Assert.AreEqual (b.GetRegistrations<PushSharp.Android.C2dmNotification> ("APP1").Count (), 1, "Expected 1 C2dmNotification APP1 Registration");

			b.RegisterService<PushSharp.Android.C2dmNotification> (gcm, "APP2");
			Assert.AreEqual (b.GetRegistrations<PushSharp.Android.C2dmNotification> ("APP2").Count (), 1, "Expected 1 C2dmNotification APP2 Registration");

			Assert.AreEqual (b.GetRegistrations<PushSharp.Android.C2dmNotification> ().Count (), 2, "Expected 2 C2dmNotification Registrations");


			Assert.AreEqual (b.GetRegistrations ("APP1").Count (), 2, "Expected 2 APP1 Registrations");
			Assert.AreEqual (b.GetRegistrations ("APP2").Count (), 2, "Expected 2 APP2 Registrations");

			//Now remove GCM by type
			b.StopAllServices<PushSharp.Android.GcmNotification> ();
			Assert.AreEqual (b.GetRegistrations<PushSharp.Android.GcmNotification> ().Count (), 0, "Expected 0 GcmNotfication Registrations");
			Assert.AreEqual (b.GetRegistrations<PushSharp.Android.C2dmNotification> ().Count (), 2, "Expected 2 C2dmNotification Registrations");
			Assert.AreEqual (b.GetRegistrations ("APP1").Count (), 1, "Expected 1 APP1 Registration");
			Assert.AreEqual (b.GetRegistrations ("APP2").Count (), 1, "Expected 1 APP2 Registration");

			//Now remove APP1
			b.StopAllServices ("APP1");
			Assert.AreEqual (b.GetRegistrations ("APP1").Count (), 0, "Expected 0 APP1 Registration");
			Assert.AreEqual (b.GetRegistrations ("APP2").Count (), 1, "Expected 1 APP2 Registration");

			//Now remove APP2 
			b.StopAllServices ("APP2");
			Assert.AreEqual (b.GetRegistrations ("APP2").Count (), 0, "Expected 0 APP2 Registration");

			Assert.AreEqual (b.GetAllRegistrations ().Count (), 0, "Expected 0 Registrations");
		}


		[Test]
		public void Test_StopAll()
		{
			var b = new PushBroker();

			b.RegisterWindowsPhoneService ("APP1");
			b.RegisterWindowsPhoneService ("APP2");

			Assert.AreEqual (b.GetAllRegistrations ().Count (), 12, "Expected Registrations");

			b.StopAllServices ();

			Assert.AreEqual (b.GetAllRegistrations ().Count (), 0, "Expected 0 Registrations");
		}
    }
}
