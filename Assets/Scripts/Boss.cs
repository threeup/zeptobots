﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour {

	public static Boss Instance;

	public int localOID = -1;
	public bool localIsRed = true;
	public List<Actor> localActors = new List<Actor>();
	public List<Hero> localHeroes = new List<Hero>();
	public Kingdom selectedKingdom = null;
	public Hero selectedHero = null;

	private float sendUpdateTimer = 0.1f;

	void Awake()
	{
		Instance = this;
	}

	public void MessageReceive(string message)
	{
		string[] chunks = message.Split('|');
		switch(chunks[0])
		{
			case "setupclient":
				if( Boss.Instance.localOID > 0 )
				{
					Debug.LogError("Double setup");
					return;
				}
				localOID = Utils.IntParseFast(chunks[1]);
				//Debug.LogError("LocalOID"+Boss.Instance.localOID);
				localIsRed = chunks[2].StartsWith("true");
				break;
			default:
				break;
		}
	}
	
	public void SelectRandomSpawn()
	{
		if( World.Instance.sectorCount == 0 )
		{
			Debug.Log("wait");
			return;
		}
		int attempt = 0;;
		List<Kingdom> kingdoms = null;
		selectedKingdom = null;
		while(selectedKingdom == null && attempt < 100)
		{
			attempt++;
			kingdoms = World.Instance.GetRandomSector().kingdoms;
			for(int i=0; i<kingdoms.Count; ++i)
			{
				if( kingdoms[i].ownerOID < 0 )
				{
					selectedKingdom = kingdoms[i];
					CamControl.Instance.FollowObject(selectedKingdom.transform);
					break;
				}
			}
		}
		
	}

	public void SelectRandomLocalHero()
	{
		int len = localHeroes.Count;
		if( len > 0 )
		{
			if( selectedHero != null )
			{
				selectedHero.Deselect();
			}
			selectedHero = localHeroes[Random.Range(0,len)];
			if( selectedHero != null )
			{
				selectedHero.Select();
				CamControl.Instance.FollowObject(selectedHero.transform);
			}
		}
	}

	public void RequestSpawn() 
	{
		if( selectedKingdom == null )
		{
			SelectRandomSpawn();
		}
		if( selectedKingdom == null )
		{
			Debug.Log("nowhere to spawn");
			return;
		}
		selectedKingdom.RequestKingdom(localOID, localIsRed);
		
	}

	public void SendUpdate()
	{
		for(int i=0; i<localActors.Count; ++i)
		{
			Actor actor = localActors[i];
			actor.PrepOutString();
			NetMan.Instance.Send( actor.outString );
		}
	}

	public void ScanLocalActors()
	{
		localHeroes.Clear();
		localActors.Clear();
		List<Actor> actorList = Director.Instance.ActorList;
		for(int i = 0; i<actorList.Count; ++i)
		{
			Actor a = actorList[i];
			if( a.OID == localOID )
			{
				localActors.Add(a);
				if( a.hero != null )
				{
					localHeroes.Add(a.hero);
				}
				a.SetLocal(true);
			}
			else
			{
				a.SetLocal(false);
			}
		}
	}

	public void Update()
	{
		float deltaTime = Time.deltaTime;
		if( selectedHero == null )
		{
			SelectRandomLocalHero();
		}
		else
		{
			selectedHero.inputVec = Vector2.zero;
			selectedHero.inputA = false;
			selectedHero.inputB = false;
			if (Input.GetKey("up") || Input.GetKey("w"))
			{
				selectedHero.inputVec.y += 1;
			}
			if (Input.GetKey("down")|| Input.GetKey("s"))
			{
				selectedHero.inputVec.y -= 1;
			}
			if (Input.GetKey("left")|| Input.GetKey("a"))
			{
				selectedHero.inputVec.x -= 1;
			}
			if (Input.GetKey("right")|| Input.GetKey("d"))
			{
				selectedHero.inputVec.x += 1;
			}
			if (Input.GetButton("Fire1"))
			{
				selectedHero.inputA = true;
			}
			if (Input.GetButton("Jump"))
			{
				selectedHero.inputB = true;
			}
			for(int i=0; i<2; ++i)
			{
				GameAbility ga = selectedHero.actions[i];
				Hud.Instance.SetButton(i, ga.CooldownPercent, ga.IsActive, ga.isPressed);
			}
		}
		sendUpdateTimer -= deltaTime;
		if( sendUpdateTimer < 0f )
		{
			SendUpdate();
			sendUpdateTimer = 0.25f;
		}

	}


}
