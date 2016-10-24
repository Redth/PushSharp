using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushSharp.Google;

namespace PushSharp.Tests
{
	[Category("GCM")]
	[TestClass]
	public class GcmTests
	{
		[TestMethod]
		public void GcmNotification_Priority_Should_Serialize_As_String_High()
		{
			var n = new GcmNotification();
			n.Priority = GcmNotificationPriority.High;

			var str = n.ToString();

			Assert.IsTrue(str.Contains("high"));
		}

		[TestMethod]
		public void GcmNotification_Priority_Should_Serialize_As_String_Normal()
		{
			var n = new GcmNotification();
			n.Priority = GcmNotificationPriority.Normal;

			var str = n.ToString();

			Assert.IsTrue(str.Contains("normal"));
		}
	}
}

