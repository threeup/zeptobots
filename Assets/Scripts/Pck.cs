using UnityEngine;
using System;
using System.Collections;
using WebSocketSharp;

public class Pck : MonoBehaviour {


	public enum ABase
	{
		EMPTY,
		UID,
		OID,
		TEAM,
		VISUAL,
		TX,
		TY,
		HP,
		TOPSPEED,
		DAMAGE,
		COUNT,
	}

	
	public enum AQuick
	{
		TX,
		TY,
		RX,
		RY,
		FX,
		FY,
		HP,
		TOPSPEED,
		DAMAGE,
		TTL,
		ACTIONS,
		EFFECTS,
		COUNT,
	}

	public static void UnpackData(string[] chunks, ref ActorBasicData abd)
	{
		int.TryParse(chunks[(int)ABase.UID], out abd.uid);
		int.TryParse(chunks[(int)ABase.OID], out abd.oid);
		int.TryParse(chunks[(int)ABase.TEAM], out abd.team);
		abd.visualString = chunks[(int)ABase.VISUAL];
		int.TryParse(chunks[(int)ABase.TX], out abd.tx);
		int.TryParse(chunks[(int)ABase.TY], out abd.ty);
		int.TryParse(chunks[(int)ABase.HP], out abd.hp);
		int.TryParse(chunks[(int)ABase.TOPSPEED], out abd.topSpeed);
		int.TryParse(chunks[(int)ABase.DAMAGE], out abd.damage);
		
	}

	public static void UnpackData(string[] chunks, ref ActorQuickData aqd)
	{
		int offset = (int)ABase.COUNT;
		int.TryParse(chunks[offset+(int)AQuick.TX], out aqd.tx);
		int.TryParse(chunks[offset+(int)AQuick.TY], out aqd.ty);
		int.TryParse(chunks[offset+(int)AQuick.RX], out aqd.rx);
		int.TryParse(chunks[offset+(int)AQuick.RY], out aqd.ry);
		int.TryParse(chunks[offset+(int)AQuick.FX], out aqd.fx);
		int.TryParse(chunks[offset+(int)AQuick.FY], out aqd.fy);
		int.TryParse(chunks[offset+(int)AQuick.HP], out aqd.hp);
		int.TryParse(chunks[offset+(int)AQuick.TOPSPEED], out aqd.topSpeed);
		int.TryParse(chunks[offset+(int)AQuick.DAMAGE], out aqd.damage);
		int.TryParse(chunks[offset+(int)AQuick.TTL], out aqd.ttl);

		aqd.actionString = chunks[offset+(int)AQuick.ACTIONS];
		aqd.effectString = chunks[offset+(int)AQuick.EFFECTS];

		
	}


	public static void PackData(System.Text.StringBuilder sb, ActorBasicData abd)
	{
		sb.Append(abd.uid);
		sb.Append("|");
		sb.Append(abd.oid);
		sb.Append("|");
		sb.Append(abd.team);
		sb.Append("|");
		sb.Append(abd.visualString); 
		sb.Append("|");
		sb.Append(abd.tx);
		sb.Append("|");
		sb.Append(abd.ty);
		sb.Append("|");
		sb.Append(abd.hp);
		sb.Append("|");
		sb.Append(abd.topSpeed);
		sb.Append("|");
		sb.Append(abd.damage);
		sb.Append("|");
	}


	public static void PackData(System.Text.StringBuilder sb, ActorQuickData aqd)
	{
		sb.Append(aqd.tx);
		sb.Append("|");
		sb.Append(aqd.ty);
		sb.Append("|");
		sb.Append(aqd.rx);
		sb.Append("|");
		sb.Append(aqd.ry);
		sb.Append("|");
		sb.Append(aqd.fx);
		sb.Append("|");
		sb.Append(aqd.fy);
		sb.Append("|");
		sb.Append(aqd.hp);
		sb.Append("|");
		sb.Append(aqd.topSpeed);
		sb.Append("|");
		sb.Append(aqd.damage);
		sb.Append("|");
		sb.Append(aqd.ttl);
		sb.Append("|");
		sb.Append(aqd.actionString);
		sb.Append("|");
		sb.Append(aqd.effectString);
		sb.Append("|");
	}

	public static void RefreshActorQuickData(System.Text.StringBuilder sb, ref ActorQuickData aqd, Actor actor)
	{
		
		sb.Length = 0;
		if( actor && actor.hero )
		{
			for(int i=0; i<actor.hero.actions.Length; ++i)
			{
				sb.Append(actor.hero.actions[i].abName);
				sb.Append(actor.hero.actions[i].abStateName);
				sb.Append(",");
			}
		}
		aqd.actionString = sb.ToString();
		sb.Length = 0;
		if( actor )
		{
			for(int i=0; i<actor.effects.Length; ++i)
			{
				sb.Append(actor.effects[i].effName);
				sb.Append(actor.effects[i].effStateName);
				sb.Append(",");
			}
		}
		aqd.effectString = sb.ToString();
		sb.Length = 0;
	}

	
}
