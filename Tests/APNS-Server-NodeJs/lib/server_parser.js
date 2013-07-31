var HEADER_BUFFER_SIZE = 1 + 4 + 4 + 2 + 32 + 2;
var events = require('events');
var sys = require('sys');
var util = require('util');
var winston = require('winston');
var pullbuffer = require('./pullbuffer');

var ServerParser = exports.ServerParser = function(socket) {
	events.EventEmitter.call(this);
		
	this.socket = socket;
	this.reset();
};
sys.inherits(ServerParser, events.EventEmitter);

ServerParser.prototype.reset = function() {
	if (!this.header)
		this.header = new pullbuffer.PullBuffer();
	else
		this.header.reset();
		
	this.parseHeader = true;
	this.parsedHeader = {
		identifier : 0,
		deviceToken : '',
		payloadLength : 0
	}
	
	if (!this.payload)
		this.payload = new pullbuffer.PullBuffer();
	else
		this.payload.reset();	
}

ServerParser.prototype.raiseError = function(errorCode,errorIdentifier) {
	errorBuffer = new Buffer(6);
	errorBuffer[0]=8;
	errorBuffer[1]=errorCode
	errorBuffer.writeInt32BE(errorIdentifier,2);
	this.socket.end(errorBuffer);
    this.reset();
}

ServerParser.prototype.getHeaderValues = function (header,parsedHeader) {
	var result=false;
	
	parsedHeader.identifier = header.buffer.readInt32BE(1);
	if (header.buffer[0]==1) {  //make sure it is an enhanced mode header
		parsedHeader.deviceToken = header.buffer.toString('hex',11,43);
		parsedHeader.payloadLength = header.buffer.readInt16BE(43);
		if (parsedHeader.payloadLength>256 || parsedHeader.payloadLength <=0) {
			if (parsedHeader.payloadLength == 0)
				this.raiseError(4,parsedHeader.identifier);
			else
				this.raiseError(7,parsedHeader.identifier);
		} else {
			result = true;
		}
	} else {
		this.raiseError(1,parsedHeader.identifier);
	}
	return result;
}

ServerParser.prototype.parseIncomingAPNS = function (buffer) {
	winston.verbose('Server parser got: '+buffer);
	
	//try
	//{
		var bufferPosn = 0
		var actualBytes = 0
		
		while(bufferPosn < buffer.length)
		{
			if (this.parseHeader)
			{
				actualBytes = this.header.pullBytes(buffer,bufferPosn,HEADER_BUFFER_SIZE);
				bufferPosn += actualBytes;
				if (actualBytes==HEADER_BUFFER_SIZE) {
					this.parseHeader = false;
					if (!this.getHeaderValues(this.header,this.parsedHeader)) {		//if something wrong with headers, quit processing
						return;
					}
					this.payload.reset();
				}
			}
		
			if (!this.parseHeader) {
				actualBytes = this.payload.pullBytes(buffer,bufferPosn,this.parsedHeader.payloadLength);
				bufferPosn += actualBytes;
				if (actualBytes==this.parsedHeader.payloadLength) {
					var payload = this.payload.buffer.toString('utf8');
					var pushEvent = {
						identifier: this.parsedHeader.identifier,
						deviceToken: this.parsedHeader.deviceToken,
						payload: payload};
					this.emit('push', pushEvent);
					this.reset();
				} 
			}
		}
	//}
	//catch (e)
	//{
    //    winston.error(e);
	//	this.reset();
	//}

};

module.exports = ServerParser;