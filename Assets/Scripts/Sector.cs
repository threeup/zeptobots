using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sector : MonoBehaviour {

	public int sx;
	public int sy;
	string[] map;
	int mapWidth = 10;
	World world;

	bool isPopulated = false;

	public List<Kingdom> kingdoms = new List<Kingdom>();
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

	public bool Mod(string blob)
	{
		string[] blobArray = blob.Split(',');
		mapWidth = blobArray.Length-1;
		map = new string[mapWidth];
		for(int i=0; i< mapWidth; ++i)
		{
			map[i] = blobArray[i];
		}
		if( !isPopulated )
		{
			bool isDifferent = PopulateMap();
			if( isDifferent )
			{
				RefreshMap();
				return true;
			}
		}
		else
		{
			bool isDifferent = ChangeMap();
			if( isDifferent )
			{
				RefreshMap();
			}
			
		}
		return false;
		
	}

	public bool ChangeMap()
	{		
		bool result = false;
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
				Tile t = tiles[ltx,lty];
				result |= t.SetContents(c);
			}
		}
		return isPopulated;
	}


	public bool PopulateMap	()
	{		
		isPopulated = false;
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
				string tileName = "Tile "+ltx+","+lty+" of "+this.gameObject.name;
				int rtx = ltx+sx*10;
				int rty = lty+sy*10;
				Vector3 tilePos = new Vector3(rtx*10, 0, -rty*10);
				Tile t = world.MakeTile(c, tilePos, tileName);
				if( t != null )
				{
					isPopulated = true;
					t.gameObject.transform.parent = this.transform;
					t.Init(ltx,lty,rtx,rty,sx,sy);
					t.SetContents(c);
					tiles[ltx,lty] = t;

					if( (t.tFlags & Tile.TileFlags.KINGDOM) != 0 )
					{
						Kingdom kd = t.GetComponentInChildren<Kingdom>();
						if( kd != null )
						{
							kingdoms.Add(kd);
						}
					}
				}
			}
		}
		return isPopulated;
	}

	public void RefreshMap()
	{

		for(int lty=0; lty<mapWidth; ++lty)
		{
			for(int ltx=0; ltx<mapWidth; ++ltx)	
			{
				Tile t = tiles[ltx,lty];
				if( t )
				{
					if( ltx < mapWidth-1 )
					{
						t.AddNbor(tiles[ltx+1,lty]);
					}
					if( lty < mapWidth -1 )
					{
						t.AddNbor(tiles[ltx,lty+1]);
					}
					if( ltx < mapWidth-1 && lty < mapWidth-1)
					{
						t.AddNbor(tiles[ltx+1,lty+1]);
					}
					if( ltx > 0 && lty < mapWidth-1 )
					{
						t.AddNbor(tiles[ltx-1,lty+1]);
					}
				}
			}
		}
		for(int lty=0; lty<mapWidth; ++lty)
		{
			for(int ltx=0; ltx<mapWidth; ++ltx)	
			{
				Tile t = tiles[ltx,lty];
				if( t )
				{
					t.RefreshSprite();
				}
			}
		}
	}

	public Tile GetTileAt(Vector3 pos)
	{
		float localX = pos.x - sx*100;
		float localY = -pos.z - sy*100;
		int ltx = (int)Mathf.Floor(localX/10f);
		int lty = (int)Mathf.Floor(localY/10f);
		if( ltx >= 0 && ltx < 10 && lty >= 0 && lty < 10 )
		{
			return tiles[ltx,lty];
		}
		else
		{
			Debug.Log("wrong"+ltx+" "+lty);
		}
		return null;
	}



}
