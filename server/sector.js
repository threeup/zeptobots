
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
	this.active = false;
	this.sx = sx;
	this.sy = sy;
};

Sector.prototype.replaceAt=function(string, index, character) {
    return string.substr(0, index) + character + string.substr(index+character.length);
}

Sector.prototype.getGround = function(sx,sy) {
	//11 for commas
	return this.contents.charAt(sy*11+sx);
};

Sector.prototype.setGround = function(sx,sy,c) {
	//11 for commas
	this.contents = this.replaceAt(this.contents, sy*11+sx, c);
};

Sector.prototype.writeChunks = function() {
	var chunks = '';
	chunks += this.sx;
	chunks += '|';
	chunks += this.sy;
	chunks += '|';
	if( this.active )
	{
		chunks += this.contents;
	}
	return chunks;
}

module.exports = Sector;
