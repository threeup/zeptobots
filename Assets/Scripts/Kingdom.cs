using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Kingdom : MonoBehaviour {

	public int ownerOID = -1;
	public int team = -1;

	public Tile tile;

	private float spawnTimer = 5f;
	private float conquerTimer = 5f;

	private List<Actor> conquerList = new List<Actor>();


	void Start()
	{
		tile = this.transform.parent.GetComponent<Tile>();
	}

	public void RequestKingdom(int localOID, bool localIsRed)
	{

		ownerOID = localOID;
		team = localIsRed ? 1 : 0;

		string actorSprite = localIsRed ? "Rd1" : "Bd1";
		NetMan.Instance.Send("requestactoradd|"+ownerOID+"|"+team+"|"+tile.rtx+"|"+tile.rty+"|"+actorSprite);
		string worldSprite = localIsRed ? "R" : "B";
		NetMan.Instance.Send("requestworldmod|"+tile.sx+"|"+tile.sy+"|"+tile.ltx+"|"+tile.lty+"|"+worldSprite);
	}

	public void ConquerKingdom(int localOID, int team)
	{
		ownerOID = localOID;
		this.team = team;
		string worldSprite = team == 1 ? "R" : "B";
		NetMan.Instance.Send("requestworldmod|"+tile.sx+"|"+tile.sy+"|"+tile.ltx+"|"+tile.lty+"|"+worldSprite);
	}

	public void Update()
	{
		if( ownerOID > 0 )
		{
			spawnTimer -= Time.deltaTime;
			if( spawnTimer <= 0f)
			{
				spawnTimer = 20f;
				string actorSprite = "H";
				NetMan.Instance.Send("requestactoradd|"+ownerOID+"|"+team+"|"+tile.rtx+"|"+tile.rty+"|"+actorSprite);
			}
		}
		if( conquerTimer > 0f )
		{
			conquerTimer -= Time.deltaTime;
			if( conquerTimer <= 0f )
			{
				conquerTimer = -1f;
				if( conquerList.Count > 0 )
				{
					Actor a = conquerList[0];
					if( team != a.team )
					{
						ConquerKingdom( a.oid, a.team );
					}
				}
			}
		}
	}

	public void OnActorAdded(Actor a)
	{
		conquerList.Add(a);
		CheckSingle();
	}
	public void OnActorRemoved(Actor a)
	{
		conquerList.Remove(a);
		CheckSingle();
	}


	void CheckSingle()
	{
		if( conquerList.Count == 1)
		{
			conquerTimer = 5f;
		}
		else
		{
			conquerTimer = -1f;
		}
	}
}
