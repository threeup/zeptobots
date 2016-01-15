using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

	public static World Instance;
	public Sector[,] sectors = new Sector[10,10];
	public int sectorCount = 0;


	public TileMaterial wallMaterial;

	public GameObject protoSector;
	public Dictionary<string, GameObject> protos;


	void Awake()
	{
		Instance = this;
		
	}

	void Start()
	{
		protos = new Dictionary<string, GameObject>();
		ProtoVox("terrain-grass1");
		ProtoVox("terrain-hill1");
		ProtoVox("terrain-mountain1");
		ProtoVox("prop-tree1");
		ProtoVox("prop-tree2");
		ProtoVox("prop-house1");
		ProtoVox("prop-tower1");
		ProtoVox("prop-barn1");
		ProtoVox("actor-man1");
		ProtoVox("actor-dog1");
		ProtoVox("actor-mega1");
		ProtoBlend("over-red");
		ProtoBlend("over-blue");
		ProtoBlend("over-gray");
		
		foreach(Transform child in transform)
		{
			child.gameObject.SetActive(false);
		}

		for(int sy=0; sy<10; ++sy)
		{
			for(int sx=0; sx<10; ++sx)
			{
				Vector3 pos = new Vector3(sx*100, 0, -sy*100);
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

	void ProtoBlend(string str)
	{
		GameObject go = Resources.Load("Prefabs/pr-"+str) as GameObject;
		if( go == null )
		{
			Debug.Log("cant load "+str);
			return;
		}
		protos.Add(str,go);
	}

	void ProtoVox(string str)
	{
		GameObject go = Resources.Load("Vox/"+str) as GameObject;
		if( go == null )
		{
			Debug.Log("cant load "+str);
			return;
		}
		protos.Add(str,go);
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
		bool created = sectors[sx,sy].Mod(contents);
		if( created )
		{
			sectorCount++;
		}
	}

	public Tile MakeTile(char c, Vector3 pos, string tname)
	{
		GameObject prototype = null;
		Tile tile = null;
		switch(c)
		{
			default:  prototype = protos["terrain-grass1"];; break;
		}
		if( prototype )
		{
			GameObject go = GameObject.Instantiate(prototype, pos, prototype.transform.rotation) as GameObject;
			go.name = tname;
			go.SetActive(true);
			tile = go.GetComponent<Tile>();
			tile.tileMat = wallMaterial;
		}
		else
		{
			Debug.Log("broken tile");
		}
		return tile;
	}

	public TileContents MakeTileContents(char c, Transform root)
	{
		List<GameObject> prototypes = new List<GameObject>();
		TileContents contents = null;
		switch(c)
		{
			case 'A': prototypes.Add(protos["prop-tree1"]); break;
			case 'B': prototypes.Add(protos["over-blue"]); prototypes.Add(protos["prop-house1"]); break;
			case 'C': prototypes.Add(protos["prop-tree2"]); break;
			case 'D': prototypes.Add(protos["prop-tree2"]); break;
			case 'R': prototypes.Add(protos["over-red"]); prototypes.Add(protos["prop-house1"]); break;
			case 'S': prototypes.Add(protos["over-gray"]); prototypes.Add(protos["prop-house1"]); break;
			case 'T': prototypes.Add(protos["prop-tree2"]); break;
			case 'W': prototypes.Add(protos["terrain-mountain1"]); break;
			default:  break;
		}
		for( int i=0; i< prototypes.Count; ++i )
		{
			GameObject go = GameObject.Instantiate(prototypes[i], Vector3.zero, Quaternion.identity) as GameObject;
			go.transform.SetParent(root,false);
			go.SetActive(true);
			contents = go.GetComponent<TileContents>();
		}
		return contents;
	}
	
	public Sector GetRandomSector()
	{
		return sectors[Random.Range(0,10),Random.Range(0,10)];
	}

	public Sector GetSectorAt(Vector3 pos)
	{
		float localX = pos.x;
		float localY = -pos.z;
		int sx = (int)Mathf.Floor(localX/100f);
		int sy = (int)Mathf.Floor(localY/100f);
		if( sx >= 0 && sx < 10 && sy >= 0 && sy < 10 )
		{
			return sectors[sx,sy];
		}
		return null;
	}

}
