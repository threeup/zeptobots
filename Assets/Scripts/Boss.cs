using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour {

	public static Boss Instance;

	public Tile selectedTile = null;
	public List<Actor> localActors = new List<Actor>();
	public Actor selectedActor = null;

	private float sendUpdateTimer = 0.25f;

	void Awake()
	{
		Instance = this;
	}
	
	public void SelectRandomSpawn()
	{
		List<Tile> spawnTiles = World.Instance.GetRandomSector().spawnTiles;
		int len = spawnTiles.Count;
		if( len > 0 )
		{
			selectedTile = spawnTiles[Random.Range(0,len)];
			CamControl.Instance.LookAt(selectedTile.transform.position);
		}
	}

	public void SelectRandomLocalActor()
	{
		Director.Instance.GetLocalActors(ref localActors);
		int len = localActors.Count;
		if( len > 0 )
		{
			if( selectedActor != null )
			{
				selectedActor.Deselect();
			}
			selectedActor = localActors[Random.Range(0,len)];
			selectedActor.Select();
			CamControl.Instance.FollowObject(selectedActor.transform);
		}
	}

	public void RequestSpawn() 
	{
		if( selectedTile != null )
		{
			int localOID = NetMan.Instance.localOID;
			int rtx = selectedTile.rtx;
			int rty = selectedTile.rty;
			//oid, x, y, sprite

			string sprite = NetMan.Instance.localIsRed ? "R" : "B";
			NetMan.Instance.Send("requestactoradd|"+localOID+"|"+rtx+"|"+rty+"|"+sprite);
		}
	}

	public void SendUpdate()
	{
		int localOID = NetMan.Instance.localOID;
		Director.Instance.GetLocalActors(ref localActors);
		for(int i=0; i<localActors.Count; ++i)
		{
			Actor actor = localActors[i];
			int uid = actor.uid;
			int tx = actor.tx;
			int ty = actor.ty;
			int rx = actor.rx;
			int ry = actor.ry;
			string sprite = actor.sprite;
			int hp = actor.hp;
			int speed = actor.speed;
			NetMan.Instance.Send("requestactormod|"+uid+"|"+localOID+"|"+tx+"|"+ty+"|"+rx+"|"+ry+"|"+sprite+"|"+hp+"|"+speed+"\n");
		}
	}

	public void Update()
	{
		float deltaTime = Time.deltaTime;
		if( selectedActor == null )
		{
			SelectRandomLocalActor();
		}
		else
		{
			selectedActor.inputVec = Vector2.zero;
			selectedActor.inputA = false;
			selectedActor.inputB = false;
			if (Input.GetKey("up"))
			{
				selectedActor.inputVec.y += 1;
			}
			if (Input.GetKey("down"))
			{
				selectedActor.inputVec.y -= 1;
			}
			if (Input.GetKey("left"))
			{
				selectedActor.inputVec.x -= 1;
			}
			if (Input.GetKey("right"))
			{
				selectedActor.inputVec.x += 1;
			}
			if (Input.GetButton("Fire1"))
			{
				selectedActor.inputA = true;
			}
			if (Input.GetButton("Jump"))
			{
				selectedActor.inputB = true;
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
