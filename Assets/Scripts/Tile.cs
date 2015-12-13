using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	public enum TileType
	{
		NONE = 0,
		SOLID = 1,
		SPAWN = 2,
		FARM = 3,
	}

	public TileType tileType = TileType.NONE;
	public int ltx;
	public int lty;
	public int rtx;
	public int rty;

	public List<Actor> occupyList = new List<Actor>();

	public void Init(int ltx, int lty, int rtx, int rty)
	{
		this.ltx = ltx;
		this.lty = lty;
		this.rtx = rtx;
		this.rty = rty;
	}

	public bool Passable(Actor actor)
	{
		return tileType != TileType.SOLID || !actor.localInput;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool TryAdd(Actor actor)
	{
		if( Passable(actor) )
		{
			occupyList.Add(actor);
			return true;
		}
		return false;
	}

	public void Remove(Actor actor)
	{
		occupyList.Remove(actor);
	}
}
