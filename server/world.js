var fs = require('fs');
var Actor = require('./actor');
var Sector = require('./sector');
var savedGroundPath = './saved-ground.txt';
var savedActorPath = './saved-actor.txt';

var World = {

	sectors: [],
	actors: [],
	dirtySectors: [],
	majorDirtyActors: [],
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

	loadGround: function(callback) {
		var self = this;
		fs.exists(savedGroundPath, function(exists) {
		    if (!exists) { callback(0); return; }
	        fs.readFile(savedGroundPath, function(err, body) {
				if (err) throw err;
				body.toString().split('\n').forEach(function(line) {
					var chunks = line.toString().split('|');
					if( chunks.length > 2)
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
						var oid = chunks.shift();
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
	

	addActor: function(oid, tx, ty, sprite, hp) {
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
		actor.mod(oid, tx, ty, tx*10, ty*10, sprite, hp);
		self.majorDirtyActors.push(actor);
	},

	modActor: function(uid, oid, tx, ty, rx, ry, sprite, hp) {
		var self = this;
		var actor = self.actors[uid-1000];
		if( actor != null )
		{
			var major = actor.mod(oid, tx, ty, sprite, hp);
			if( major )
			{
				self.majorDirtyActors.push(actor);
			}
			else
			{
				self.minorDirtyActors.push(actor);	
			}
		}
	},

	delActors: function(oid) {
		var self = this;
		self.actors.forEach(function(actor) {
			if( actor.oid == oid )
			{
				actor.modhp(-1);
				self.majorDirtyActors.push(actor);
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
		self.majorDirtyActors.forEach(function(actor) {
			messages += 'actormod|';
			messages += actor.writeChunks();
			messages += '\n';
		});
		self.majorDirtyActors.length = 0;
		self.minorDirtyActors.forEach(function(actor) {
			messages += 'actormod|';
			messages += actor.writeChunks();
			messages += '\n';
		});
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
		return self.majorDirtyActors.length > 0 || self.minorDirtyActors.length > 0 || self.dirtySectors.length > 0;
	}

};

module.exports = World;
