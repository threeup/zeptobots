
var Actor = function(uid,oid) {
	this.uid = uid;
	this.oid = oid;
	this.team = -1;
	this.tx = 0;
	this.ty = 0;
	this.rx = 0;
	this.ry = 0;
	this.sprite = " ";
	this.hp = -1;
	this.speed = 30;
};

Actor.prototype.erase = function() {
	this.oid = -1;
	this.team = -1;
	this.hp = -1;
};


Actor.prototype.readChunks = function(chunks) {
	
	var oid = chunks.shift();
	var team = chunks.shift();
	var tx = chunks.shift();
	var ty = chunks.shift();
	var rx = chunks.shift();
	var ry = chunks.shift();
	var sprite = chunks.shift();
	var hp = chunks.shift();
	var speed = chunks.shift();

	var posDirty = false;
	if( this.rx != rx || this.ry != ry )
	{
		posDirty = true;
	}
	this.tx = tx;
	this.ty = ty;
	this.rx = rx;
	this.ry = ry;

	var stateDirty = false;
	if( this.oid != oid || this.team != team || this.sprite != sprite || this.hp != hp || this.speed != speed)
	{
		stateDirty = true;
	}
	this.oid = oid;
	this.team = team;
	this.sprite = sprite;
	this.hp = hp;
	this.speed = speed;
	return posDirty || stateDirty;
};

Actor.prototype.writeChunks = function() {
	var chunks = '';
	chunks += this.uid;
	chunks += '|';
	chunks += this.oid;
	chunks += '|';
	chunks += this.team;
	chunks += '|';
	chunks += this.tx;
	chunks += '|';
	chunks += this.ty;
	chunks += '|';
	chunks += this.rx;
	chunks += '|';
	chunks += this.ry;
	chunks += '|';
	chunks += this.sprite;
	chunks += '|';
	chunks += this.hp;
	chunks += '|';
	chunks += this.speed;
	return chunks;
};

module.exports = Actor;
