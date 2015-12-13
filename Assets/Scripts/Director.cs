using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Director : MonoBehaviour {

	public static Director Instance;
	
	public GameObject protoActorR;
	public GameObject protoActorB;

	private Dictionary<int,Actor> actorDict = new Dictionary<int,Actor>();
	private List<Actor> actorList = new List<Actor>();
	public List<Actor> localActors = new List<Actor>();


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


	public Actor AddActor(int rx, int ry, string sprite)
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
			Vector3 pos = new Vector3(rx, 1f, ry);
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
		int rx = Utils.IntParseFast(chunks[5]);
		int ry = Utils.IntParseFast(chunks[6]);
		string sprite = chunks[7];
		int hp = Utils.IntParseFast(chunks[8]);
		int speed = Utils.IntParseFast(chunks[9]);
		if( !actorDict.ContainsKey(uid) )
		{
			Actor a = AddActor(rx,ry,sprite);
			if( a != null )
			{
				a.Mod(true, uid,oid,tx,ty,rx,ry,sprite,hp,speed);
				actorDict[uid] = a;
				actorList.Add(a);
				ScanLocalActors();
			}
		}
		else
		{
			Actor a = actorDict[uid];
			if( oid != NetMan.Instance.localOID )
			{
				a.Mod(true, uid,oid,tx,ty,rx,ry,sprite,hp,speed);	
			}
			else
			{
				a.Mod(false, uid,oid,tx,ty,rx,ry,sprite,hp,speed);		
			}
		}
	}

	public void ScanLocalActors()
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

	public void SendUpdate()
	{
		int localOID = NetMan.Instance.localOID;
		for(int i=0; i<localActors.Count; ++i)
		{
			Actor actor = localActors[i];
			int uid = actor.uid;
			int tx = actor.tx;
			int ty = actor.ty;
			int rx = actor.rx;
			int ry = actor.ry;
			string sprite = actor.spriter.spriteString;
			int hp = actor.hp;
			int speed = actor.speed;
			NetMan.Instance.Send("requestactormod|"+uid+"|"+localOID+"|"+tx+"|"+ty+"|"+rx+"|"+ry+"|"+sprite+"|"+hp+"|"+speed+"\n");
		}
	}
}
