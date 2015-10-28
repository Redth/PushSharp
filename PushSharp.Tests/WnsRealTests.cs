using System;
using NUnit.Framework;
using PushSharp.Windows;
using System.Xml.Linq;

namespace PushSharp.Tests
{
    [TestFixture]
    public class WnsRealTests
    {
        [Test]
        public void WNS_Send_Single ()
        {
            var succeeded = 0;
            var failed = 0;
            var attempted = 0;

            var config = new WnsConfiguration (Settings.Instance.WnsPackageName,
                             Settings.Instance.WnsPackageSid,
                             Settings.Instance.WnsClientSecret);

            var broker = new WnsServiceBroker (config);
            broker.OnNotificationFailed += (notification, exception) => {
                failed++;
            };
            broker.OnNotificationSucceeded += (notification) => {
                succeeded ++;
            };

            broker.Start ();

            foreach (var uri in Settings.Instance.WnsChannelUris) {
                attempted++;
                broker.QueueNotification (new WnsToastNotification {
                    ChannelUri = uri,
                    Payload = XElement.Parse (@"
                        <toast>
                            <visual>
                                <binding template=""ToastText01"">
                                    <text id=""1"">bodyText</text>
                                </binding>  
                            </visual>
                        </toast>
                    ")
                });
            }

            broker.Stop ();

            Assert.AreEqual (attempted, succeeded);
            Assert.AreEqual (0, failed);
        }
    }
}

