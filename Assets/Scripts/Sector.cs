using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sector : MonoBehaviour {

	public int sx;
	public int sy;
	string[] map;
	public int mapWidth = 10;
	World world;

	bool isInit = false;

	public List<Tile> spawnTiles = new List<Tile>();
	public Tile[,] tiles = new Tile[10,10];


	void Awake()
	{

	}

	void Start()
	{
		world = World.Instance;
	}

	// Update is called once per frame
	void Update () {
	
	}
	public void Init(int sx, int sy)
	{
		this.sx = sx;
		this.sy = sy;
	}

	public void Mod(string blob)
	{
		string[] blobArray = blob.Split(',');
		mapWidth = blobArray.Length;
		map = new string[mapWidth];
		for(int i=0; i< blobArray.Length; ++i)
		{
			map[i] = blobArray[i];
		}
		if( !isInit )
		{
			InitMap();
		}
		else
		{
			//?
		}
	}


	public void InitMap	()
	{		
		for(int ty=0; ty<mapWidth; ++ty)
		{
			for(int tx=0; tx<mapWidth; ++tx)	
			{
				string line = map[ty];
				char c = ' ';
				if( line.Length > tx )
				{
					c = line[tx];
				}
				string tileName = "Tile "+tx+","+ty;
				Vector3 tilePos = new Vector3((tx+sx*10)*10, 0, (ty+sy*10)*10);
				tilePos.x += 5;
				tilePos.z += 5;
				Tile t = world.MakeTile(c, tilePos, tileName);
				switch(t.tileType)
				{
					case Tile.TileType.SPAWN:
						spawnTiles.Add(t);
						break;
				}
				t.gameObject.transform.parent = this.transform;
				t.Init(tx,ty);
				tiles[tx,ty] = t;
			}
		}
		isInit = true;
	}

	public Tile GetTileAt(Vector3 pos)
	{
		float localX = pos.x - sx*100;
		float localY = pos.z - sx*100;
		int tx = (int)Mathf.Floor(localX/10f);
		int ty = (int)Mathf.Floor(localY/10f);
		if( tx >= 0 && tx < 10 && ty >= 0 && ty < 10 )
		{
			return tiles[tx,ty];
		}
		return null;
	}



}
