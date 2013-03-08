using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Core;

namespace PushSharp.Android
{
	public interface IC2dmPushChannelSettings : IPushChannelSettings
	{
		string SenderID { get; }
		string Password { get; }
		string ApplicationID { get; }
	}


	[Obsolete("Google has Deprecated C2DM, and you should now use GCM Instead.")]
	public class C2dmPushChannelSettings : IPushChannelSettings
	{
		public C2dmPushChannelSettings(string senderID, string password, string applicationID)
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
