var ws           = require('ws').Server;
var server       = new ws({ port: 3000 });
var world 		 = require('./world');
var fps          = 4;


server.broadcast = function(data) {
	var self = this;
	this.clients.forEach(function(client, index) {
		//var info = index;
		var message = data;
		client.send(message, function(err) {
			if (err) console.error(err);
		});
	});
};
var clientNum = 0;
var nextIsRed = true;
server.on('connection', function(socket) {
	console.log('connected');

	clientNum = server.clients.length;
	var oid = 100+clientNum;

	socket.send('setupclient|'+oid+"|"+nextIsRed);
	var allData = world.getAll(0)
	socket.send(allData);
	server.broadcast('newclient|'+oid+"|"+nextIsRed);
	nextIsRed = !nextIsRed;
	socket.on('message', function(message) {
		var chunks = message.toString().split('|');
		var cmd = chunks.shift();
		
		if( cmd == "requestactoradd" )
		{
			//oid, x, y, sprite
			world.addActor(chunks[0], chunks[1], chunks[2], chunks[3]);
		}
		if( cmd == "requestactormod" )
		{
			//uid, oid, tx, ty, rx, ry, sprite, hp, speed
			world.modActor(chunks);
		}
	});

	socket.on('close', function() {
		console.log('disconnected...');
		world.delActors(oid);
		clientNum = server.clients.length;
	});

	setInterval(function() {
		if (world.hasDirty()) {
			var dirtyData = world.getDirty();
			server.broadcast(dirtyData);
		}
	}, 1000 / fps);
});
world.initSector();
world.loadGround(function(count) {
	
	console.log('loaded World '+count);
});
/*world.loadActor(function(count) {
	
	console.log('loaded Actor '+count);
});*/


setInterval(function() {
	world.saveGround();
	world.saveActor();
}, 1000 * 10);

process.on('uncaughtException', function(e) {
	console.error("Unexpected Exception:", e.stack);
});
