var net     = require("net");
var http = require("http");
var util	= require('util');
var url = require('url');
var serverparser = require('./lib/server_parser');
var winston = require('winston');
var sys = require('sys');

var pushRx = 0;
var pushRxDiscarded = 0;
var successIds = [];
var failedIds = [];


var ts = function() { 
		var ts = new Date(); 
		return ts.getMonth()+1+'-'+ts.getDate()+'-'+ts.getFullYear()+' '+ts.toLocaleTimeString() };


var failIds = [];

winston.remove(winston.transports.Console)
	.add(winston.transports.Console, { level: 'info', colorize: false, timestamp: ts, handleExceptions: true })
	//.add(winston.transports.File, { level: 'debug', filename: 'error.log', maxsize:5000000, maxFiles:10, json:false, timestamp: ts, handleExceptions: true })

//spinup the server to accept incoming push requests
var pushServer = net.createServer(function(sock) {

    var isClosing = false;

	winston.info('server connected: '+sock.remoteAddress+':'+sock.remotePort);

    sock.serverParser = new serverparser(sock);
	sock.serverParser.on('push', function(push) {

        pushRx++;

        //var pid = 0;
        var pid = push.identifier;
        var success = true;

        //See if our id is in the fail list
        for (var i = 0; i < failIds.length; i++)
        {
            if (failIds[i] == pid)
            {
                failedIds.push(pid);
                success = false;
                isClosing = true;
                winston.info('! FAILING id: ' + pid + ', deviceToken: ' + push.deviceToken);
                sock.serverParser.raiseError(4, pid);
            }
        }

        if (!isClosing && success)
        {
		    successIds.push(pid);
            winston.info('+ SUCCESS id: ' + pid + ', deviceToken: ' + push.deviceToken);
        }
        else if (isClosing && success)
        {
            pushRxDiscarded++;
            winston.info('~ DISCARD id: ' + pid + ', deviceToken: ' + push.deviceToken);
        }
	});
	sock.on('data', function(data) {
        if (!isClosing)
		    sock.serverParser.parseIncomingAPNS(data);
	});
	sock.on('close',function() {
		winston.info('server disconnected');
        isClosing = false;
	});
	sock.on('error',function(err) {
		winston.error('error on server socket: '+err);
		sock.destroy();
        isClosing = false;
	});
});

pushServer.listen(2195, function() {
	winston.info('APNS Server is listening');
	}
);


var httpServer = http.createServer(function(request, response) {

    var urlParts = url.parse(request.url, true);

    if (urlParts.pathname === '/setup')
    {
        failIds = [];
        if (urlParts.query && urlParts.query.failId)
        {
            for (var i = 0; i < urlParts.query.failId.length; i++) {
                failIds.push(urlParts.query.failId[i]);
            }
        }

        winston.info('setup: failIds: ' + JSON.stringify(failIds));

        var info = {};
        info.failIds = failIds;
        info.status = "OK";

        response.writeHead(200, {"Content-Type": "application/javascript"});
        response.write(JSON.stringify(info));
        response.end();
    }
    else if (urlParts.pathname === '/info')
    {
        winston.info('Rx: ' + pushRx + ' (Discarded: ' + pushRxDiscarded +'), Success: ' + successIds.length + ', Failed: ' + failedIds.length);

        var info = {};

        info.status = "OK";
        info.received = pushRx;
        info.successIds = successIds;
        info.failedIds = failedIds;
        info.discarded = pushRxDiscarded;

        response.writeHead(200, {"Content-Type": "application/javascript"});
        response.write(JSON.stringify(info));
        response.end();
    }
    else if (urlParts.pathname === '/reset')
    {
        winston.info('Reset Stats');

        pushRx = 0;
        pushRxDiscarded = 0;
        successIds = [];
        failedIds = [];

        var info = {};
        info.status = "OK";

        response.writeHead(200, {"Content-Type": "application/javascript"});
        response.write(JSON.stringify(info));
        response.end();
    }

});
httpServer.listen(8888, function() {
    winston.info("HTTP Server is listening");
});




var stdin = process.openStdin();



stdin.addListener("data", function(d) {
    // note:  d is an object, and when converted to a string it will
    // end with a linefeed.  so we (rather crudely) account for that  
    // with toString() and then substring() 
    var  line = d.toString().substring(0, d.length-1);

    if (line === 'stat')
    {
    	winston.info('Rx: ' + pushRx + ' (Discarded: ' + pushRxDiscarded +'), Success: ' + pushRxSuccess + ', Failed: ' + pushRxFailed);
    }

    if (line === 'reset')
    {
    	pushRx = 0;
    	pushRxDiscarded = 0;
        successIds = [];
        failedIds = [];
    }
    
  });

winston.info('Fin. starting');