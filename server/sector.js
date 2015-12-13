
var Sector = function(sx,sy) {
	
	this.contents = "";
	this.contents += "WWWWFFWWWW,";
	var modxy = (sx+sy) % 4;
	if( modxy == 0 )
	{
		this.contents += "WFSFFFFFFW,";
		this.contents += "WFFFSFFFFW,";
		this.contents += "WFFWWWFFFW,";
		this.contents += "FFFFFFFFFF,";
		this.contents += "FFFFFFFFFF,";
		this.contents += "WFFFWWWFFW,";
		this.contents += "WFFFFSFFFW,";
		this.contents += "WFFFFFFSFW,";
	}
	else
	{
		this.contents += "WFTTFFFFFW,";
		this.contents += "WFTTFWWFFW,";
		this.contents += "WFFFFFWFFW,";
		if( modxy == 3)
		{
			this.contents += "FFFFFFFFFF,";
			this.contents += "FFFFFFFFFF,";
		}
		else
		{
			this.contents += "FTWFWFFFFF,";
			this.contents += "FFFFFWFWTF,";
		}
		this.contents += "WFFWFFFFFW,";
		this.contents += "WFFWWFTTFW,";
		this.contents += "WFFFFFTTFW,";
	}
	this.contents += "WWWWFFWWWW,";	

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
