using UnityEngine;
using System;
using System.Collections;
using WebSocketSharp;

public class ServerMan : MonoBehaviour {


	public static ServerMan Instance;


	void Awake()
	{
		Instance = this;
	}

	public string websocketServerUrl = "ws://45.55.10.165:3000";
#if (UNITY_EDITOR || !UNITY_WEBGL) && !WEBSOCKET_BROWSER_DEBUG 
    private WebSocket ws;
#else
    private WebSocketBrowser ws;
#endif

    private NetMan nm;

	private bool isConnected = false;
	public bool IsConnected
	{
		get { return isConnected; }
	}






	public IEnumerator SetupRoutine()
	{
		nm = NetMan.Instance;
#if (UNITY_EDITOR || !UNITY_WEBGL) && !WEBSOCKET_BROWSER_DEBUG 
        ws = new WebSocket( new Uri(websocketServerUrl) );
        yield return StartCoroutine( ws.Connect() );
#else
		if (!ws) {
			ws = GetComponent<WebSocketBrowser>() ?? gameObject.AddComponent<WebSocketBrowser>();
			Application.ExternalEval("init();");
		}
#endif
        isConnected = true;
        
		StartCoroutine(TickRoutine());
		yield return null;
	}


	IEnumerator TickRoutine()
    {
        for (;;) {
            for (;;) {
                var message = ws.RecvString();
                while (message != null) {
					if (Application.isPlaying) {
						nm.OnReceive(message);
					}
                    message = ws.RecvString();
                }
                if (ws.Error != null) {
                    LogError(ws.Error);
                    isConnected = false;
                    yield return new WaitForSeconds(1);
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }

	void OnDestroy()
	{
		if( ws != null )
		{
			ws.Close();
		}
	}

	public void Send(byte[] data)
	{
		ws.Send(data);
	}

	public void Send(string data)
	{
		ws.SendString(data);
	}

	void Log(string message)
	{
		Debug.Log(message);
		Application.ExternalCall("console.log", message);
	}
	
	void LogError(string error)
	{
		Debug.LogError(error);
		Application.ExternalCall("console.error", error);
	}
	
}
