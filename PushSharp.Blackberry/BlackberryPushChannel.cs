using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace PushSharp.Blackberry
{
	public class BlackberryPushChannel : Common.PushChannelBase
	{
		BlackberryPushChannelSettings blackberrySettings = null;

		public BlackberryPushChannel(BlackberryPushChannelSettings channelSettings, Common.PushServiceSettings serviceSettings = null) : base(channelSettings, serviceSettings)
		{
			blackberrySettings = channelSettings;
		}

		protected override void SendNotification(Common.Notification notification)
		{
			var bbn = notification as BlackberryNotification;

			if (bbn != null)
				push(bbn);
		}


		bool push(BlackberryNotification notification)
		{
			bool success = true;
			byte[] bytes = Encoding.ASCII.GetBytes(notification.Message);

			Stream requestStream = null;
			HttpWebResponse HttpWRes = null;
			HttpWebRequest HttpWReq = null;

			try
			{
				//http://<BESName>:<BESPort>/push?DESTINATTION=<PIN/EMAIL>&PORT=<PushPort>&REQUESTURI=/
				// Build the URL to define our connection to the BES.
				string httpURL = "http://" + this.blackberrySettings.BESAddress + ":" + blackberrySettings.BESWebServerListenPort.ToString()
					+ "/push?DESTINATION=" + notification.PushPin + "&PORT=" + blackberrySettings.BESPushPort.ToString()
					+ "&REQUESTURI=/";

				//make the connection
				HttpWReq = (HttpWebRequest)WebRequest.Create(httpURL);
				HttpWReq.Method = ("POST");
				//add the headers nessecary for the push
				HttpWReq.ContentType = "text/plain";
				HttpWReq.ContentLength = bytes.Length;
				// ******* Test this *******
				HttpWReq.Headers.Add("X-Rim-Push-Id", notification.PushPin + "~" + DateTime.Now); //"~" +pushedMessage +
				HttpWReq.Headers.Add("X-Rim-Push-Reliability", "application-preferred");
				HttpWReq.Headers.Add("X-Rim-Push-NotifyURL", (notification.WidgetNotificationUrl + notification.PushPin + "~" + notification.Message + "~" + DateTime.Now).Replace(" ", ""));

				// *************************
				HttpWReq.Credentials = new NetworkCredential(blackberrySettings.PushUsername, blackberrySettings.PushPassword);
				
				requestStream = HttpWReq.GetRequestStream();
				//Write the data from the source
				requestStream.Write(bytes, 0, bytes.Length);

				//get the response
				HttpWRes = (HttpWebResponse)HttpWReq.GetResponse();

				var pushStatus = HttpWRes.Headers["X-RIM-Push-Status"];

				//if the MDS received the push parameters correctly it will either respond with okay or accepted
				if (HttpWRes.StatusCode == HttpStatusCode.OK || HttpWRes.StatusCode == HttpStatusCode.Accepted)
				{
					success = true;
				}
				else
				{
					success = false;
				}
				//Close the streams

				HttpWRes.Close();
				requestStream.Close();
			}
			catch (System.Exception e)
			{
				success = false;
			}

			return success;
		}
	}
}
