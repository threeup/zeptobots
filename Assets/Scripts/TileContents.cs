using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileContents : ZeptoObject {

	public int ownerOID = -2;
	public int ownerTeam = -1;
	public int ownerTeamafafaff = -5;

	public int TX { get { return tile.rtx; } }
	public int TY { get { return tile.rty; } }

	[EnumFlagsAttribute]
	public Tile.TileFlags tFlags = Tile.TileFlags.NONE;

	public Tile tile;

	[SerializeField]
	protected List<Actor> affectedList = new List<Actor>();

	protected delegate bool CheckActorDelegate(TileContents t, Actor a);
	protected CheckActorDelegate IsAffected;
	
	protected delegate bool ConquerDelegate(TileContents t, int oid, int team);
	protected ConquerDelegate OnConquer;

	public virtual void Setup()
	{
		tile = this.transform.parent.GetComponent<Tile>();
		IsAffected = RulesTile.CanNever;
	}

	public virtual void Update()
	{

	}

	public virtual void OnActorEnter(Actor a)
	{
		if( IsAffected(this, a) )
		{
			affectedList.Add(a);
			OnAffectedChange();
		}
	}

	public virtual void OnActorExit(Actor a)
	{
		if( affectedList.Contains(a) )
		{
			affectedList.Remove(a);
			OnAffectedChange();
		}
	}

	public virtual void OnAffectedChange()
	{
	}

	protected static bool AlwaysPass(Actor a)
	{
		return true;
	}

	public void FlushAffected()
	{
		affectedList.Clear();
		for(int i=0; i<tile.occupyList.Count; ++i)
		{
			OnActorEnter(tile.occupyList[i]);
		}	
		OnAffectedChange();
	}

}
