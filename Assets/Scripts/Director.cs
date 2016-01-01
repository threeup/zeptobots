using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Director : MonoBehaviour {

	public static Director Instance;
	
	public GameObject protoActorHero;
	public GameObject protoActorDog;
	public GameObject protoActorBullet;

	private Dictionary<int,Actor> actorDict = new Dictionary<int,Actor>();
	private List<Actor> actorList = new List<Actor>();
	public List<Actor> ActorList { get { return actorList; } }


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
			case 'H': prototype = protoActorHero; break;
			case 'D': prototype = protoActorDog; break;
			case '*': prototype = protoActorBullet; break;
			default: break;
		}
		if( prototype )
		{
			Vector3 pos = new Vector3(rx-5, 1f, -ry-5);
			GameObject go = GameObject.Instantiate(prototype, pos, prototype.transform.rotation) as GameObject;
			go.SetActive(true);
			actor = go.GetComponent<Actor>();
			if( actor.hero )
			{
				actor.hero.actions = go.GetComponents<GameAbility>();
			}
			
		}
		return actor;
	}

	public void ModActor(string[] chunks)
	{
		int uid = Utils.IntParseFast(chunks[1]);
		int oid = Utils.IntParseFast(chunks[2]);
		int team = Utils.IntParseFast(chunks[3]);
		int tx = Utils.IntParseFast(chunks[4]);
		int ty = Utils.IntParseFast(chunks[5]);
		int rx = Utils.IntParseFast(chunks[6]);
		int ry = Utils.IntParseFast(chunks[7]);
		string sprite = chunks[8];
		int hp = Utils.IntParseFast(chunks[9]);
		int speedLimit = Utils.IntParseFast(chunks[10]);
		int damage = Utils.IntParseFast(chunks[11]);
		int ttl = Utils.IntParseFast(chunks[12]);
		int fx = Utils.IntParseFast(chunks[13]);
		int fy = Utils.IntParseFast(chunks[14]);
		Actor a = null;
		
		if( actorDict.ContainsKey(uid) )
		{
			a = actorDict[uid];

			if( a != null && a.HP <= 0 )
			{
				actorDict[uid] = null;
				actorList.Remove(a);
				Destroy(a.gameObject);
			}

			if( a != null )
			{
				if( oid != Boss.Instance.localOID )
				{
					a.Mod(true, uid,oid,team,tx,ty,rx,ry,sprite,hp,speedLimit,damage,ttl,fx,fy);	
				}
				else
				{
					a.Mod(false, uid,oid,team,tx,ty,rx,ry,sprite,hp,speedLimit,damage,ttl,fx,fy);	
				}
			}
		}
		else if( hp > 0 )
		{
			a = AddActor(rx,ry,sprite);
			if( a != null )
			{
				a.Mod(true, uid,oid,team,tx,ty,rx,ry,sprite,hp,speedLimit,damage,ttl,fx,fy);
				actorDict[uid] = a;
				actorList.Add(a);

				a.Init();
				if( a.hero )
				{
					a.hero.Init();
				}
				if( a.creature )
				{
					a.creature.Init(sprite);
				}
				Boss.Instance.ScanLocalActors();
			}
		}
		
	}

	 

}
