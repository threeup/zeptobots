using UnityEngine;
using System;
using System.Collections;
using WebSocketSharp;

public class NetMan : MonoBehaviour {


	public static NetMan Instance;


	void Awake()
	{
		Instance = this;
	}

	public string websocketServerUrl = "ws://45.55.10.165:3000";
#if (UNITY_EDITOR || !UNITY_WEBGL) && !WEBSOCKET_BROWSER_DEBUG 
    private WebSocket ws_;
#else
    private WebSocketBrowser ws_;
#endif

	private bool isConnected_ = false;
	public bool isConnected
	{
		get { return isConnected_; }
	}

	public delegate void ReceiveEventListener(string data);
	public ReceiveEventListener OnReceive = data => {};
	public ReceiveEventListener OnMessage = data => {};


	public void Launch()
	{
		StartCoroutine(SetupRoutine());
	}

    public void BlobReceive(string blob)
    {
    	string[] chunks = blob.Split('\n');
    	//Debug.Log("blob["+chunks.Length+"] "+blob);
        for(int i=0; i<chunks.Length; ++i)
        {
        	string chunk = chunks[i];
        	if( !string.IsNullOrEmpty(chunk) )
        	{
        		OnMessage(chunk);
        	}
        }
    }

	public void MessageReceive(string message)
	{
		string[] chunks = message.Split('|');
		switch(chunks[0])
		{
			case "setupclient":
				if( Boss.Instance.localOID > 0 )
				{
					Debug.LogError("Double setup");
					return;
				}
				Boss.Instance.localOID = Utils.IntParseFast(chunks[1]);
				Debug.LogError("LocalOID"+Boss.Instance.localOID);
				Boss.Instance.localIsRed = chunks[2].StartsWith("true");
				break;
			default:
				break;
		}
	}

	IEnumerator SetupRoutine()
	{
#if (UNITY_EDITOR || !UNITY_WEBGL) && !WEBSOCKET_BROWSER_DEBUG 
        ws_ = new WebSocket( new Uri(websocketServerUrl) );
        yield return StartCoroutine( ws_.Connect() );
#else
		if (!ws_) {
			ws_ = GetComponent<WebSocketBrowser>() ?? gameObject.AddComponent<WebSocketBrowser>();
			Application.ExternalEval("init();");
		}
#endif
        isConnected_ = true;
        OnReceive += BlobReceive;
        OnMessage += MessageReceive;
        OnMessage += World.Instance.MessageReceive;
        OnMessage += Director.Instance.MessageReceive;
		StartCoroutine(NetManRoutine());
		yield return null;
	}


	IEnumerator NetManRoutine()
    {
        for (;;) {
            for (;;) {
                var message = ws_.RecvString();
                while (message != null) {
					if (Application.isPlaying) {
						OnReceive(message);
					}
                    message = ws_.RecvString();
                }
                if (ws_.Error != null) {
                    LogError(ws_.Error);
                    isConnected_ = false;
                    yield return new WaitForSeconds(1);
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }

	void OnDestroy()
	{
		if( ws_ != null )
		{
			ws_.Close();
		}
	}

	public void Send(byte[] data)
	{
		ws_.Send(data);
	}

	public void Send(string data)
	{
		ws_.SendString(data);
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
