using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour {

	public static Boss Instance;

	public int localOID = -1;
	public bool localIsRed = true;
	public List<Actor> localActors = new List<Actor>();
	public List<Hero> localHeroes = new List<Hero>();
	public Tile selectedTile = null;
	public Hero selectedHero = null;

	private float sendUpdateTimer = 0.1f;

	void Awake()
	{
		Instance = this;
	}
	
	public void SelectRandomSpawn()
	{
		int len = 0;
		int attempt = 0;;
		List<Tile> spawnTiles = null;
		while(len == 0 && attempt < 100)
		{
			attempt++;
			spawnTiles = World.Instance.GetRandomSector().spawnTiles;
			len = spawnTiles.Count;
		}
		if( len > 0 )
		{
			selectedTile = spawnTiles[Random.Range(0,len)];
			CamControl.Instance.LookAt(selectedTile.transform.position);
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
			selectedHero.Select();
			CamControl.Instance.FollowObject(selectedHero.transform);
		}
	}

	public void RequestSpawn() 
	{
		if( selectedTile != null )
		{
			int rtx = selectedTile.rtx;
			int rty = selectedTile.rty;
			//oid, x, y, sprite

			string sprite = localIsRed ? "Rd1" : "Bd1";
			NetMan.Instance.Send("requestactoradd|"+localOID+"|"+rtx+"|"+rty+"|"+sprite);
			Debug.Log("request spawn"+rtx+" "+rty+" "+sprite);
		}
	}

	public void SendUpdate()
	{
		for(int i=0; i<localActors.Count; ++i)
		{
			Actor actor = localActors[i];
			string str = actor.GetOutString(localOID);
			NetMan.Instance.Send(str);
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
			if( a.oid == localOID )
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
			if (Input.GetKey("up"))
			{
				selectedHero.inputVec.y += 1;
			}
			if (Input.GetKey("down"))
			{
				selectedHero.inputVec.y -= 1;
			}
			if (Input.GetKey("left"))
			{
				selectedHero.inputVec.x -= 1;
			}
			if (Input.GetKey("right"))
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
		}
		sendUpdateTimer -= deltaTime;
		if( sendUpdateTimer < 0f )
		{
			SendUpdate();
			sendUpdateTimer = 0.25f;
		}

	}


}
