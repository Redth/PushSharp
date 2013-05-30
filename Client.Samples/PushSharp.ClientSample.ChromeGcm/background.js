chrome.app.runtime.onLaunched.addListener(function() {

  console.log("Hello World");

  chrome.app.window.create('window.html', {
    'bounds': {
      'width': 400,
      'height': 500
    }
  });
  chrome.pushMessaging.getChannelId(true, function(channelId) {
  	console.log("CHANNEL ID: " + channelId.channelId);
  	//chrome.extension.getBackgroundPage().console.log('foo');
  });
  chrome.pushMessaging.onMessage.addListener(function(message) {
  	console.log("SubChannelID: " + message.SubChannelID);
  	console.log("Payload: " + message.Payload);
  });
});