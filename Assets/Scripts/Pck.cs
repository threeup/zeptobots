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
		TX = 5,
		TY = 6,
		RX = 7,
		RY = 8,
		FX = 9,
		FY = 10,
		HP = 11,
		DEFAULTHP = 12,
		CURRENTSPEEDLIMIT = 13,
		DEFAULTSPEEDLIMIT = 14,
		DAMAGE = 15,
		TTL = 16,
		ACTIONS = 17,
		EFFECTS = 18,
		
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
		sb.Append(ad.hp);
		sb.Append("|");
		sb.Append(ad.defaulthp);
		sb.Append("|");
		sb.Append(ad.currentSpeedLimit);
		sb.Append("|");
		sb.Append(ad.defaultSpeedLimit);
		sb.Append("|");
		sb.Append(ad.damage);
		sb.Append("|");
		sb.Append(ad.ttl);
		sb.Append("|");
		sb.Append(ad.actionString);
		sb.Append("|");
		sb.Append(ad.effectString);
		sb.Append("|");
	}

	public static void DynamicActorData(System.Text.StringBuilder sb, ref ActorData ad, Actor actor)
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
		ad.actionString = sb.ToString();
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
		ad.effectString = sb.ToString();
		sb.Length = 0;
	}

	
}
