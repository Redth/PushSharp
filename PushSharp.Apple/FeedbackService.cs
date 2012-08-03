﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace PushSharp.Apple
{
	public class FeedbackService
	{
		public delegate void FeedbackReceivedDelegate(string deviceToken, DateTime timestamp);
		public event FeedbackReceivedDelegate OnFeedbackReceived;

		public void RaiseFeedbackReceived(string deviceToken, DateTime timestamp)
		{
			var evt = this.OnFeedbackReceived;
			if (evt != null)
				evt(deviceToken, timestamp);
		}

		public void Run(ApplePushChannelSettings settings)
		{
			Run(settings, (new CancellationTokenSource()).Token);
		}

		public void Run(ApplePushChannelSettings settings, CancellationToken cancelToken)
		{
			var encoding = Encoding.ASCII;

			var certificate = settings.Certificate;

			var certificates = new X509CertificateCollection();
			certificates.Add(certificate);


			var client = new TcpClient(settings.FeedbackHost, settings.FeedbackPort);

			var stream = new SslStream(client.GetStream(), true,
				(sender, cert, chain, sslErrs) => { return true; },
				(sender, targetHost, localCerts, remoteCert, acceptableIssuers) => { return certificate; });

			stream.AuthenticateAsClient(settings.FeedbackHost, certificates, System.Security.Authentication.SslProtocols.Ssl3, false);


			//Set up
			byte[] buffer = new byte[38];
			int recd = 0;
			DateTime minTimestamp = DateTime.Now.AddYears(-1);

			//Get the first feedback
			recd = stream.Read(buffer, 0, buffer.Length);

			//Continue while we have results and are not disposing
			while (recd > 0 && !cancelToken.IsCancellationRequested)
			{
				try
				{

					//Get our seconds since 1970 ?
					byte[] bSeconds = new byte[4];
					byte[] bDeviceToken = new byte[32];

					Array.Copy(buffer, 0, bSeconds, 0, 4);

					//Check endianness
					if (BitConverter.IsLittleEndian)
						Array.Reverse(bSeconds);

					int tSeconds = BitConverter.ToInt32(bSeconds, 0);

					//Add seconds since 1970 to that date, in UTC and then get it locally
					var timestamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(tSeconds).ToLocalTime();

					//flag to allow feedback times in UTC or local, but default is local
					if (!settings.FeedbackTimeIsUTC)
						timestamp = timestamp.ToLocalTime();
				
					//Now copy out the device token
					Array.Copy(buffer, 6, bDeviceToken, 0, 32);

					var deviceToken = BitConverter.ToString(bDeviceToken).Replace("-", "").ToLower().Trim();

					//Make sure we have a good feedback tuple
					if (deviceToken.Length == 64
						&& timestamp > minTimestamp)
					{
						//Raise event
						RaiseFeedbackReceived(deviceToken, timestamp);
					}

				}
				catch { }

				//Clear our array to reuse it
				Array.Clear(buffer, 0, buffer.Length);

				//Read the next feedback
				recd = stream.Read(buffer, 0, buffer.Length);
			}
			

			
		}
	}
}
