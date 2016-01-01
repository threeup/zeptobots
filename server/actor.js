
var Actor = function(uid,oid) {
	this.uid = uid;
	this.oid = oid;
	this.team = -1;
	this.tx = 0;
	this.ty = 0;
	this.rx = 0;
	this.ry = 0;
	this.fx = 0;
	this.fy = 0;
	this.sprite = " ";
	this.hp = -1;
	this.speed = -1;
	this.damage = -1;
	this.ttl = -1;
};

Actor.prototype.erase = function() {
	this.oid = -1;
	this.team = -1;
	this.hp = -1;
};

Actor.prototype.loadDef = function() {
	var actorType = this.sprite.charAt(0);
	switch(actorType)
	{
		case 'H':
			this.hp = 10;
			this.speedlimit = 30;
			this.dmg = 1;
			this.ttl = 1000;
			break;
		case '*':
			this.hp = 1;
			this.speedlimit = 100;
			this.dmg = 10;
			this.ttl = 3;
			break;
		default:
			this.hp = 2;
			this.speedlimit = 30;
			this.dmg = 1;
			this.ttl = 1000;
			break;
	}
	
}


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
	var damage = chunks.shift();
	var ttl = chunks.shift();
	var fx = chunks.shift();
	var fy = chunks.shift();

	var posDirty = false;
	if( this.rx != rx || this.ry != ry || this.fx != fx || this.fy != fy )
	{
		posDirty = true;
	}
	this.tx = tx;
	this.ty = ty;
	this.rx = rx;
	this.ry = ry;
	this.fx = fx;
	this.fy = fy;

	var stateDirty = false;
	if( this.oid != oid || 
		this.team != team || 
		this.sprite != sprite || 
		this.hp != hp || 
		this.speed != speed ||
		this.damage != damage ||
		this.ttl != ttl)
	{
		stateDirty = true;
	}
	this.oid = oid;
	this.team = team;
	this.sprite = sprite;
	this.hp = hp;
	this.speed = speed;
	this.damage = damage;
	this.ttl = ttl;
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
	chunks += this.fx;
	chunks += '|';
	chunks += this.fy;
	chunks += '|';
	chunks += this.sprite;
	chunks += '|';
	chunks += this.hp;
	chunks += '|';
	chunks += this.speed;
	chunks += '|';
	chunks += this.damage;
	chunks += '|';
	chunks += this.ttl;

	return chunks;
};

module.exports = Actor;
