
var Actor = function(uid,oid) {
	this.uid = uid;
	this.oid = oid;
	this.tx = 0;
	this.ty = 0;
	this.sprite = " ";
	this.hp = -1;
};

Actor.prototype.mod = function(oid,tx,ty,rx,ry,sprite,hp) {
	var majorDirty = false;
	this.oid = oid;
	if( this.tx != tx || this.ty != ty )
	{
		majorDirty = true;
	}
	this.tx = tx;
	this.ty = ty;
	this.rx = rx;
	this.ry = ry;
	this.sprite = sprite;
	this.hp = hp;
	return majorDirty;
};

Actor.prototype.modhp = function(hp) {
	this.hp = hp;
};

Actor.prototype.readChunks = function(chunks) {
	this.tx = chunks[0];
	this.ty = chunks[1];
	this.sprite = chunks[2];
	this.hp = chunks[3];
};

Actor.prototype.writeChunks = function() {
	var chunks = '';
	chunks += this.uid;
	chunks += '|';
	chunks += this.oid;
	chunks += '|';
	chunks += this.tx;
	chunks += '|';
	chunks += this.ty;
	chunks += '|';
	chunks += this.sprite;
	chunks += '|';
	chunks += this.hp;
	return chunks;
};

module.exports = Actor;
