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
			if( actor.hero )
			{
				actor.hero.actions = go.GetComponents<GameAbility>();
			}
			
		}
		return actor;
	}

	public void ModActor(string[] chunks)
	{
		int uid = Utils.IntParseFast(chunks[(int)Pck.Ac.UID]);
		int oid = Utils.IntParseFast(chunks[(int)Pck.Ac.OID]);
		int hp = Utils.IntParseFast(chunks[(int)Pck.Ac.HP]);
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
					a.Mod(true, chunks);	
				}
				else
				{
					a.Mod(false, chunks);	
				}
			}
		}
		else if( hp > 0 )
		{
			int rx = Utils.IntParseFast(chunks[(int)Pck.Ac.RX]);
			int ry = Utils.IntParseFast(chunks[(int)Pck.Ac.RY]);
			string sprite = chunks[(int)Pck.Ac.SPRITE];
			a = AddActor(rx,ry,sprite);
			if( a != null )
			{
				a.Mod(true, chunks);
				a.Init(sprite);			
				actorDict[uid] = a;
				actorList.Add(a);
				Boss.Instance.ScanLocalActors();
			}
		}
		
	}

	 

}
