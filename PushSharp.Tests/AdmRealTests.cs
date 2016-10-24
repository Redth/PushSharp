using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushSharp.Amazon;

namespace PushSharp.Tests
{
	[TestClass]
	public class AdmRealTests
	{
		[TestMethod]
		[Category("Disabled")]
		public void ADM_Send_Single()
		{
			var succeeded = 0;
			var failed = 0;
			var attempted = 0;

			var config = new AdmConfiguration(Settings.Instance.AdmClientId, Settings.Instance.AdmClientSecret);
			var broker = new AdmServiceBroker(config);
			broker.OnNotificationFailed += (notification, exception) =>
			{
				failed++;
			};
			broker.OnNotificationSucceeded += (notification) =>
			{
				succeeded++;
			};
			broker.Start();

			foreach (var regId in Settings.Instance.AdmRegistrationIds)
			{
				attempted++;
				broker.QueueNotification(new AdmNotification
				{
					RegistrationId = regId,
					Data = new Dictionary<string, string> {
												{ "somekey", "somevalue" }
										}
				});
			}

			broker.Stop();

			Assert.AreEqual(attempted, succeeded);
			Assert.AreEqual(0, failed);
		}
	}
}

