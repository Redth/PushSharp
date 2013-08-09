PushSharp [![Build Status](http://teamcity.codebetter.com/app/rest/builds/buildType:\(id:bt990\)/statusIcon)](http://teamcity.codebetter.com/project.html?projectId=project338&tab=projectOverview)
===========================

A server-side library for sending Push Notifications to iOS (iPhone/iPad APNS), OSX (APNS 10.8+) Android (C2DM and GCM - Google Cloud Message), Chrome (GCM) Windows Phone, Windows 8, Blackberry (PAP), and Amazon (ADM) devices!

![PushSharp Diagram](https://github.com/Redth/PushSharp/raw/master/Resources/PushSharp-Diagram.png)
*********

###PushSharp was at Evolve 2013!###
I was fortunate enough to attend and present at Evolve 2013 in Austin, and the video is now online! [http://xamarin.com/evolve/2013#session-b8fz8gfsnf](http://xamarin.com/evolve/2013#session-b8fz8gfsnf)


News
----
**August 8, 2013** 2.1.2-beta released, release notes below...

**June 3, 2013** Xamarin Evolve video on PushSharp is now online! [http://xamarin.com/evolve/2013#session-b8fz8gfsnf](http://xamarin.com/evolve/2013#session-b8fz8gfsnf)

**May 31, 2013** 2.1.1-beta is released!  See the release notes below...

**March 19, 2013** 2.0 is released! See the release notes below...

*******
**PushSharp Jabbr Channel!**  http://jabbr.net/#/rooms/PushSharp
******

Features
--------
 - Supports sending push notifications for many platforms:
   - Apple (APNS - iPhone, iPad, Mountain Lion)
   - Android (GCM/C2DM - Phones/Tablets)
   - Chrome (GCM)
   - Amazon (ADM - Amazon Device Messaging)
   - Windows Phone 7 / 7.5 / 8 (including FlipTile, CycleTile, and IconicTile Templates!)
   - Windows 8
   - Blackberry (BIS and BES via PAP)
   - Firefox OS (Coming soon)
 - Fluent API for constructing Notifications for each platform
 - Auto Scaling of notification channels (more workers/connections are added as demand increases, and scaled down as it decreases)
 - 100% managed code awesomeness for Mono compatibility!
 - Unit Tests
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

Sample Code
-------------------

Using the library to send push notifications should be easy, and the platform fairly abstracted away... Here's some sample code:

```csharp
//Create our push services broker
var push = new PushBroker();

//Registering the Apple Service and sending an iOS Notification
var appleCert = File.ReadAllBytes("ApnsSandboxCert.p12"));
push.RegisterAppleService(new ApplePushChannelSettings(appleCert, "pwd"));
push.QueueNotification(new AppleNotification()
                           .ForDeviceToken("DEVICE TOKEN HERE")
                           .WithAlert("Hello World!")
                           .WithBadge(7)
                           .WithSound("sound.caf"));


//Registering the GCM Service and sending an Android Notification
push.RegisterGcmService(new GcmPushChannelSettings("theauthorizationtokenhere"));
//Fluent construction of an Android GCM Notification
//IMPORTANT: For Android you MUST use your own RegistrationId here that gets generated within your Android app itself!
push.QueueNotification(new GcmNotification().ForDeviceRegistrationId("DEVICE REGISTRATION ID HERE")
                      .WithJson("{\"alert\":\"Hello World!\",\"badge\":7,\"sound\":\"sound.caf\"}"));

```	

Please see the PushSharp.Sample project for a more thorough example!

********************

v2.1.2 BETA Release Notes
--------------------------

v2.1.x is still BETA.  For iOS, Android, Windows, and Windows phone it should be quite stable still, however this is a first release of Blackberry and Amazon support!

**Changes**

 - Amazon Device Messaging support
 - Chrome GCM support
 - Blackberry BIS (and BES in theory) support
 - Performance enhancements
 - APNS Stability Improvements
 - Other bugfixes
 

v2.0 Release Notes
------------------

PushSharp v2.0 has finally arrived.  It is the culmination of hours of refactoring, decoupling, simplifying, and testing.  The main emphasis on this release was to refactor the code to decouple various classes so that Unit Tests could finally be written.  


**Changes**

 - PushService was renamed to PushBroker
 - PushSharp.Common is now PushSharp.Core
 - PushSharp assembly was merged with PushSharp.Core
 - PushSharp.Core no longer depends on each specific platform, but each specific platform depends on PushSharp.Core
 - Unit Tests are now available using NUnit and Moq
 - Push 'Channels' are now less sophisticated and not responsible for handling their own queues, which should result in fewer possible points of failure
 - Each specific platform assembly is now dependant on PushSharp.Core
 - Scaling logic was greatly improved
 - Apple (APNS) more stable and resilient to connection failures
 - Various other bugfixes

**PushBroker - Registering Services**

Since PushSharp.Core no longer references individual platform assemblies, it was not practical to keep the same pattern of pushSharpInstance.StartApplePushService(...).  Instead, PushBroker contains a .RegisterService<TNotification>(TPushService svc) method.  

In addition, each platform also includes extension methods to assist in registering the platform specific services.  If you are using the PushSharp namespace, for example, you could call pushBrokerInstance.RegisterAppleService(...) which works very similarly to how the old PushService methods worked!

See the sample for more info!

**Thanks!**
Special thanks goes out to the MvvmCross room on Jabbr, and especially Greg Shackles for his helpful suggestions and feedback during the refactoring process!
 

********************

FAQ's
------------------------

##### How do I use PushSharp in my ASP.NET Web Application?  #####
An ASP.NET application is NOT the ideal place to use PushSharp.  You'd be better off using a Windows Service, or some other infrastructure if at all possible.  The reason is that in an ASP.NET application, the Application Pool (AppPool) can be restarted on you and is usually not under your direct control, which means all the Queued notifications that PushSharp may be in the process of sending could be lost if PushSharp is not cleaned up gracefully.

If you MUST run PushSharp in an ASP.NET application, the best way is to create a ***singleton PushBroker instance*** in your Global.asax file.  You should keep this singleton instance around for the lifespan of your web application, including a call to pushBroker.StopAllServices() when your Application is ending (Application_End in global.asax).

You can help mitigate losing messages due to unforeseen App Pool terminations or restarts by persisting notifications you want to send in some other way, and only removing them from that persistent storage once the OnNotificationSent event has fired.  This is still not perfect (you may risk multiple notification deliveries), but it's probably adequate for most.

You should not be creating and destroying instances of PushBroker each time you send a notification, as this uses unnecessary resources and if you're using Apple APNS, they require you to keep the connection to their servers open as long as possible when sending notifications.  You should also call pushBroker.StopAllServices() in your Application_Ended event in your Global.asax.  Keep in mind that PushSharp works.


##### How do I support multiple applications with PushSharp? #####
**NOTE:** as of version 2.1.2 PushSharp now supports the concept of an arbitrary ApplicationId when you Start or Register push services, for example:

```csharp
//Specify your application id when registering the service
pushBroker.RegisterAppleService(channelSettings, "MY-APP-ID-HERE");

//Specify the application id for which the notification is intended when queueing
pushBroker.QueueNotification(notification, "MY-APP-ID"))
```

The alternative (old) way is to create an instance of PushBroker for each application you need.  



********************

MonoTouch and Mono for Android Client Application Integration
---------------------------------------------------------------------
Given that PushSharp is written in C#, you probably thought there was a good chance that it's being used somewhere with MonoTouch or Mono for Android... and you would be correct!  There are samples of how to setup the client app push notification end of things included in the PushSharp project source!  There's even a Windows Phone 7.5 project to show how to register for notifications!

All client samples can be found in the **/Client.Samples/** folder.  

PushSharp.ClientSample.MonoForAndroid
-----------------------------------------
There are two projects for Mono For Android:

1. PushSharp.ClientSample.C2dm
2. PushSharp.ClientSample.Gcm

C2DM is now deprecated by Google, and you can ignore it unless you are working with an application that already uses it.  Otherwise, focus on the *GCM* project.  

The GCM project also references the ***/PushSharp.Client/PushSharp.Client.MonoForAndroid.Gcm*** project which is a client library (ported from the java gcm-client library).  It is a helper library for managing GCM registrations on the client side, within your Android app.  You don't necessarily need to understand the code in this library, but just know that this Client Sample project references it for its own use!  

**VERY IMPORTANT NOTE:** Make sure your package name does not start with an uppercase character.  If it does, you will get cryptic errors in logcat and things will not work, because your package name is used in defining some permissions required by GCM. 
Eg: my.package.name is ok, but My.package.name is NOT

Next, take a look at the PushService.cs file in the sample project.  You can copy much of this class into your own App, but again be sure to substitute your own package name in where applicable (the BroadcastReceiver attributes need to be changed).  You will also need to change the SENDER_ID constant to your own (see the documentation for Configuring GCM with PushSharp in the wiki).  Finally, in this class, you will probably want to change what happens in some of the GCMIntentService methods.  In the OnRegistered, you would want to send the registration ID to your server, so that you can use it to send the device notifications.  You get the point.


PushSharp.ClientSample.MonoTouch
------------------------------------
Registering for remote notifications in MonoTouch is fairly trivial.  The only real tricky part is figuring out how to get the deviceToken into a nice string that you can send to your server.  Check out *AppDelegate.cs* for details on how this is done in MonoTouch!


****************	
 
License
-------
Apache PushSharp
Copyright 2012 The Apache Software Foundation

This product includes software developed at
The Apache Software Foundation (http://www.apache.org/).
