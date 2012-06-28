using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Common;

namespace PushSharp.Android
{
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
