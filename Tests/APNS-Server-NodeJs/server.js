var net     = require("net");
var util	= require('util');
var serverparser = require('./lib/server_parser');
var winston = require('winston');
var sys = require('sys');

var pushRx = 0;
var pushRxDiscarded = 0;
var pushRxSuccess = 0;
var pushRxFailed = 0;


var ts = function() { 
		var ts = new Date(); 
		return ts.getMonth()+1+'-'+ts.getDate()+'-'+ts.getFullYear()+' '+ts.toLocaleTimeString() };
		
winston.remove(winston.transports.Console)
	.add(winston.transports.Console, { level: 'info', colorize: false, timestamp: ts, handleExceptions: true })
	//.add(winston.transports.File, { level: 'debug', filename: 'error.log', maxsize:5000000, maxFiles:10, json:false, timestamp: ts, handleExceptions: true })

//spinup the server to accept incoming push requests
var pushServer = net.createServer(function(sock) {

    var isClosing = false;

	winston.info('server connected: '+sock.remoteAddress+':'+sock.remotePort);

    sock.serverParser = new serverparser(sock);
	sock.serverParser.on('push', function(push) {

        var pid = 0;
        pid = push.identifier;

        if (pid == 5000 || pid == 15000 || pid == 25000 || pid == 35000 || pid == 45000 || pid == 55000 || pid == 65000 || pid == 75000 || pid == 85000 || pid == 95000)
        {
            isClosing = true;
            success = false;
            winston.info('FAILING PUSH: ' + pid);
            sock.serverParser.raiseError(4, pid);
        }

        if (isClosing)
		    winston.info('! id: ' + pid + ', deviceToken: ' + push.deviceToken);
        //else
        //    winston.info('+ id: ' + pid + ', deviceToken: ' + push.deviceToken);

		var success = true;


        if (!isClosing)
        {
            pushRx++;
		    if (success)
			    pushRxSuccess++;
		    else
		    	pushRxFailed++;
        }
        else
        {
            pushRxDiscarded++;
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
	winston.info('server listener running');
	}
);
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
    	pushRxFailed = 0;
    	pushRxSuccess = 0;
        pushRxDiscarded = 0;
    }
    
  });

winston.info('Server running');