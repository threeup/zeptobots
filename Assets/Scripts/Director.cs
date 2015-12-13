using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Director : MonoBehaviour {

	public static Director Instance;
	
	public GameObject protoActorR;
	public GameObject protoActorB;

	private Dictionary<int,Actor> actorDict = new Dictionary<int,Actor>();
	private List<Actor> actorList = new List<Actor>();


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

	}

	
	// Update is called once per frame
	void Update () {
	
	}

	public void MessageReceive(string message)
	{
		string[] chunks = message.Split('|');
		switch(chunks[0])
		{
			case "actormod":
				ModActor(chunks);
				break;
			default:
				break;
		}
		
	}


	public Actor AddActor(int tx, int ty, string sprite)
	{
		GameObject prototype = null;
		Actor actor = null;
		switch(sprite[0])
		{
			case 'R': prototype = protoActorR; break;
			case 'B': prototype = protoActorB; break;
			default: break;
		}
		if( prototype )
		{
			Vector3 pos = new Vector3(tx*10, 1f, ty*10);
			GameObject go = GameObject.Instantiate(prototype, pos, prototype.transform.rotation) as GameObject;
			go.SetActive(true);
			actor = go.GetComponent<Actor>();
		}
		return actor;
	}

	public void ModActor(string[] chunks)
	{
		int uid = Utils.IntParseFast(chunks[1]);
		int oid = Utils.IntParseFast(chunks[2]);
		int tx = Utils.IntParseFast(chunks[3]);
		int ty = Utils.IntParseFast(chunks[4]);
		string sprite = chunks[5];
		int hp = Utils.IntParseFast(chunks[6]);
		if( !actorDict.ContainsKey(uid) )
		{
			Actor a = AddActor(tx,ty,sprite);
			if( a != null )
			{
				a.Mod(true, uid,oid,tx,ty,sprite,hp);
				actorDict[uid] = a;
				actorList.Add(a);
			}
		}
		else
		{
			Actor a = actorDict[uid];
			bool authoritative = oid != NetMan.Instance.localOID;
			a.Mod(authoritative, uid,oid,tx,ty,sprite,hp);	
		}
	}

	public void GetLocalActors(ref List<Actor> localActors)
	{
		localActors.Clear();
		int localOID = NetMan.Instance.localOID;
		for(int i = 0; i<actorList.Count; ++i)
		{
			Actor a = actorList[i];
			if( a.oid == localOID )
			{
				localActors.Add(a);
			}
		}
	} 
}
