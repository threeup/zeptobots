using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour {

	public static Boss Instance;

	public Tile selectedTile = null;
	public Actor selectedActor = null;

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

	public void SelectRandomLocalActor()
	{
		int len = Director.Instance.localActors.Count;
		if( len > 0 )
		{
			if( selectedActor != null )
			{
				selectedActor.Deselect();
			}
			selectedActor = Director.Instance.localActors[Random.Range(0,len)];
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

			string sprite = NetMan.Instance.localIsRed ? "Rd1" : "Bd1";
			NetMan.Instance.Send("requestactoradd|"+localOID+"|"+rtx+"|"+rty+"|"+sprite);
		}
	}

	public void SendUpdate()
	{
		Director.Instance.SendUpdate();
		
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
