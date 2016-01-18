using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileContents : MonoBehaviour {

	[EnumFlagsAttribute]
	public Tile.TileFlags tFlags = Tile.TileFlags.NONE;

	public void Setup()
	{
		if( (tFlags & Tile.TileFlags.KINGDOM) != 0)
		{
			this.gameObject.AddComponent<Kingdom>();
		}
	}

}
