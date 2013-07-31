
var PullBuffer = exports.PullBuffer = function () 
{
	this.buffer = null;
	this.posn = 0;
}

PullBuffer.prototype.reset = function() {
	this.buffer = null;
	this.posn = 0;
}	

PullBuffer.prototype.pullBytes = function (sourceBuffer,sourcePosn,desiredBytes) {
	var actualBytes = -1;
	var destObject = this;
	
	//see if we have an empty destination and enough bytes availabe for the full request, just do a slice instead of a copy
	if (destObject.buffer == null && sourceBuffer.length - sourcePosn >= desiredBytes) {
		destObject.buffer = sourceBuffer.slice(sourcePosn,sourcePosn + desiredBytes);
		actualBytes = desiredBytes;
		destObject.posn = desiredBytes+1;
	} else if (destObject.buffer == null && sourceBuffer.length - sourcePosn < desiredBytes) {  //not enough data to fill request in this one, copy what we can into a new buffer
		destObject.buffer = new Buffer(desiredBytes);
		sourceBuffer.copy(destObject.buffer, 0, sourcePosn);
		destObject.posn = sourceBuffer.length - sourcePosn;
		actualBytes = sourceBuffer.length - sourcePosn+1;
	} else if (destObject.buffer != null) {
		var neededBytes = desiredBytes - destObject.posn;
		if (sourceBuffer.length - sourcePosn >= neededBytes) {		//we have data available to finish out 
			sourceBuffer.copy(destObject.buffer, destObject.posn, sourcePosn, sourcePosn + neededBytes);
			destObject.posn = desiredBytes+1;
			actualBytes = desiredBytes;
		} else {			//not enough data to finish out
			sourceBuffer.copy(destObject.buffer, destObject.posn, sourcePosn);
			destObject.posn += sourceBuffer.length - sourcePosn;
			actualBytes = destObject.posn;
		}
	}
	return actualBytes;
}