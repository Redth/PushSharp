using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using PushSharp.Core;

namespace PushSharp.Android
{
	[Obsolete("Google has Deprecated C2DM, and you should now use GCM Instead.")]
	public class C2dmPushChannel : PushChannelBase
	{
		C2dmPushChannelSettings androidSettings = null;
		string googleAuthToken = string.Empty;
		C2dmMessageTransportAsync transport;
		long waitCounter = 0;

		public C2dmPushChannel(IPushService pushService) : base(pushService)
		{
			androidSettings = pushService.ChannelSettings as C2dmPushChannelSettings;

			//Go get the auth token from google
			try
			{
				RefreshGoogleAuthToken();
			}
			catch (GoogleLoginAuthorizationException glaex)
			{
				this.Events.RaiseChannelException(this, glaex);
			}

			transport = new C2dmMessageTransportAsync();
			transport.UpdateGoogleClientAuthToken += new Action<string>((newToken) =>
			{
				this.googleAuthToken = newToken;
			});

			transport.MessageResponseReceived += new Action<C2dmMessageTransportResponse>(transport_MessageResponseReceived);

			transport.UnhandledException += new Action<C2dmNotification, Exception>(transport_UnhandledException);
		}

		void transport_UnhandledException(C2dmNotification notification, Exception exception)
		{
			this.Events.RaiseChannelException(this, exception);

			Interlocked.Decrement(ref waitCounter);
		}

		void transport_MessageResponseReceived(C2dmMessageTransportResponse response)
		{
			//Check if our token was expired and refresh/requeue if need be
			if (response.ResponseCode == MessageTransportResponseCode.InvalidAuthToken)
			{
				this.PushService.QueueNotification(response.Message, false);
				this.RefreshGoogleAuthToken();
				return;
			}

			if (response.ResponseStatus == MessageTransportResponseStatus.Ok)
				this.Events.RaiseNotificationSent(this, response.Message); //Msg ok!
			else if (response.ResponseStatus == MessageTransportResponseStatus.InvalidRegistration)
			{
				//Device subscription is no good!
				this.Events.RaiseDeviceSubscriptionExpired(this, response.Message.RegistrationId, response.Message);
			}
			else if (response.ResponseStatus == MessageTransportResponseStatus.NotRegistered)
			{
				//Device must have uninstalled app
				this.Events.RaiseDeviceSubscriptionExpired(this, response.Message.RegistrationId, response.Message);
			}
			else
			{
				//Message Failed some other way
				this.Events.RaiseNotificationSendFailure(this, response.Message, new Exception(response.ResponseStatus.ToString()));
			}

			Interlocked.Decrement(ref waitCounter);
		}

		public override void SendNotification(Notification notification)
		{
			Interlocked.Increment(ref waitCounter);
			transport.Send(notification as C2dmNotification, this.googleAuthToken, androidSettings.SenderID, androidSettings.ApplicationID);
		}

		public override void Stop()
		{
			base.Stop();

			var slept = 0;
			while (Interlocked.Read(ref waitCounter) > 0 && slept <= 5000)
			{
				slept += 100;
				Thread.Sleep(100);
			}
		}

		/// <summary>
		/// Explicitly refreshes the Google Auth Token.  Usually not necessary.
		/// </summary>
		public void RefreshGoogleAuthToken()
		{
			string authUrl = "https://www.google.com/accounts/ClientLogin";

			var data = new NameValueCollection();

			data.Add("Email", this.androidSettings.SenderID);
			data.Add("Passwd", this.androidSettings.Password);
			data.Add("accountType", "GOOGLE_OR_HOSTED");
			data.Add("service", "ac2dm");
			data.Add("source", this.androidSettings.ApplicationID);

			var wc = new WebClient();

			try
			{
				var authStr = Encoding.ASCII.GetString(wc.UploadValues(authUrl, data));

				//Only care about the Auth= part at the end
				if (authStr.Contains("Auth="))
					googleAuthToken = authStr.Substring(authStr.IndexOf("Auth=") + 5);
				else
					throw new GoogleLoginAuthorizationException("Missing Auth Token");
			}
			catch (WebException ex)
			{
				var result = "Unknown Error";
				try { result = (new System.IO.StreamReader(ex.Response.GetResponseStream())).ReadToEnd(); }
				catch { }

				throw new GoogleLoginAuthorizationException(result);
			}
		}
	}
}
