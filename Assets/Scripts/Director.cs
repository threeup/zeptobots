using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Director : MonoBehaviour {

	public static Director Instance;


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
		World world = World.Instance;

		switch(sprite[0])
		{
			case 'H': prototype = world.protos["actor-mega1"]; break;
			case '~': prototype = world.protos["actor-man1"]; break;
			case 'D': prototype = world.protos["actor-dog1"]; break;
			case '*': prototype = world.protos["actor-bullet"]; break;
			default: break;
		}
		if( prototype )
		{
			Vector3 pos = new Vector3(rx-5, 1f, -ry-5);
			GameObject go = GameObject.Instantiate(prototype, pos, prototype.transform.rotation) as GameObject;
			go.SetActive(true);
			actor = go.GetComponent<Actor>();
		}
		return actor;
	}

	public void ModActor(string[] chunks)
	{
		ActorBasicData abd = new ActorBasicData();
		ActorQuickData aqd = new ActorQuickData();
		
		Pck.UnpackData(chunks, ref abd);
		Pck.UnpackData(chunks, ref aqd);

		Actor a = null;
		
		if( actorDict.ContainsKey(abd.uid) )
		{
			a = actorDict[abd.uid];

			if( a != null && a.HP <= 0 )
			{
				actorDict[abd.uid] = null;
				actorList.Remove(a);
				Destroy(a.gameObject);
			}

			if( a != null )
			{
				if( abd.oid != Boss.Instance.localOID )
				{
					a.ModBase(true, abd);	
					a.ModQuick(true, aqd);	
				}
				else
				{
					a.ModBase(false, abd);
					a.ModQuick(false, aqd);	
				}
			}
		}
		else if( aqd.hp > 0 )
		{
			a = AddActor(aqd.rx,aqd.ry,abd.visualString);
			if( a != null )
			{
				a.ModBase(true, abd);	
				a.ModQuick(true, aqd);	
				a.Init();			
				a.engine.Init();
				if( a.hero ) { a.hero.Init(); }
				if( a.creature ) { a.creature.Init(abd.visualString); }
				actorDict[abd.uid] = a;
				actorList.Add(a);
				Boss.Instance.ScanLocalActors();
			}
		}
		
	}

	 

}
