using System;
using NUnit.Framework;
using PushSharp.Blackberry;
using System.Threading;

namespace PushSharp.Tests
{
	public class BlackberryTests
	{
		public BlackberryTests ()
		{
		}

		[Test]
		public void Blackberry_Simple_Test()
		{
			var waitCallback = new ManualResetEvent (false);

		    var settings = new BlackberryPushChannelSettings("APPID", "PWD"); 

			var c = new BlackberryPushChannel (settings);

			var n = new BlackberryNotification ();
		    n.Content = new BlackberryMessageContent("text/plain", "66");

			n.QualityOfService = QualityOfServiceLevel.Unconfirmed;
		    n.SourceReference = "3682-8s4B97Rc2388i46I8M08769D005ra6286o4";
		    n.DeliverBeforeTimestamp = DateTime.UtcNow.AddMinutes(1);
			var r = new BlackberryRecipient ("push_all");

			n.Recipients.Add (r);

			c.SendNotification (n, (sender, resp) => {

				waitCallback.Set();
			});

			waitCallback.WaitOne ();
		}
	}
}

