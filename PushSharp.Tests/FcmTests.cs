using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushSharp.Firebase;

namespace PushSharp.Tests
{
	[Category("FCM")]
	[TestClass]
	public class FcmTests
	{
		[TestMethod]
		public void FcmNotification_Priority_Should_Serialize_As_String_High()
		{
			var n = new FcmNotification();
			n.Priority = FcmNotificationPriority.High;

			var str = n.ToString();

			Assert.IsTrue(str.Contains("high"));
		}

		[TestMethod]
		public void FcmNotification_Priority_Should_Serialize_As_String_Normal()
		{
			var n = new FcmNotification();
			n.Priority = FcmNotificationPriority.Normal;

			var str = n.ToString();

			Assert.IsTrue(str.Contains("normal"));
		}
	}
}

