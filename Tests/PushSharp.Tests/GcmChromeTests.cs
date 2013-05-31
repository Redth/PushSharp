using System;
using System.Threading.Tasks;
using NUnit.Framework;
using PushSharp.Google.Chrome;
using System.Threading;

namespace PushSharp.Tests
{
	public class GcmChromeTests
	{
		const string oauthClientId = "785671162406-hdjh5dfe38bp2b6c2quvh2oelq0ie48i.apps.googleusercontent.com";
		const string oauthSecret = "GgYTVbDxX4JogGSeh9IxS83f";
		const string redirectUrl = "https://developers.google.com/oauthplayground";

		public GcmChromeTests ()
		{
		}

		[Test]
		public void TestOAuth()
		{
			var wait = new ManualResetEvent(false);

			var chan = new ChromePushChannel (new ChromePushChannelSettings(oauthClientId, oauthSecret)
            {
                AuthorizationCode = "4/Ndq0smr04tldagzUcJa9MFdamaAt.srbbbJRseYMTmmS0T3UFEsOetIXtfQI",
                RefreshToken = "1/3n7TKFv2xoHNBuKqHEXDLpiBZJVGkExnR1K_uHhU0H4"
			});

			var n = new ChromeNotification ();
            n.ChannelId = "14952429134341445468/nndgomkogokhhfiogeamahchcbkldiok";
            n.SubChannelId = ChromeNotificationSubChannel.SubChannel0;
			n.Payload = "Test";

			var cb = new PushSharp.Core.SendNotificationCallbackDelegate ((sender, response) => wait.Set());

			chan.SendNotification (n, cb);

			wait.WaitOne ();
		}
	}
}

