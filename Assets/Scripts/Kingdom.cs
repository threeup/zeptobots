using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Kingdom : TileContents {

	private float conquerTimer = 5f;


	public override void Setup()
	{
		base.Setup();
		tFlags |= Tile.TileFlags.KINGDOM;
		IsAffected = RulesTile.CanConquerKingdom;
		OnConquer = RulesTile.DoConquerKingdom;
	}


	

	public override void Update()
	{
		base.Update();
		float deltaTime = Time.deltaTime;

		ConquerUpdate(deltaTime);
	}

	void ConquerUpdate(float deltaTime)
	{
		if( conquerTimer > 0f )
		{
			conquerTimer -= deltaTime;
			if( conquerTimer <= 0f )
			{
				conquerTimer = -1f;
				if( affectedList.Count > 0 )
				{
					Actor a = affectedList[0];
					if( ownerTeam != a.Team )
					{
						OnConquer(this, a.OID, a.Team );
						RefreshAffected();
					}
				}
			}
		}
	}

	public override void OnAffectedChange()
	{
		if( affectedList.Count == 1)
		{
			conquerTimer = 5f;
			Debug.Log("Conquer Start"+this+" "+this.transform.position);
		}
		else
		{
			conquerTimer = -1f;
		}
	}
}
