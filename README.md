PushSharp
=========

A server-side library for sending Push Notifications to iOS (iPhone/iPad APNS), Android (C2DM), Windows Phone, and Blackberry devices!

![PushSharp Diagram](https://github.com/Redth/PushSharp/raw/master/Resources/PushSharp-Diagram.png)

Features
--------
 - Supports sending push notifications for many platforms:
   - Apple (APNS - iOS - iPhone, iPad)
   - Android (C2DM - Phone/Tablets)
   - Windows Phone 7 / 7.5 (and 8 presumably when it's released)
   - Blackberry (Not fully functional)
 - Fluent API for constructing Notifications for each platform
 - Auto Scaling of notification channels (more workers/connections are added as demand increases, and scaled down as it decreases)
 - Asynchronous code where possible, use of library is very event oriented
 - No third party dependencies! 100% managed code awesomeness for Mono compatibility!


Documentation
-------------
Head over to the Wiki for some documentation, guides, etc.
[Wiki](https://github.com/Redth/PushSharp/wiki)

Some sample action!
-------------------

Using the library should be easy, and the platform fairly abstracted away... Here's some sample code:
```csharp
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

//Fluent construction of a Windows Phone Toast notification
push.QueueNotification(NotificationFactory.WindowsPhone().Toast()
	.ForEndpointUri(new Uri("<DEVICE-ENDPOINT-URL-HERE"))
	.ForOSVersion(WindowsPhone.WindowsPhoneDeviceOSVersion.MangoSevenPointFive)
	.WithBatchingInterval(WindowsPhone.BatchingInterval.Immediate)
	.WithNavigatePath("/MainPage.xaml")
	.WithText1("Alert Text!")
	.WithText2("You have 7 new things!"));
	
//Fluent construction of an iOS notification
push.QueueNotification(NotificationFactory.Apple()
	.ForDeviceToken("<DEVICETOKEN>")
	.WithAlert("Alert Text!")
	.WithSound("sound.caf")
	.WithBadge(7));

//Fluent construction of an Android C2DM Notification
push.QueueNotification(NotificationFactory.Android()
	.ForDeviceRegistrationId("<C2DM-DEVICE-ID>")
	.WithCollapseKey("LATEST")
	.WithData("alert", "Alert Text!")
	.WithData("badge", "7"));
```	
	
Yet to Come
-----------
 - More thorough Testing!
 - Blackberry support
 
License
-------
Apache PushSharp
Copyright 2012 The Apache Software Foundation

This product includes software developed at
The Apache Software Foundation (http://www.apache.org/).
