using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Notification;
using System.Text;

namespace PushSharp.ClientSample.WindowsPhone
{
	public class PushSharpClient
	{
		public void RegisterForToast()
		{
			/// Holds the push channel that is created or found.
			HttpNotificationChannel pushChannel;

			// The name of our push channel.
			string channelName = "PushSharp.NotificationChannel.Tile";
						
			// Try to find the push channel.
			pushChannel = HttpNotificationChannel.Find(channelName);

			// If the channel was not found, then create a new connection to the push service.
			if (pushChannel == null)
			{
				pushChannel = new HttpNotificationChannel(channelName);

				// Register for all the events before attempting to open the channel.
				pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>((sender, e) => {
					//Tell server of the new channel uri
					System.Diagnostics.Debug.WriteLine("PushChannel URI Updated: " + e.ChannelUri.ToString());
	
				});
				pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>((sender, e) =>
				{
					//Error occurred
					System.Diagnostics.Debug.WriteLine("PushChannel Error: " + e.ErrorType.ToString() + " -> " + e.ErrorCode + " -> " + e.Message + " -> " + e.ErrorAdditionalData);
				});

				//// Register for this notification only if you need to receive the notifications while your application is running.
				//pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>((sender, e) =>
				//{
				//	//Yay notification
				//});

				pushChannel.Open();
			}
			else
			{
				// The channel was already open, so just register for all the events.
				pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>((sender, e) => {
					//Updated uri
					System.Diagnostics.Debug.WriteLine("PushChannel URI Updated: " + e.ChannelUri.ToString());
				});
				pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>((sender, e) =>
				{
					//Error occurred
					System.Diagnostics.Debug.WriteLine("PushChannel Error: " + e.ErrorType.ToString() + " -> " + e.ErrorCode + " -> " + e.Message + " -> " + e.ErrorAdditionalData);
				});

				// Bind this new channel for toast events.
				if (pushChannel.IsShellToastBound)
					Console.WriteLine("Already Bound to Toast");
				else
					pushChannel.BindToShellToast();

				if (pushChannel.IsShellTileBound)
					Console.WriteLine("Already Bound to Tile");
				else
					pushChannel.BindToShellTile();

				//// Register for this notification only if you need to receive the notifications while your application is running.
				//pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>((sender, e) =>
				//{
				//	//Yay
				//});
			}

			// Bind this new channel for toast events.
			if (pushChannel.IsShellToastBound)
				Console.WriteLine("Already Bound to Toast");
			else
				pushChannel.BindToShellToast();

			if (pushChannel.IsShellTileBound)
				Console.WriteLine("Already Bound to Tile");
			else
				pushChannel.BindToShellTile();

			// Display the URI for testing purposes. Normally, the URI would be passed back to your web service at this point.
			if (pushChannel != null && pushChannel.ChannelUri != null)
				System.Diagnostics.Debug.WriteLine(pushChannel.ChannelUri.ToString());
		}
	}
}
