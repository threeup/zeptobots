using UnityEngine;
using System;
using System.Collections;
using WebSocketSharp;

public class NetMan : MonoBehaviour {


	public static NetMan Instance;
	public bool isLocal = false;


	public delegate void ReceiveEventListener(string data);
	public ReceiveEventListener OnReceive = data => {};
	public ReceiveEventListener OnMessage = data => {};

	private System.Text.StringBuilder sb = new System.Text.StringBuilder();

	void Awake()
	{
		Instance = this;
	}

	public bool IsConnected
	{
		get {
			if( isLocal )
			{
				return LocalMan.Instance.IsConnected;
			}
			else
			{
				return ServerMan.Instance.IsConnected;	
			}
		
		}
		
	}

	public void Launch(bool local)
	{
		isLocal = local;
		OnReceive += BlobReceive;
        OnMessage += Boss.Instance.MessageReceive;
        OnMessage += World.Instance.MessageReceive;
        OnMessage += Director.Instance.MessageReceive;

		if( isLocal )
		{
			StartCoroutine(LocalMan.Instance.SetupRoutine());
		}
		else
		{
			StartCoroutine(ServerMan.Instance.SetupRoutine());
		}
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

    public void Send(string str)
    {
    	if( isLocal )
		{
			LocalMan.Instance.Send(str);
		}
		else
		{
			ServerMan.Instance.Send(str);
		}
    }

    public void SendReqWorld(int sx, int sy, int ltx, int lty, string spriteString)
    {
    	sb.Length = 0;
		sb.Append("requestworldmod|");
		sb.Append(sx);
		sb.Append("|");
		sb.Append(sy);
		sb.Append("|");
		sb.Append(ltx);
		sb.Append("|");
		sb.Append(lty);
		sb.Append("|");
		sb.Append(spriteString);
		sb.Append("\n");
		Send( sb.ToString() );
    }


    public void SendReqActor(ActorData ad)
    {
    	sb.Length = 0;
		sb.Append("requestactor|");
		Pck.PackActorData(sb, ad);
		Pck.PackActorExtra(sb, null);
		sb.Append("\n");
		Send( sb.ToString() );
    }
    

}
