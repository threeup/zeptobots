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
		for(int lty=0; lty<mapWidth; ++lty)
		{
			for(int ltx=0; ltx<mapWidth; ++ltx)	
			{
				string line = map[lty];
				char c = ' ';
				if( line.Length > ltx )
				{
					c = line[ltx];
				}
				string tileName = "Tile "+ltx+","+lty;
				int rtx = ltx+sx*10;
				int rty = lty+sy*10;
				Vector3 tilePos = new Vector3(rtx*10, 0, rty*10);
				tilePos.x += 5;
				tilePos.z += 5;
				if( c == 'S')
				{
					if( sy > 5 )
					{
						c = 'R';
					}
					else
					{
						c = 'B';
					}
				}
				Tile t = world.MakeTile(c, tilePos, tileName);
				if( t != null )
				{
					switch(t.tileType)
					{
						case Tile.TileType.SPAWN:
							spawnTiles.Add(t);
							break;
					}
					t.gameObject.transform.parent = this.transform;
					t.Init(ltx,lty,rtx,rty);
					tiles[ltx,lty] = t;
				}
			}
		}
		isInit = true;
	}

	public Tile GetTileAt(Vector3 pos)
	{
		float localX = pos.x - sx*100;
		float localY = pos.z - sy*100;
		int ltx = (int)Mathf.Floor(localX/10f);
		int lty = (int)Mathf.Floor(localY/10f);
		if( ltx >= 0 && ltx < 10 && lty >= 0 && lty < 10 )
		{
			return tiles[ltx,lty];
		}
		return null;
	}



}
