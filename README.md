PushSharp
=========

A server-side library for sending Push Notifications to iOS (iPhone/iPad APNS), Android (C2DM - soon Google Cloud Message), Windows Phone, and Blackberry devices!

**UPDATE: August 28, 2012** Windows 8 support is now in master, however it's very untested!

**UPDATE: July 13, 2012** I've started working on Windows 8 support, check out the Windows-8 branch!

**UPDATE: July 3, 2012** Google GCM branch has now been merged into the master branch, and we now support Google Cloud Messaging!

![PushSharp Diagram](https://github.com/Redth/PushSharp/raw/master/Resources/PushSharp-Diagram.png)

Features
--------
 - Supports sending push notifications for many platforms:
   - Apple (APNS - iPhone, iPad, Mountain Lion)
   - Android (GCM/C2DM - Phones/Tablets)
   - Windows Phone 7 / 7.5 (and 8 presumably when it's released)
   - Windows 8 (BETA - Not well tested yet!)
   - Blackberry (Not fully functional / Tested - Looking for help here)
 - Fluent API for constructing Notifications for each platform
 - Auto Scaling of notification channels (more workers/connections are added as demand increases, and scaled down as it decreases)
 - Asynchronous code where possible, use of library is very event oriented
 - 100% managed code awesomeness for Mono compatibility!


Documentation
-------------
Head over to the [Wiki](https://github.com/Redth/PushSharp/wiki) for some documentation, guides, etc.
 - [How to Configure & Send Apple Push Notifications using PushSharp](https://github.com/Redth/PushSharp/wiki/How-to-Configure-&-Send-Apple-Push-Notifications-using-PushSharp)
 - [How to Configure & Send Android GCM Google Cloud Messaging Push Notifications using PushSharp](https://github.com/Redth/PushSharp/wiki/How-to-Configure-&-Send-GCM-Google-Cloud-Messaging-Push-Notifications-using-PushSharp)

PushSharp Featured in Xamarin Seminar!
--------------------------------------
On August 9th, 2012, I had the great honor of hosting a Xamarin Seminar on Push Notifications, and introducing PushSharp.  If you missed it, the video and slides are all online now!
 - [Push Notifications - Introduction to PushSharp - Video](http://www.youtube.com/watch?v=MytQ6vqrE5g)
 - [Push Notifications - Introduction to PushSharp - Slides](http://www.slideshare.net/Xamarin/push-notifications-introduction-to-pushsharp-seminar)

Some sample action!
-------------------

Using the library to send push notifications should be easy, and the platform fairly abstracted away... Here's some sample code:
```csharp
//Create our service	
PushService push = new PushService();

//Wire up the events
push.Events.OnDeviceSubscriptionExpired += new Common.ChannelEvents.DeviceSubscriptionExpired(Events_OnDeviceSubscriptionExpired);
push.Events.OnDeviceSubscriptionIdChanged += new Common.ChannelEvents.DeviceSubscriptionIdChanged(Events_OnDeviceSubscriptionIdChanged);
push.Events.OnChannelException += new Common.ChannelEvents.ChannelExceptionDelegate(Events_OnChannelException);
push.Events.OnNotificationSendFailure += new Common.ChannelEvents.NotificationSendFailureDelegate(Events_OnNotificationSendFailure);
push.Events.OnNotificationSent += new Common.ChannelEvents.NotificationSentDelegate(Events_OnNotificationSent);

//Configure and start Apple APNS
var appleCert = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppleSandbox.p12"));
push.StartApplePushService(new ApplePushChannelSettings(false, appleCert, "test"));

//Configure and start Android C2DM
push.StartGoogleCloudMessagingPushService(new GcmPushChannelSettings("<YOUR_API_SENDER_ID>", 
  "<YOUR_API_ACCESS_API_KEY_FOR_SERVER_APP>", "<YOUR_ANDROID_APP_PACKAGE_NAME>"));

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
	.ForDeviceRegistrationId("<GCM-DEVICE-REGISTRATION_ID>")
	.WithCollapseKey("LATEST")
	.WithJson("{\"alert\":\"Alert Text!\",\"badge\":\"7\"}"));
```	
	

**MonoTouch** and **Mono for Android** Client Application Integration
---------------------------------------------------------------------
Given that PushSharp is written in C#, you probably thought there was a good chance that it's being used somewhere with MonoTouch or Mono for Android... and you would be correct!  There are samples of how to setup the client app push notification end of things included in the PushSharp project source!  There's even a Windows Phone 7.5 project to show how to register for notifications!

All client samples can be found in the **/Client.Samples/** folder.  

PushSharp.ClientSample.**MonoForAndroid** 
-----------------------------------------
There are two projects for Mono For Android:

1. PushSharp.ClientSample.C2dm
2. PushSharp.ClientSample.Gcm

C2DM is now deprecated by Google, and you can ignore it unless you are working with an application that already uses it.  Otherwise, focus on the *GCM* project.  

The GCM project also references the ***/PushSharp.Client/PushSharp.Client.MonoForAndroid.Gcm*** project which is a client library (ported from the java gcm-client library).  It is a helper library for managing GCM registrations on the client side, within your Android app.  You don't necessarily need to understand the code in this library, but just know that this Client Sample project references it for its own use!  

Unfortunately, integration of GCM with Mono for Android is a bit tricky.  You will need to make sure you have the right permissions setup in the ***AndroidManifest.xml*** file, inside your *<manifest>* node:

```xml
<permission android:name="com.pushsharp.test.permission.C2D_MESSAGE" android:protectionLevel="signature" />
<uses-permission android:name="android.permission.GET_ACCOUNTS" />
<uses-permission android:name="android.permission.INTERNET" />
<uses-permission android:name="com.pushsharp.test.permission.C2D_MESSAGE" />
<uses-permission android:name="com.google.android.c2dm.permission.RECEIVE" />
<uses-permission android:name="android.permission.WAKE_LOCK" />
```

Make sure that you use your own app's package name where applicable:
> eg: "**your.package.name**.permission.C2D_MESSAGE" instead of "***com.pushsharp.test***.permission.C2D_MESSAGE" from the example

Next, take a look at the PushService.cs file in the sample project.  You can copy much of this class into your own App, but again be sure to substitute your own package name in where applicable (the BroadcastReceiver attributes need to be changed).  You will also need to change the SENDER_ID constant to your own (see the documentation for Configuring GCM with PushSharp in the wiki).  Finally, in this class, you will probably want to change what happens in some of the GCMIntentService methods.  In the OnRegistered, you would want to send the registration ID to your server, so that you can use it to send the device notifications.  You get the point.

In future versions I'm hoping that some of the limitations of Mono for Android will be addressed to allow me to incorporate the client sample in a more packaged up, easy to use library.  Until then, this is what you get!  Follow the directions closely! 


PushSharp.ClientSample.**MonoTouch**
------------------------------------
Registering for remote notifications in MonoTouch is fairly trivial.  The only real tricky part is figuring out how to get the deviceToken into a nice string that you can send to your server.  Check out *AppDelegate.cs* for details on how this is done in MonoTouch!


	
 
License
-------
Apache PushSharp
Copyright 2012 The Apache Software Foundation

This product includes software developed at
The Apache Software Foundation (http://www.apache.org/).
