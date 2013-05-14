using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Core;

namespace PushSharp.Facebook
{
	public class FacebookPushChannelSettings : IPushChannelSettings
	{

		public FacebookPushChannelSettings(string p_AppAccessToken)
		{
			this.AppAccessToken = p_AppAccessToken;
		}

		public FacebookPushChannelSettings(string p_AppId, string p_AppSecret)
		{
			this.AppId = p_AppId;
			//this.AppAccessToken = string.em;
			this.AppSecret = p_AppSecret;
			//this.FacebookUrl = Facebook_SEND_URL;
		}

		public string AppId { get; private set; }
		public string AppSecret { get; private set; }
		public string AppAccessToken { get; private set; }
		
	}
}
