using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour {

	public static Boss Instance;

	public Tile selectedTile = null;
	public List<Actor> localActors = new List<Actor>();
	public Actor selectedActor = null;

	private float sendUpdateTimer = 1f;

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
			selectedActor = localActors[Random.Range(0,len)];
			CamControl.Instance.FollowObject(selectedActor.transform);
		}
	}

	public void RequestSpawn() 
	{
		if( selectedTile != null )
		{
			int localOID = NetMan.Instance.localOID;
			int tx = selectedTile.tx;
			int ty = selectedTile.ty;
			//oid, x, y, sprite, hp

			string sprite = NetMan.Instance.localIsRed ? "R" : "B";
			NetMan.Instance.Send("requestactoradd|"+localOID+"|"+tx+"|"+ty+"|"+sprite+"|"+10);
		}
	}

	public void SendUpdate()
	{
		if( selectedActor != null )
		{
			int uid = selectedActor.uid;
			int localOID = NetMan.Instance.localOID;
			int tx = selectedActor.tx;
			int ty = selectedActor.ty;
			int rx = selectedActor.rx;
			int ry = selectedActor.ry;
			string sprite = selectedActor.sprite;
			int hp = selectedActor.hp;
			NetMan.Instance.Send("requestactormod|"+uid+"|"+localOID+"|"+tx+"|"+ty+"|"+rx+"|"+ry+"|"+sprite+"|"+hp);
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
			sendUpdateTimer = 0.1f;
		}

	}


}
