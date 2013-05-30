using System;
using NUnit.Framework;
using PushSharp.Amazon.Adm;
using System.Threading;

namespace PushSharp.Tests
{
	public class AmazonAdmTests
	{
		[Test]
		public void AmazonAdm_Simple_Test()
		{
			var wait = new ManualResetEvent(false);

			var settings = new AdmPushChannelSettings ("client_id", "client_secret");

			var adm = new AdmPushChannel (settings);

			var n = new AdmNotification ();
			n.Data.Add ("Test", "value");
			n.RegistrationId = "12345";
            
			adm.SendNotification (n, (sender, response) => wait.Set());

			wait.WaitOne ();
		}
	}
}

