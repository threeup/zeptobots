
var Sector = function(sx,sy) {
	this.contents = "WWWWFFWWWW,WFFFSFFFFW,WFFFFFFFFW,WFFFFFFFFW,FFFFFFFFFF,FFFWFWFWFF,WWWFFFFFFW,WFFFFFFFFW,WFFFFFFFFW,WWWWFFWWWW";
	this.sx = sx;
	this.sy = sy;
};

Sector.prototype.getGround = function(sx,sy) {
	return this.contents.charAt(sy*10+sx);
};

Sector.prototype.writeChunks = function() {
	var chunks = '';
	chunks += this.sx;
	chunks += '|';
	chunks += this.sy;
	chunks += '|';
	chunks += this.contents;
	return chunks;
}

module.exports = Sector;
