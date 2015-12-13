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
	public int tx;
	public int ty;

	public void Init(int tx, int ty)
	{
		this.tx = tx;
		this.ty = ty;
	}

	public bool Passable(Actor actor)
	{
		return tileType != TileType.SOLID;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
