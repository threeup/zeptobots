var fs = require('fs');
var Actor = require('./actor');
var Sector = require('./sector');
var savedGroundPath = './saved-ground.txt';
var savedActorPath = './saved-actor.txt';

var World = {

	sectors: [],
	actors: [],
	dirtySectors: [],
	dirtyActors: [],
	minorDirtyActors: [],
	nextUID: 1000,

	initSector: function() {
		for(var sy = 0; sy<10; ++sy)
		{
			for(var sx = 0; sx<10; ++sx)
			{
				this.sectors.push(new Sector(sx,sy));
			}	
		}

		for( var q = 0; q<1000; ++q)
		{
			var uid = q+1000;
			this.actors.push(new Actor(uid,-1));
		}
	},

	getSector: function(sx,sy) {
		return this.sectors[sy*10+sx];
	},

	activateSector: function(radius) {
		var self = this;
		var startX = 5-radius;
		var endX = 5+radius;
		var startY = 5-radius;
		var endY = 5+radius;
		for( var sy = startY; sy<= endY; ++sy)
		{
			for( var sx = startX; sx<= endX; ++sx)
			{
				self.getSector(sx, sy).active = true;
			}	
		}
	},


	loadGround: function(callback) {
		var self = this;
		fs.exists(savedGroundPath, function(exists) {
		    if (!exists) { callback(0); return; }
	        fs.readFile(savedGroundPath, function(err, body) {
				if (err) throw err;
				body.toString().split('\n').forEach(function(line) {
					var chunks = line.toString().split('|');
					if( chunks.length > 2 )
					{
						var sx = chunks.shift();
						var sy = chunks.shift();
						var contents = chunks.shift();
						var sec = self.getSector(sx,sy);
						if( sec )
						{
							sec.contents = contents;
						}
					}
				});
				callback(100);
			});
		});
		
	},

	loadActor: function(callback) {
		var self = this;
		var count = 0;
		fs.exists(savedActorPath, function(exists) {
		    
		    if (!exists) { callback(0); return; }

			fs.readFile(savedActorPath, function(err, body) {
				if (err) throw err;
				body.toString().split('\n').forEach(function(line) {
					var chunks = line.toString().split('|');
					if( chunks.length > 3)
					{
						var uid = chunks.shift();
						var actor = self.actors[uid-1000];
						actor.readChunks(chunks);
						count++;
					}
				});
				

				callback(count);
			});
		});
	},

	saveGround: function() {
		var self = this;
		var groundData = '';
		for(var sy = 0; sy<10; ++sy)
		{
			for(var sx = 0; sx<10; ++sx)
			{
				groundData += self.sectors[sy*10+sx].writeChunks();
				groundData += '\n';
			}
		}
		fs.writeFile(savedGroundPath, groundData, function(err) {
			if (err) console.error(err);
		});
	},

	saveActor: function() {
		var self = this;
		var actorData = '';
		self.actors.forEach(function(actor) {
			if( actor.oid > 0 )
			{
				actorData += actor.writeChunks();
				actorData += '\n';
			}
		});
		fs.writeFile(savedActorPath, actorData, function(err) {
			if (err) console.error(err);
		});
	},
	

	addActor: function(oid, tx, ty, sprite) {
		var self = this;
		var actor = self.actors[self.nextUID-1000];
		if( self.nextUID == 2000 )
		{
			console.log("reuse actors");
			self.nextUID = 1000;
		}
		else
		{
			self.nextUID++;
		}
		actor.oid = oid;
		actor.tx = tx;
		actor.ty = ty;
		actor.rx = tx*10+5;
		actor.ry = ty*10+5;
		actor.sprite = sprite;
		actor.hp = 10;
		actor.speed = 30;
		self.dirtyActors.push(actor);
	},

	modActor: function(chunks) {
		var self = this;
		var uid = chunks.shift();
		
		var actor = self.actors[uid-1000];
		if( actor != null )
		{
			var dirty = actor.readChunks(chunks);
			if( dirty )
			{
				self.dirtyActors.push(actor);
				//self.minorDirtyActors.push(actor);	
			}
		}
	},

	delActors: function(oid) {
		var self = this;
		self.actors.forEach(function(actor) {
			if( actor.oid == oid )
			{
				actor.erase();
				self.dirtyActors.push(actor);
			}
		});
	},

	getAll: function() {
		var self = this;
		var messages = '';
		self.actors.forEach(function(actor) {
			if( actor.oid > 0 && actor.hp > 0)
			{
				messages += 'actormod|';
				messages += actor.writeChunks();
				messages += '\n';
			}
		});
		self.sectors.forEach(function(sector) {
			messages += 'sectormod|';
			messages += sector.writeChunks();
			messages += '\n';
		});
		return messages;
	},

	getDirty: function() {
		var self = this;
		var messages = '';
		self.dirtyActors.forEach(function(actor) {
			messages += 'actormod|';
			messages += actor.writeChunks();
			messages += '\n';
		});
		self.dirtyActors.length = 0;
		/*self.minorDirtyActors.forEach(function(actor) {
			messages += 'actormod|';
			messages += actor.writeChunks();
			messages += '\n';
		});*/
		self.minorDirtyActors.length = 0;
		self.dirtySectors.forEach(function(sector) {
			messages += 'sectormod|';
			messages += sector.writeChunks();
			messages += '\n';
		});
		self.dirtySectors.length = 0; 
		return messages;
	},

	hasDirty: function() {
		var self = this;
		return self.dirtyActors.length > 0 || self.minorDirtyActors.length > 0 || self.dirtySectors.length > 0;
	}

};

module.exports = World;
