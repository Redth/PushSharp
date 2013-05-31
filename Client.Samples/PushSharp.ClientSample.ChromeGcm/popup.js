$(function() {

	
		chrome.runtime.onMessage.addListener(function(message,sender,sendRepsonse){
  			if(message.method == "pushRegistered")
  				$('#channelId').text(message.channelId);
    	
    		if(message.method == "pushNotification")
    			$('#messages').html($('#messages').html() + '<strong>SubChannelID:</strong> ' 
    				+ message.subchannelId + '<br /><strong>Payload:</strong> ' + message.payload + '<br /><br />');
		});
	
	
});
