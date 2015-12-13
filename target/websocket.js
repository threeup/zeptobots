(function() {
	window.ws = new WebSocket('ws://45.55.10.165:3000');

	var isInitialized = false;
	var cache = "";

	window.init = function() {
		isInitialized = true;
		SendMessage('netman', 'PushWebSocketData', cache);
	};

	ws.onmessage = function(event) {
		if (isInitialized) {
			SendMessage('netman', 'PushWebSocketData', event.data);
		} else {
			cache += event.data + '\n';
		}
	};
})();
