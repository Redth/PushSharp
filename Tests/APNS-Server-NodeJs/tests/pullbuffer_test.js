var pullbuffer = require('../lib/pullbuffer');
var assert=require('assert');

function testAllAvailable() {
	sourceBuffer = new Buffer('abcdefghijklmnopqrstuvwxyz');
	testpull = new pullbuffer.PullBuffer();
	var desired = testpull.pullBytes(sourceBuffer,0,5);
	assert.equal(desired,5);
	assert.equal(testpull.buffer.toString(),'abcde');
	
	//test where we aren't starting at the beginning
	testpull.reset();
	desired=testpull.pullBytes(sourceBuffer,2,5);
	assert.equal(desired,5);
	assert.equal(testpull.buffer.toString(),'cdefg');
}

function testPartialAvailable() {
	sourcebuffer1 = new Buffer('abc');
	sourcebuffer2 = new Buffer('defghijklm');
	
	var testpull = new pullbuffer.PullBuffer();
	var desired = testpull.pullBytes(sourcebuffer1,0,8);
	assert.equal(desired,4);
	assert.equal(testpull.buffer.toString('utf-8',0,3),'abc');
	
	//simulate that we got the "next" block of data
	desired = testpull.pullBytes(sourcebuffer2,0,8);
	assert.equal(desired,8);
	assert.equal(testpull.buffer.toString(),'abcdefgh');
	
	//test not at beginning
	testpull.reset();
	desired = testpull.pullBytes(sourcebuffer1,1,8);
	assert.equal(desired,3);
	assert.equal(testpull.buffer.toString('utf-8',0,2),'bc');
	
	//next block of data
	desired=testpull.pullBytes(sourcebuffer2,0,8);
	assert.equal(desired,8);
	assert.equal(testpull.buffer.toString(),'bcdefghi');
	
	//test not at beginning, read entire buffer
	testpull.reset();
	desired = testpull.pullBytes(sourcebuffer1,0,14);
	assert.equal(desired,4);
	assert.equal(testpull.buffer.toString('utf-8',0,3),'abc');
	
	desired = testpull.pullBytes(sourcebuffer2,0,14);
	assert.equal(desired,13);
	assert.equal(testpull.buffer.toString('utf-8',0,13),'abcdefghijklm');
	
	sourcebuffer3 = new Buffer('nopqr');
	desired = testpull.pullBytes(sourcebuffer3,2,14);
	assert.equal(desired,14);
	assert.equal(testpull.buffer.toString('utf-8'),'abcdefghijklmp');
	
}

testAllAvailable();
testPartialAvailable();