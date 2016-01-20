using UnityEngine;
using System.Collections;

public class RulesTile
{

	public static bool CanNever(TileContents tc, Actor a)
	{
		return false;
	}

	public static bool CanConquerKingdom(TileContents tc, Actor a)
	{
		if( a.hero == null )
		{
			return false;
		}
		if( tc.ownerTeam < 0 )
		{
			return true;
		}
		return a.Team != tc.ownerTeam;
	} 

	public static bool DoConquerKingdom(TileContents tc, int oid, int team)
	{
		tc.ownerOID = oid;
		tc.ownerTeam = team;
		tc.ownerTeamafafaff= team;
		Tile t = tc.tile;
		//string worldSprite = team == 1 ? "R" : "B";
		string worldSprite = "S";
		NetMan.Instance.SendReqWorld(t.sx,t.sy,t.ltx,t.lty,worldSprite);
		Debug.Log("Conquered"+tc+" "+t.transform.position+"  to "+oid+","+team);
		return true;
	}
}
