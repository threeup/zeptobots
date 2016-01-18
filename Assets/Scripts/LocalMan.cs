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
		ActorBasicData abd = new ActorBasicData();
		ActorQuickData aqd = new ActorQuickData();
		
		Pck.UnpackData(chunks, ref abd);
		Pck.UnpackData(chunks, ref aqd);
		
		if( abd.uid < 0 )
		{
			abd.uid = nextUID++;
			int ttl = 99;
			Lookup(abd.visualString, out abd.hp, out abd.topSpeed, out abd.damage, out ttl);
			aqd = new ActorQuickData(abd);
			aqd.ttl = ttl;
		}
		if( aqd.ttl <= 0 )
		{
			aqd.hp = -1;
		}

		sb.Length = 0;
		sb.Append("actormod|");
		Pck.PackData(sb, abd);
		Pck.PackData(sb, aqd);
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

	public void Lookup(string sprite, out int hp, out int topSpeed, out int dmg, out int ttl)
	{
		switch(sprite[0])
		{
			case '*': RulesBullet.Lookup(out hp, out topSpeed, out dmg, out ttl); break;
			case 'H': RulesHero.Lookup(out hp, out topSpeed, out dmg, out ttl); break;
			case 'D': RulesDog.Lookup(out hp, out topSpeed, out dmg, out ttl); break;
			default:  RulesDog.Lookup(out hp, out topSpeed, out dmg, out ttl); break;
		}

	}
	

}
