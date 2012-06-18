PushSharp
=========

A server-side library for sending Push Notifications to iOS (iPhone/iPad APNS), Android (C2DM), Windows Phone, and Blackberry devices!

Features
--------
 - Supports sending push notifications for many platforms:
   - Apple (APNS - iOS - iPhone, iPad)
   - Android (C2DM - Phone/Tablets)
   - Windows Phone 7 / 7.5
   - Blackberry (Still in alpha)
 - Fluent API for constructing Notifications for each platform
 - Auto Scaling of notification channels (more workers/connections are added as demand increases, and scaled down as it decreases)
 - Asynchronous code where possible, use of library is very event oriented
 - No third party dependencies!


Disclaimer: Beta Quality
---------------------------------
Ok, so since the first update, some code has been tested and appears to be working!  Apple APNS should be working reasonably well, as well as C2DM.  I'm working on testing WP7 and Blackberry.
I will warn that this code has not been heavily tested in production, so YMMV.  Having said that, it only gets better the more bugs are exposed, and since these were both based off previous libraries (APNS-Sharp and C2DM-Sharp) which I wrote, I feel that the code is pretty stable otherwise.


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
 - More thorough Testing!
 
