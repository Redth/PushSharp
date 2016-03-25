PushSharp v4.0
==============

PushSharp is a server-side library for sending Push Notifications to iOS/OSX (APNS), Android/Chrome (GCM), Windows/Windows Phone, Amazon (ADM) and Blackberry devices!

PushSharp v3.0+ is a complete rewrite of the original library, aimed at taking advantage of things like async/await, HttpClient, and generally a better infrastructure using lessons learned from the old code.

PushSharp will now follow [semver](http://semver.org/) versioning, so major version numbers will go up as there are any breaking api changes. 

[![Join the chat at https://gitter.im/Redth/PushSharp](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/Redth/PushSharp?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

[![AppVeyor CI Status](https://ci.appveyor.com/api/projects/status/github/Redth/PushSharp?branch=master&svg=true)](https://ci.appveyor.com/project/Redth/pushsharp)

[![NuGet Version](https://badge.fury.io/nu/PushSharp.svg)](https://badge.fury.io/nu/PushSharp)

 - Read more on my blog http://redth.codes/pushsharp-3-0-the-push-awakens/
 - Join the [Gitter.im channel](https://gitter.im/Redth/PushSharp) with questions/feedback

---

## Sample Usage

The API in v3.x+ series is quite different from 2.x.  The goal is to simplify things and focus on the core functionality of the library, leaving things like constructing valid payloads up to the developer.

### APNS Sample Usage
Here is an example of how you would send an APNS notification:

```csharp
// Configuration (NOTE: .pfx can also be used here)
var config = new ApnsConfiguration (ApnsConfiguration.ApnsServerEnvironment.Sandbox, 
    "push-cert.p12", "push-cert-pwd");

// Create a new broker
var apnsBroker = new ApnsServiceBroker (config);
    
// Wire up events
apnsBroker.OnNotificationFailed += (notification, aggregateEx) => {

	aggregateEx.Handle (ex => {
	
		// See what kind of exception it was to further diagnose
		if (ex is ApnsNotificationException) {
			var notificationException = (ApnsNotificationException)ex;
			
			// Deal with the failed notification
			var apnsNotification = notificationException.Notification;
			var statusCode = notificationException.ErrorStatusCode;

			Console.WriteLine ($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");
	
		} else {
			// Inner exception might hold more useful information like an ApnsConnectionException			
			Console.WriteLine ($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
		}

		// Mark it as handled
		return true;
	});
};

apnsBroker.OnNotificationSucceeded += (notification) => {
	Console.WriteLine ("Apple Notification Sent!");
};

// Start the broker
apnsBroker.Start ();

foreach (var deviceToken in MY_DEVICE_TOKENS) {
	// Queue a notification to send
	apnsBroker.QueueNotification (new ApnsNotification {
		DeviceToken = deviceToken,
		Payload = JObject.Parse ("{\"aps\":{\"badge\":7}}")
	});
}
   
// Stop the broker, wait for it to finish   
// This isn't done after every message, but after you're
// done with the broker
apnsBroker.Stop ();
```

#### Apple Notification Payload

More information about the payload sent in the ApnsNotification object can be found [here](https://developer.apple.com/library/ios/documentation/NetworkingInternet/Conceptual/RemoteNotificationsPG/Chapters/TheNotificationPayload.html).


#### Apple APNS Feedback Service

For APNS you will also need to occasionally check with the feedback service to see if there are any expired device tokens you should no longer send notifications to.  Here's an example of how you would do that:

```csharp
var config = new ApnsConfiguration (
    ApnsConfiguration.ApnsServerEnvironment.Sandbox, 
    Settings.Instance.ApnsCertificateFile, 
    Settings.Instance.ApnsCertificatePassword);

var fbs = new FeedbackService (config);
fbs.FeedbackReceived += (string deviceToken, DateTime timestamp) => {
    // Remove the deviceToken from your database
    // timestamp is the time the token was reported as expired
};
fbs.Check ();
```


### GCM Sample Usage

Here is how you would send a GCM Notification:

```csharp
// Configuration
var config = new GcmConfiguration ("GCM-SENDER-ID", "AUTH-TOKEN", null);

// Create a new broker
var gcmBroker = new GcmServiceBroker (config);
    
// Wire up events
gcmBroker.OnNotificationFailed += (notification, aggregateEx) => {

	aggregateEx.Handle (ex => {
	
		// See what kind of exception it was to further diagnose
		if (ex is GcmNotificationException) {
			var notificationException = (GcmNotificationException)ex;
			
			// Deal with the failed notification
			var gcmNotification = notificationException.Notification;
			var description = notificationException.Description;

			Console.WriteLine ($"GCM Notification Failed: ID={gcmNotification.MessageId}, Desc={description}");
		} else if (ex is GcmMulticastResultException) {
			var multicastException = (GcmMulticastResultException)ex;

			foreach (var succeededNotification in multicastException.Succeeded) {
				Console.WriteLine ($"GCM Notification Failed: ID={succeededNotification.MessageId}");
			}

			foreach (var failedKvp in multicastException.Failed) {
				var n = failedKvp.Key;
				var e = failedKvp.Value;

				Console.WriteLine ($"GCM Notification Failed: ID={n.MessageId}, Desc={e.Description}");
			}

		} else if (ex is DeviceSubscriptionExpiredException) {
			var expiredException = (DeviceSubscriptionExpiredException)ex;
			
			var oldId = expiredException.OldSubscriptionId;
			var newId = expiredException.NewSubscriptionId;

			Console.WriteLine ($"Device RegistrationId Expired: {oldId}");

			if (!string.IsNullOrWhitespace (newId)) {
				// If this value isn't null, our subscription changed and we should update our database
				Console.WriteLine ($"Device RegistrationId Changed To: {newId}");
			}
		} else if (ex is RetryAfterException) {
			var retryException = (RetryAfterException)ex;
			// If you get rate limited, you should stop sending messages until after the RetryAfterUtc date
			Console.WriteLine ($"GCM Rate Limited, don't send more until after {retryException.RetryAfterUtc}");
		} else {
			Console.WriteLine ("GCM Notification Failed for some unknown reason");
		}

		// Mark it as handled
		return true;
	});
};

gcmBroker.OnNotificationSucceeded += (notification) => {
	Console.WriteLine ("GCM Notification Sent!");
};

// Start the broker
gcmBroker.Start ();

foreach (var regId in MY_REGISTRATION_IDS) {
	// Queue a notification to send
	gcmBroker.QueueNotification (new GcmNotification {
		RegistrationIds = new List<string> { 
			regId
		},
		Data = JObject.Parse ("{ \"somekey\" : \"somevalue\" }")
	});
}
   
// Stop the broker, wait for it to finish   
// This isn't done after every message, but after you're
// done with the broker
gcmBroker.Stop ();
```

#### Components of a GCM Notification

GCM notifications are much more customizable than Apple Push Notifications. More information about the messaging concepts and options can be found [here](https://developers.google.com/cloud-messaging/concept-options#components-of-a-message).


### WNS Sample Usage

Here's how to send WNS Notifications:

```csharp
// Configuration
var config = new WnsConfiguration ("WNS_PACKAGE_NAME", "WNS_PACKAGE_SID", "WNS_CLIENT_SECRET");

// Create a new broker
var wnsBroker = new WnsServiceBroker (config);

// Wire up events
wnsBroker.OnNotificationFailed += (notification, aggregateEx) => {

	aggregateEx.Handle (ex => {
	
		// See what kind of exception it was to further diagnose
		if (ex is WnsNotificationException) {
			var notificationException = (WnsNotificationException)ex;
			Console.WriteLine ($"WNS Notification Failed: {notificationException.Message}");
		} else {
			Console.WriteLine ("WNS Notification Failed for some (Unknown Reason)");
		}

		// Mark it as handled
		return true;
	});
};

wnsBroker.OnNotificationSucceeded += (notification) => {
	Console.WriteLine ("WNS Notification Sent!");
};

// Start the broker
wnsBroker.Start ();

foreach (var uri in MY_DEVICE_CHANNEL_URIS) {
	// Queue a notification to send
	wnsBroker.QueueNotification (new WnsToastNotification {
		ChannelUri = uri,
		Payload = XElement.Parse (@"
			<toast>
				<visual>
					<binding template=""ToastText01"">
						<text id=""1"">WNS_Send_Single</text>
					</binding>  
				</visual>
			</toast>")
	});
}

// Stop the broker, wait for it to finish   
// This isn't done after every message, but after you're
// done with the broker
wnsBroker.Stop ();
```


## How to Migrate from PushSharp 2.x to 3.x and higher

Please see this Wiki page for more information: https://github.com/Redth/PushSharp/wiki/Migrating-from-PushSharp-2.x-to-3.x-


## Roadmap

 - **APNS - Apple Push Notification Service** 
   - Finish HTTP/2 support (currently in another branch)
 - **GCM - Google Cloud Messaging** 
   - XMPP transport still under development
 - **Other**
   - More NUnit tests to be written, with a test GCM Server, and eventually Test servers for other platforms
   - New Xamarin Client samples (how to setup each platform as a push client) will be built and live in a separate repository to be less confusing
   

License
-------
Copyright 2012-2016 Jonathan Dick

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
