PushSharp
=========

A server-side library for sending Push Notifications to iOS (iPhone/iPad APNS), Android (C2DM), Windows Phone, and Blackberry devices!

Disclaimer: Only for the Brave
------------------------------
There's finally some code available here to look at, but I must warn you, this code is completely untested as of now!  I mean it, I haven't even confirmed that the Push Channels can actually send notifications successfully!  It compiles, but past that, venture here at your own risk!  

I will obviously start trying to do some testing in the near future, but until then, feel free to fork it, watch it, play with it, and let me know of any obvious mistakes or errors!  Thanks!

Some sample Usage
-----------------

Using the library should be easy, and the platform fairly abstracted away... Here's some sample code:

	//Create our service	
	PushService push = new PushService();
  
	//Wire up the events
	push.Events.OnDeviceSubscriptionExpired += new Common.ChannelEvents.DeviceSubscriptionExpired(Events_OnDeviceSubscriptionExpired);
	push.Events.OnChannelException += new Common.ChannelEvents.ChannelExceptionDelegate(Events_OnChannelException);
	push.Events.OnNotificationSendFailure += new Common.ChannelEvents.NotificationSendFailureDelegate(Events_OnNotificationSendFailure);
	push.Events.OnNotificationSent += new Common.ChannelEvents.NotificationSentDelegate(Events_OnNotificationSent);
  
	//Configure and start Apple APNS
	var appleCert = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppleSandbox.p12"));
	push.StartApplePushService(new ApplePushChannelSettings(false, appleCert, "test"));
  
	//Configure and start Android C2DM
	push.StartAndroidPushService(new Android.AndroidPushChannelSettings("<SENDERID>", "test", "<APPID>"));
  
	//Configure and start Windows Phone Notifications
	push.StartWindowsPhonePushService(new WindowsPhone.WindowsPhonePushChannelSettings());
  
	//Fluent construction of an iOS notification
	push.QueueNotification(NotificationFactory.Apple()
		.ForDeviceToken("<DEVICETOKEN>")
		.WithAlert("Alert Text!")
		.WithBadge(7));
  
	//Fluent construction of an Android C2DM Notification
	push.QueueNotification(NotificationFactory.Android()
		.ForDeviceRegistrationId("<C2DM-DEVICE-ID>")
		.WithData("alert", "Alert Text!")
		.WithData("badge", "7"));
	
	
Yet to Come
-----------
 - Fluent API for Notification construction
 - Testing!
 
