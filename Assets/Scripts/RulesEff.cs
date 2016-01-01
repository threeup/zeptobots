using UnityEngine;
using System.Collections;

public class RulesEffect 
{


}

public class GlowEff : RulesEffect
{

	public static void Assign(GameEffect ge, string effName)
	{
		ge.effName = effName;
		ge.Activate = Activate;
		ge.Finish = Finish;
	}

	public static void Activate(GameEffect ge, Actor actor)
	{
		actor.actorBody.SetColor(Color.yellow);
		ge.SetLock(999f);
	}

	public static void Finish(GameEffect ge, Actor actor)
	{
		actor.actorBody.SetColor(actor.actorBody.defaultColor);	
		ge.SetLock(0f);
	}
}

public class RockEff : RulesEffect
{

	public static void Assign(GameEffect ge, string effName)
	{
		ge.effName = effName;
		ge.Activate = Activate;
		ge.Finish = Finish;
	}

	public static void Activate(GameEffect ge, Actor actor)
	{
		actor.actorBody.SetColor(Color.black);
		ge.SetLock(999f);
	}

	public static void Finish(GameEffect ge, Actor actor)
	{
		actor.actorBody.SetColor(actor.actorBody.defaultColor);	
		ge.SetLock(0f);
	}
}