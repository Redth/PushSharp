using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Core;

namespace PushSharp.Android
{
	public interface IGcmPushChannelSettings : IPushChannelSettings
	{
		string SenderID { get; }
		string SenderAuthToken { get; }
		string ApplicationIdPackageName { get; }
	}
	
	public class GcmPushChannelSettings : IGcmPushChannelSettings
	{
		public GcmPushChannelSettings(string senderAuthToken)
		{
			this.SenderAuthToken = senderAuthToken;
		}

		public GcmPushChannelSettings(string optionalSenderID, string senderAuthToken, string optionalApplicationIdPackageName)
		{
			this.SenderID = optionalSenderID;
			this.SenderAuthToken = senderAuthToken;
			this.ApplicationIdPackageName = optionalApplicationIdPackageName;
		}
		
		public string SenderID { get; private set; }
		public string SenderAuthToken { get; private set; }
		public string ApplicationIdPackageName { get; private set; }
	}
}
