PushSharp
=========

A server-side library for sending Push Notifications to iOS (iPhone/iPad APNS), Android (C2DM and GCM - Google Cloud Message), Windows Phone, Windows 8, and Blackberry devices!

![PushSharp Diagram](https://github.com/Redth/PushSharp/raw/master/Resources/PushSharp-Diagram.png)
*********
News
----
**January 23, 2013** New version with some substantial improvements to how Apple Notifications are processed :)

**September 6, 2012** Blog Post: [The Problem with Apple's Push Notification Service... Solutions and Workarounds...](http://redth.info/the-problem-with-apples-push-notification-ser)

**August 28, 2012** Windows 8 support is now in master, however it's very untested!

**July 13, 2012** I've started working on Windows 8 support, check out the Windows-8 branch!

**July 3, 2012** Google GCM branch has now been merged into the master branch, and we now support Google Cloud 
Messaging!

*******

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

*******

Documentation
-------------
Head over to the [Wiki](https://github.com/Redth/PushSharp/wiki) for some documentation, guides, etc.
 - [How to Configure & Send Apple Push Notifications using PushSharp](https://github.com/Redth/PushSharp/wiki/How-to-Configure-&-Send-Apple-Push-Notifications-using-PushSharp)
 - [How to Configure & Send Android GCM Google Cloud Messaging Push Notifications using PushSharp](https://github.com/Redth/PushSharp/wiki/How-to-Configure-&-Send-GCM-Google-Cloud-Messaging-Push-Notifications-using-PushSharp)

*******

PushSharp Featured in Xamarin Seminar!
--------------------------------------
On August 9th, 2012, I had the great honor of hosting a Xamarin Seminar on Push Notifications, and introducing PushSharp.  If you missed it, the video and slides are all online now!
 - [Push Notifications - Introduction to PushSharp - Video](http://www.youtube.com/watch?v=MytQ6vqrE5g)
 - [Push Notifications - Introduction to PushSharp - Slides](http://www.slideshare.net/Xamarin/push-notifications-introduction-to-pushsharp-seminar)

*******

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
********
	
The Bare Metal
-----------------
The primary goal of PushSharp is to allow you to ***easily*** send notifications without thinking to much about it.  However, there are going to be cases where you want more raw power.  Luckily, PushSharp was designed with that in mind!  See the diagram and explanation of the structure of PushSharp below:

![PushSharp Structure Diagram](https://github.com/Redth/PushSharp/raw/master/Resources/PushSharp-Structure.png)

PushSharp is composed of several tiers with each higher tier building upon the lower.

**PushService**

The purpose of PushService is to group all of the various push service platforms into a single class, so you have a single starting point for using all the services.

**PushServiceBase** 

Each platform has its own implementation of the PushServiceBase class.  The PushServiceBase for each platform is responsible for maintaining a collection of instances of PushChannelBase and distribute Queued notifications to them.  

Settings for the PushServiceBase instance can be changed through the PushServiceSettings object.  By default the PushServiceBase instance will attempt to auto scale up and down the instances of PushChannelBase, however this behaviour can be changed in the settings.  Each platform has an implementation of PushServiceBase (eg: ApplePushService, GcmPushService, WindowsPushService, etc.).

**PushChannelBase**

The Channel instance is the closest to the metal that you can get in PushSharp.  Each PushChannelBase instance represents a single *connection* or a single *channel* to the push service provider's cloud service.  In the case of Apple (ApplePushChannel), this means a single, open, TCP Socket connection to Apple's Push Notification gateway server.  However in the case of GCM, and Windows/WindowsPhone because the notification protocol is HTTP, it represents a single queue and a single worker for processing that queue.  

PushChannelBase requires a PushChannelSettings which has a different implementation for each platform (each platform requires different parameters for connecting to the platform's specific cloud service - eg: ApplePushChannelSettings, GcmPushChannelSettings, WindowsPushChannelSettings, etc).  

You can create instances of any of the implementations of PushChannelBase (ApplePushChannel, GcmPushChannel, WindowsPushChannel, etc.) and queue notifications to them directly, bypassing any of the PushService channel management features in the library, so that you are able to implement scaling as you see fit!


**Implementing your own**

The PushSharp.Common assembly contains all the necessary base classes to implement your own Push notification provider.  You should subclass the following classes when implementing your own provider:

- PushChannelBase
- PushChannelSettings
- PushServiceBase - optional

You PushChannelBase implementation should get all of its connection parameters from your PushChannelSettings implementation.  You don't have to implement PushServiceBase of course, but it is useful if you want to take advantage of some of its features such as auto-scaling and notification queue distribution.


********************

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

**VERY IMPORTANT NOTE:** Make sure your package name does not start with an uppercase character.  If it does, you will get cryptic errors in logcat and things will not work, because your package name is used in defining some permissions required by GCM. 
Eg: my.package.name is ok, but My.package.name is NOT

Next, take a look at the PushService.cs file in the sample project.  You can copy much of this class into your own App, but again be sure to substitute your own package name in where applicable (the BroadcastReceiver attributes need to be changed).  You will also need to change the SENDER_ID constant to your own (see the documentation for Configuring GCM with PushSharp in the wiki).  Finally, in this class, you will probably want to change what happens in some of the GCMIntentService methods.  In the OnRegistered, you would want to send the registration ID to your server, so that you can use it to send the device notifications.  You get the point.


PushSharp.ClientSample.**MonoTouch**
------------------------------------
Registering for remote notifications in MonoTouch is fairly trivial.  The only real tricky part is figuring out how to get the deviceToken into a nice string that you can send to your server.  Check out *AppDelegate.cs* for details on how this is done in MonoTouch!


****************	
 
License
-------
Apache PushSharp
Copyright 2012 The Apache Software Foundation

This product includes software developed at
The Apache Software Foundation (http://www.apache.org/).
