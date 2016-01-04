using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Kingdom : MonoBehaviour {

	public int ownerOID = -1;
	public int ownerTeam = -1;

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
		ownerTeam = localIsRed ? 1 : 0;

		ActorData ad = new ActorData(localIsRed ? "HR" : "HB");
		ad.oid = localOID;
		ad.team = ownerTeam;
		ad.tx = tile.rtx;
		ad.ty = tile.rty;
		ad.rx = ad.tx*10+5;
		ad.ry = ad.ty*10-5;
		ad.fx = 0;
		ad.fy = -1;

		NetMan.Instance.SendReqActor(ad);
		string worldSprite = localIsRed ? "R" : "B";
		NetMan.Instance.SendReqWorld(tile.sx,tile.sy,tile.ltx,tile.lty,worldSprite);
	}

	public void ConquerKingdom(int localOID, int team)
	{
		ownerOID = localOID;
		ownerTeam = team;
		string worldSprite = ownerTeam == 1 ? "R" : "B";
		NetMan.Instance.SendReqWorld(tile.sx,tile.sy,tile.ltx,tile.lty,worldSprite);
	}

	public void Update()
	{
		if( ownerOID > 0 )
		{
			spawnTimer -= Time.deltaTime;
			if( spawnTimer <= 0f)
			{
				spawnTimer = 20f;
				ActorData ad = new ActorData(ownerTeam==1 ? "DR" : "DB");
				ad.oid = ownerOID;
				ad.team = ownerTeam;
				ad.tx = tile.rtx;
				ad.ty = tile.rty;
				ad.rx = ad.tx*10+5;
				ad.ry = ad.ty*10-5;
				ad.fx = 0;
				ad.fy = -1;
				NetMan.Instance.SendReqActor(ad);
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
					if( ownerTeam != a.Team )
					{
						ConquerKingdom( a.OID, a.Team );
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
