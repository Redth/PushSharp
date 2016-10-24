using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushSharp.Windows;

namespace PushSharp.Tests
{
	[TestClass]
	[Category("Disabled")]
	public class WnsRealTests
	{
		[TestMethod]
		public void WNS_Send_Single()
		{
			var succeeded = 0;
			var failed = 0;
			var attempted = 0;

			var config = new WnsConfiguration(Settings.Instance.WnsPackageName,
											 Settings.Instance.WnsPackageSid,
											 Settings.Instance.WnsClientSecret);

			var broker = new WnsServiceBroker(config);
			broker.OnNotificationFailed += (notification, exception) =>
			{
				failed++;
			};
			broker.OnNotificationSucceeded += (notification) =>
			{
				succeeded++;
			};

			broker.Start();

			foreach (var uri in Settings.Instance.WnsChannelUris)
			{
				attempted++;
				broker.QueueNotification(new WnsToastNotification
				{
					ChannelUri = uri,
					Payload = XElement.Parse(@"
                        <toast>
                            <visual>
                                <binding template=""ToastText01"">
                                    <text id=""1"">WNS_Send_Single</text>
                                </binding>  
                            </visual>
                        </toast>
                    ")
				});
			}

			broker.Stop();

			Assert.AreEqual(attempted, succeeded);
			Assert.AreEqual(0, failed);
		}

		[TestMethod]
		public void WNS_Send_Mutiple()
		{
			var succeeded = 0;
			var failed = 0;
			var attempted = 0;

			var config = new WnsConfiguration(Settings.Instance.WnsPackageName,
					Settings.Instance.WnsPackageSid,
					Settings.Instance.WnsClientSecret);

			var broker = new WnsServiceBroker(config);
			broker.OnNotificationFailed += (notification, exception) =>
			{
				failed++;
			};
			broker.OnNotificationSucceeded += (notification) =>
			{
				succeeded++;
			};

			broker.Start();

			foreach (var uri in Settings.Instance.WnsChannelUris)
			{
				for (var i = 1; i <= 3; i++)
				{
					attempted++;
					broker.QueueNotification(new WnsToastNotification
					{
						ChannelUri = uri,
						Payload = XElement.Parse(@"
                                <toast>
                                    <visual>
                                        <binding template=""ToastText01"">
                                            <text id=""1"">WNS_Send_Multiple " + i.ToString() + @"</text>
                                        </binding>  
                                    </visual>
                                </toast>
                            ")
					});
				}
			}

			broker.Stop();

			Assert.AreEqual(attempted, succeeded);
			Assert.AreEqual(0, failed);
		}
	}
}

