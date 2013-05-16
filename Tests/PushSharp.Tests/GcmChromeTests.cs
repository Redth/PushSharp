using System;
using System.Threading.Tasks;
using NUnit.Framework;

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
			Google.Chrome.ChromePushChannel chan = new PushSharp.Google.Chrome.ChromePushChannel (new PushSharp.Google.Chrome.ChromePushChannelSettings(oauthClientId, oauthSecret)
            {
				GrantType = "4/m11YZRjo5yoYyuy3Wx8bIY1NtN3I.kvLUtsadF1YSuJJVnL49Cc_yg3RGfQI", RefreshToken = "1/737Kzd3sjIr8ME97HYam3fPW8euce6lHeP800RUXl8Y"
			});
			chan.RefreshAccessToken ();
		}
	}
}

