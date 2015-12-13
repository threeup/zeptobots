using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public enum TileType
	{
		NONE,
		SOLID,
		SPAWN,
	}

	public TileType tileType = TileType.NONE;
	public int ltx;
	public int lty;
	public int rtx;
	public int rty;

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
}
