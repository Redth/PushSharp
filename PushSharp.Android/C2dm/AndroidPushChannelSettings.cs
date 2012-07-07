﻿using System;
using PushSharp.Common;

namespace PushSharp.Android
{
	[Obsolete("Google has Deprecated C2DM, and you should now use GCM Instead.")]
	public class AndroidPushChannelSettings : PushChannelSettings
	{
		public AndroidPushChannelSettings(string senderID, string password, string applicationID)
		{
			this.SenderID = senderID;
			this.Password = password;
			this.ApplicationID = applicationID;
		}

		public string SenderID { get; private set; }
		public string Password { get; private set; }

		public string ApplicationID { get; private set; }
	}
}
