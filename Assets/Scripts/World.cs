using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

	public static World Instance;
	public Sector[,] sectors = new Sector[10,10];


	public TileMaterial wallMaterial;

	public GameObject protoSector;
	public GameObject protoTileFloor;
	public GameObject protoTileWall;
	public GameObject protoTileTree;
	public GameObject protoTileRedHouse;
	public GameObject protoTileBlueHouse;


	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		foreach(Transform child in transform)
		{
			child.gameObject.SetActive(false);
		}

		for(int sy=0; sy<10; ++sy)
		{
			for(int sx=0; sx<10; ++sx)
			{
				Vector3 pos = new Vector3(sx*100, 0, sy*100);
				GameObject go = GameObject.Instantiate(protoSector, pos, protoSector.transform.rotation) as GameObject;
				go.name = "Sector "+sx+","+sy;
				go.transform.parent = this.transform;
				Sector s = go.GetComponent<Sector>();
				s.Init(sx,sy);
				sectors[sx,sy] = s;
				go.SetActive(true);
			}	
		}
	}


	// Update is called once per frame
	void Update () {
	
	}

	public void MessageReceive(string message)
	{
		string[] chunks = message.Split('|');
		switch(chunks[0])
		{
			case "sectormod":
				ModSector(chunks);
				break;
			default:
				break;
		}
		
	}

	public void ModSector(string[] chunks)
	{
		int sx = Utils.IntParseFast(chunks[1]);
		int sy = Utils.IntParseFast(chunks[2]);
		string contents = chunks[3];
		sectors[sx,sy].Mod(contents);	
	}

	public Tile MakeTile(char c, Vector3 pos, string tname)
	{
		GameObject prototype = null;
		Tile tile = null;
		switch(c)
		{
			case 'F': prototype = protoTileFloor; break;
			case 'W': prototype = protoTileWall; break;
			case 'T': prototype = protoTileTree; break;
			case 'R': prototype = protoTileRedHouse; break;
			case 'B': prototype = protoTileBlueHouse; break;
			default:  break;
		}
		if( prototype )
		{
			GameObject go = GameObject.Instantiate(prototype, pos, prototype.transform.rotation) as GameObject;
			go.name = tname;
			go.SetActive(true);
			tile = go.GetComponent<Tile>();
			tile.tileMat = wallMaterial;
		}
		return tile;
	}
	
	public Sector GetRandomSector()
	{
		return sectors[Random.Range(0,10),Random.Range(0,10)];
	}

	public Sector GetSectorAt(Vector3 pos)
	{
		int sx = (int)Mathf.Floor(pos.x/100f);
		int sy = (int)Mathf.Floor(pos.z/100f);
		if( sx >= 0 && sx < 10 && sy >= 0 && sy < 10 )
		{
			return sectors[sx,sy];
		}
		return null;
	}
	public Tile GetTileAt(Vector3 pos)
	{
		int sx = (int)Mathf.Floor(pos.x/100f);
		int sy = (int)Mathf.Floor(pos.z/100f);
		if( sx >= 0 && sx < 10 && sy >= 0 && sy < 10 )
		{
			return sectors[sx,sy].GetTileAt(pos);
		}
		return null;
	}
}
