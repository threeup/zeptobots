using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Kingdom : TileContents {

	private float conquerTimer = -1f;
	private float conquerDuration = 2.5f;

	private Actor conqueringActor = null;


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
						FlushAffected();
					}
				}
			}
		}
	}

	public override void OnAffectedChange()
	{
		if( affectedList.Count == 1)
		{
			if( affectedList[0] != conqueringActor )
			{
				conqueringActor = affectedList[0];
				conquerTimer = conquerDuration;
				conqueringActor.AddEff('C', conquerDuration);
			}
		}
		else
		{
			if( conqueringActor != null )
			{
				conqueringActor.RemEff('C');
				conqueringActor = null;
			}
			conquerTimer = -1f;
		}
	}
}
