chrome.app.runtime.onLaunched.addListener(function() {

  console.log("Hello World");


  chrome.app.window.create('window.html', {
    'bounds': {
      'width': 600,
      'height': 500
    }
  });
  chrome.pushMessaging.getChannelId(true, function(msg) {
  	console.log("CHANNEL ID: " + msg.channelId);
  	
    chrome.runtime.sendMessage({method:"pushRegistered", channelId: msg.channelId},function(response){ });
  });

  chrome.pushMessaging.onMessage.addListener(function(msg){
    console.log("PUSH...");
    console.log(msg.subchannelId + " : " + msg.payload);
    chrome.runtime.sendMessage({method:"pushNotification", subchannelId: msg.subchannelId, payload: msg.payload}, function(response){});

    var notification = window.webkitNotifications.createNotification('', 'PushSharp', msg.payload + " [" + msg.subchannelId + "]");
    notification.show();
  });
 
 
  
 
});