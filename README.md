PushSharp v3.0
==============

PushSharp is a server-side library for sending Push Notifications to iOS/OSX (APNS), Android/Chrome (GCM), Windows/Windows Phone, Amazon (ADM) and Blackberry devices!

PushSharp v3.0 is a complete rewrite of the original library, aimed at taking advantage of things like async/await, HttpClient, and generally a better infrastructure using lessons learned from the old code.

[![Join the chat at https://gitter.im/Redth/PushSharp](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/Redth/PushSharp?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

[![AppVeyor CI Status](https://ci.appveyor.com/api/projects/status/github/Redth/PushSharp?branch=master&svg=true)](https://ci.appveyor.com/project/Redth/pushsharp)

[![NuGet Version](https://badge.fury.io/nu/PushSharp.svg)](https://badge.fury.io/nu/PushSharp)

 - Read more on my blog http://redth.codes/pushsharp-3-0-the-push-awakens/
 - Join the [Gitter.im channel](https://gitter.im/Redth/PushSharp) with questions/feedback

---

## Sample Usage

The API in v3.x series is quite different from 2.x.  The goal is to simplify things and focus on the core functionality of the library, leaving things like constructing valid payloads up to the developer.

Here is an example of how you would send an APNS notification:
```csharp
// Configuration
var config = new ApnsConfiguration ("push-cert.pfx", "push-cert-pwd");

// Create a new broker
var broker = new ApnsServiceBroker (config);
    
// Wire up events
broker.OnNotificationFailed += (notification, aggregateEx) => {

	aggregateEx.Handle (ex => {
	
		// See what kind of exception it was to further diagnose
		if (ex is ApnsNotificationException) {
			var apnsEx = ex as ApnsNotificationException;

			// Deal with the failed notification
			var n = apnsEx.Notification;
		
			Console.WriteLine ($"Notification Failed: ID={n.Identifier}, Code={apnsEx.ErrorStatusCode}");
	
		} else if (ex is ApnsConnectionException) {
			// Something failed while connecting (maybe bad cert?)
			Console.WriteLine ("Notification Failed (Bad APNS Connection)!");
		} else {
			Console.WriteLine ("Notification Failed (Unknown Reason)!");
		}

		// Mark it as handled
		return true;
	});
};

broker.OnNotificationSucceeded += (notification) => {
	Console.WriteLine ("Notification Sent!");
};

// Start the broker
broker.Start ();

// Queue a notification to send
broker.QueueNotification (new ApnsNotification {
		DeviceToken = "device-token-from-device",
		Payload = JObject.Parse ("{\"aps\":{\"badge\":7}}")
	});
   
// Stop the broker, wait for it to finish   
// This isn't done after every message, but after you're
// done with the broker
broker.Stop ();
```

Other platforms are structured the same way, although the configuration of each broker may vary.



## How to Migrate from PushSharp 2.x to 3.x

Please see this Wiki page for more information: https://github.com/Redth/PushSharp/wiki/Migrating-from-PushSharp-2.x-to-3.x


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
