using UnityEngine;
using System;
using System.Collections;
using WebSocketSharp;

public class Pck : MonoBehaviour {


	
	public enum Ac
	{
		UID = 1,
		OID = 2,
		TEAM = 3,
		SPRITE = 4,
		HP = 5,
		TX = 6,
		TY = 7,
		RX = 8,
		RY = 9,
		FX = 10,
		FY = 11,
		SPEEDLIMIT = 12,
		DAMAGE = 13,
		TTL = 14,
		ACTIONS = 15,
		EFFECTS = 16,
		
	}

	public static void PackActorData(System.Text.StringBuilder sb, ActorData ad)
	{
		sb.Append(ad.uid);
		sb.Append("|");
		sb.Append(ad.oid);
		sb.Append("|");
		sb.Append(ad.team);
		sb.Append("|");
		sb.Append(ad.spriteString); 
		sb.Append("|");
		sb.Append(ad.hp);
		sb.Append("|");
		sb.Append(ad.tx);
		sb.Append("|");
		sb.Append(ad.ty);
		sb.Append("|");
		sb.Append(ad.rx);
		sb.Append("|");
		sb.Append(ad.ry);
		sb.Append("|");
		sb.Append(ad.fx);
		sb.Append("|");
		sb.Append(ad.fy);
		sb.Append("|");
		sb.Append(ad.speedLimit);
		sb.Append("|");
		sb.Append(ad.damage);
		sb.Append("|");
		sb.Append(ad.ttl);
		sb.Append("|");
	}

	public static void PackActorExtra(System.Text.StringBuilder sb, Actor actor)
	{
		
		if( actor )
		{
			for(int i=0; i<actor.effects.Length; ++i)
			{
				sb.Append(actor.effects[i].effName);
				sb.Append(actor.effects[i].effStateName);
				sb.Append(",");
			}
		}
		sb.Append("|");
		if( actor && actor.hero )
		{
			for(int i=0; i<actor.hero.actions.Length; ++i)
			{
				sb.Append(actor.hero.actions[i].abName);
				sb.Append(actor.hero.actions[i].abStateName);
				sb.Append(",");
			}
		}
		sb.Append("|");
	}

	
}
