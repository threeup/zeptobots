using UnityEngine;
using System.Collections;

public class LocalMan : MonoBehaviour {

	public static LocalMan Instance;

	string[] storedSectorData = new string[100];

	private int nextUID = 1000;

	private bool isConnected = false;
	public bool IsConnected
	{
		get { return isConnected; }
	}

	private System.Text.StringBuilder sb;

	void Awake()
	{
		Instance = this;
		sb = new System.Text.StringBuilder();
		for(int sy=0; sy<10; ++sy)
		{
			for(int sx=0; sx<10; ++sx)
			{
				storedSectorData[sy*10+sx] = "WWWWFFWWWW,WFSFFFFFFW,WFFFSTTFFW,WFFWWWFFFW,FFFFFFFFFF,FFFFFFFFFF,WFFFWWWFFW,WFFFFSFFFW,WFFFFFFSFW,WWWWFFWWWW,";
			}
		}
	}

	string GetSector(int sx, int sy)
	{
		return storedSectorData[sy*10+sx];
	}

	void SetSector(int sx, int sy, int tx, int ty, char c)
	{
		char[] charArr = storedSectorData[sy*10+sx].ToCharArray();
		charArr[ty*11+tx] = c;
		storedSectorData[sy*10+sx] = new string(charArr);
	}



	public IEnumerator SetupRoutine()
	{
		HandleMessage(GetClientSetup());
		for(int sy=0; sy<10;++sy)
		{
			for(int sx=0; sx<10;++sx)
			{
				HandleMessage(GetWorld(sx,sy));
			}
		}
		HandleMessage(GetClientMessage());
		//HandleMessage(GetActorMessage());
		isConnected = true;
		yield return null;
	}

	string GetClientSetup()
	{
		return "setupclient|101|true";
	}

	string GetClientMessage()
	{
		return "newclient|101|true";
	}


	string GetActorMessage(string[] chunks)
	{
		ActorData ad = new ActorData();
		sb.Length = 0;
		int.TryParse(chunks[(int)Pck.Ac.UID], out ad.uid);
		int.TryParse(chunks[(int)Pck.Ac.OID], out ad.oid);
		int.TryParse(chunks[(int)Pck.Ac.TEAM], out ad.team);
		
		int.TryParse(chunks[(int)Pck.Ac.TX], out ad.tx);
		int.TryParse(chunks[(int)Pck.Ac.TY], out ad.ty);
		int.TryParse(chunks[(int)Pck.Ac.RX], out ad.rx);
		int.TryParse(chunks[(int)Pck.Ac.RY], out ad.ry);
		int.TryParse(chunks[(int)Pck.Ac.FX], out ad.fx);
		int.TryParse(chunks[(int)Pck.Ac.FY], out ad.fy);
		
		ad.spriteString = chunks[(int)Pck.Ac.SPRITE];
		
		if( ad.uid < 0 )
		{
			ad.uid = nextUID++;
			Lookup(ad.spriteString, out ad.defaulthp, out ad.defaultSpeedLimit, out ad.damage, out ad.ttl);
			ad.hp = ad.defaulthp;
			ad.currentSpeedLimit = ad.defaultSpeedLimit;
		}
		else
		{
			int.TryParse(chunks[(int)Pck.Ac.HP], out ad.hp);
			int.TryParse(chunks[(int)Pck.Ac.DEFAULTHP], out ad.defaulthp);
			int.TryParse(chunks[(int)Pck.Ac.CURRENTSPEEDLIMIT], out ad.currentSpeedLimit);
			int.TryParse(chunks[(int)Pck.Ac.DEFAULTSPEEDLIMIT], out ad.defaultSpeedLimit);
			int.TryParse(chunks[(int)Pck.Ac.DAMAGE], out ad.damage);
			int.TryParse(chunks[(int)Pck.Ac.TTL], out ad.ttl);
			if( ad.ttl <= 0 )
			{
				ad.hp = -1;
			}
		}
		string actionChunks = chunks[(int)Pck.Ac.ACTIONS];
		string effectChunks = chunks[(int)Pck.Ac.EFFECTS];
		
		sb.Append("actormod|");
		Pck.PackActorData(sb,ad);
		sb.Append(actionChunks);
		sb.Append('|');
		sb.Append(effectChunks);
		sb.Append('|');
		return sb.ToString();
	}

	void ChangeWorld(string[] chunks)
	{
		sb.Length = 0;

		int sx = 0;
		int.TryParse(chunks[1], out sx);
		int sy = 0;
		int.TryParse(chunks[2], out sy);
		int tx = 0;
		int.TryParse(chunks[3], out tx);
		int ty = 0;
		int.TryParse(chunks[4], out ty);
		char c = chunks[5][0];
		SetSector(sx,sy,tx,ty,c);
	}

	string GetWorld(string[] chunks)
	{
		int sx = 0;
		int.TryParse(chunks[1], out sx);
		int sy = 0;
		int.TryParse(chunks[2], out sy);
		
		return GetWorld(sx,sy);
	}

	string GetWorld(int sx, int sy)
	{
		sb.Length = 0;

		sb.Append("sectormod|");
		sb.Append(sx);
		sb.Append('|');
		sb.Append(sy);
		sb.Append('|');
		if( sx >= 4 && sx <= 6 && sy >=4 && sy <= 6)
		{
			sb.Append(GetSector(sx,sy));
		}
		
		return sb.ToString();
	}

	void HandleMessage(string msg)
	{
		Boss.Instance.MessageReceive(msg);
		Director.Instance.MessageReceive(msg);
		World.Instance.MessageReceive(msg);
	}


	public void Send(byte[] data)
	{
		
	}
	public void Send(string message)
	{
		string[] chunks = message.Split('|');
		switch(chunks[0])
		{
			case "requestactor":
				HandleMessage(GetActorMessage(chunks));
				break;
			case "requestworldmod":
				ChangeWorld(chunks);
				HandleMessage(GetWorld(chunks));
				break;
			default:
				Debug.Log("tx? "+message);
				break;
		}
	}

	public void Lookup(string sprite, out int defaulthp, out int defaultSpeedLimit, out int dmg, out int ttl)
	{
		switch(sprite[0])
		{
			case '*': RulesBullet.Lookup(out defaulthp, out defaultSpeedLimit, out dmg, out ttl); break;
			case 'H': RulesHero.Lookup(out defaulthp, out defaultSpeedLimit, out dmg, out ttl); break;
			case 'D': RulesDog.Lookup(out defaulthp, out defaultSpeedLimit, out dmg, out ttl); break;
			default:  RulesDog.Lookup(out defaulthp, out defaultSpeedLimit, out dmg, out ttl); break;
		}

	}
	

}
