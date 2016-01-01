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
		sb.Length = 0;
		int uid = 0;
		int oid = 0;
		int team = 0;
		int.TryParse(chunks[1], out uid);
		int.TryParse(chunks[2], out oid);
		int.TryParse(chunks[3], out team);
		
		int tx = 0;
		int ty = 0;
		int rx = 0;
		int ry = 0;
		int fx = 0;
		int fy = 0;
		int.TryParse(chunks[4], out tx);
		int.TryParse(chunks[5], out ty);
		int.TryParse(chunks[6], out rx);
		int.TryParse(chunks[7], out ry);
		int.TryParse(chunks[8], out fx);
		int.TryParse(chunks[9], out fy);
		
		string sprite = chunks[10];
		
		int hp = 0;
		int speedlimit = 0;
		int damage = 0;
		int ttl = 0;
		if( uid < 0 )
		{
			uid = nextUID++;
			Lookup(sprite, out hp, out speedlimit, out damage, out ttl);
		}
		else
		{
			int.TryParse(chunks[11], out hp);
			int.TryParse(chunks[12], out speedlimit);
			int.TryParse(chunks[13], out damage);
			int.TryParse(chunks[14], out ttl);
			if( ttl <= 0 )
			{
				hp = -1;
			}
		}
		
		
		sb.Append("actormod|");
		sb.Append(uid);
		sb.Append('|');
		sb.Append(oid);
		sb.Append('|');
		sb.Append(team);
		sb.Append('|');
		sb.Append(tx);
		sb.Append('|');
		sb.Append(ty);
		sb.Append('|');
		sb.Append(rx);
		sb.Append('|');
		sb.Append(ry);
		sb.Append('|');
		sb.Append(sprite); 
		sb.Append('|');
		sb.Append(hp);
		sb.Append('|');
		sb.Append(speedlimit);
		sb.Append('|');
		sb.Append(damage);
		sb.Append('|');
		sb.Append(ttl);
		sb.Append('|');
		sb.Append(fx);
		sb.Append('|');
		sb.Append(fy);
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

	public void Lookup(string sprite, out int hp, out int speedlimit, out int dmg, out int ttl)
	{
		switch(sprite[0])
		{
			case '*': RulesBullet.Lookup(out hp, out speedlimit, out dmg, out ttl); break;
			case 'H': RulesHero.Lookup(out hp, out speedlimit, out dmg, out ttl); break;
			case 'D': RulesDog.Lookup(out hp, out speedlimit, out dmg, out ttl); break;
			default:  RulesDog.Lookup(out hp, out speedlimit, out dmg, out ttl); break;
		}

	}
	

}
