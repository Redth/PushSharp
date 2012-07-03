using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Common;

namespace PushSharp.Android
{
	public class GcmPushChannelSettings : PushChannelSettings
	{
		public GcmPushChannelSettings(string senderID, string senderAuthToken, string applicationIdPackageName)
		{
			this.SenderID = senderID;
			this.SenderAuthToken = senderAuthToken;
			this.ApplicationIdPackageName = applicationIdPackageName;
		}

		public string SenderID { get; private set; }
		public string SenderAuthToken { get; private set; }

		public string ApplicationIdPackageName { get; private set; }
	}
}
