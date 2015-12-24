using UnityEngine;
using System.Collections;

public class LocalMan : MonoBehaviour {

	public static LocalMan Instance;

	string[] storedSectorData = new string[100];

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

		int rx = 0;
		int.TryParse(chunks[3], out rx);
		int ry = 0;
		int.TryParse(chunks[4], out ry);
		int tx = (int)Mathf.Round((rx-5)/10f);
		int ty = (int)Mathf.Round((ry-5)/10f);
		sb.Append("actormod|");
		sb.Append(1000);
		sb.Append('|');
		sb.Append(chunks[1]);
		sb.Append('|');
		sb.Append(chunks[2]);
		sb.Append('|');
		sb.Append(tx);
		sb.Append('|');
		sb.Append(ty);
		sb.Append('|');
		sb.Append(rx);
		sb.Append('|');
		sb.Append(ry);
		sb.Append('|');
		sb.Append(chunks[5]);
		sb.Append('|');
		sb.Append(10);
		sb.Append('|');
		sb.Append(30);
		sb.Append('|');
		sb.Append(1);
		sb.Append('|');
		sb.Append(1000);
		sb.Append('|');
		sb.Append(0);
		sb.Append('|');
		sb.Append(0);
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
			case "requestactoradd":
				HandleMessage(GetActorMessage(chunks));
				break;
			case "requestactormod":
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

}
