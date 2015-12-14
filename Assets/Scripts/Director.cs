﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Director : MonoBehaviour {

	public static Director Instance;
	
	public GameObject protoActorR;
	public GameObject protoActorB;

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
			case 'R': prototype = protoActorR; break;
			case 'B': prototype = protoActorB; break;
			default: break;
		}
		if( prototype )
		{
			Vector3 pos = new Vector3(rx-5, 1f, -ry-5);
			GameObject go = GameObject.Instantiate(prototype, pos, prototype.transform.rotation) as GameObject;
			go.SetActive(true);
			actor = go.GetComponent<Actor>();
			actor.hero.actions = go.GetComponents<GameAction>();
			actor.hero.Init();
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
		int speed = Utils.IntParseFast(chunks[10]);
		if( !actorDict.ContainsKey(uid) )
		{
			Actor a = AddActor(rx,ry,sprite);
			if( a != null )
			{
				a.Mod(true, uid,oid,team,tx,ty,rx,ry,sprite,hp,speed);
				actorDict[uid] = a;
				actorList.Add(a);
				Boss.Instance.ScanLocalActors();
			}
		}
		else
		{
			Actor a = actorDict[uid];
			if( oid != Boss.Instance.localOID )
			{
				a.Mod(true, uid,oid,team,tx,ty,rx,ry,sprite,hp,speed);	
			}
			else
			{
				a.Mod(false, uid,oid,team,tx,ty,rx,ry,sprite,hp,speed);		
			}
		}
	}

	 

}
