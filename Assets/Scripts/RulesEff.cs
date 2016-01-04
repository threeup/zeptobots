using UnityEngine;
using System.Collections;

public class RulesEffect 
{

	public static void Noop(GameEffect ge, Actor actor)
	{
		return;
	}

	public static void NoopTick(GameEffect ge, Actor actor, float deltaTime)
	{
		return;
	}
	
}

public class GlowEff : RulesEffect
{
	static Color glowColor = new Color(1f,1f,0f,0.5f);

	public static void Assign(GameEffect ge, char effName)
	{
		ge.effName = effName;
		ge.StartUp = Noop;
		ge.Activate = Activate;
		ge.Finish = Finish;
		ge.Tick = Tick;
	}	

	public static void Tick(GameEffect ge, Actor actor, float deltaTime)
	{
		ge.tickTime += deltaTime;
		Color startColor = Color.Lerp(actor.actorBody.defaultColor, glowColor, 0.5f);
		Color endColor = actor.actorBody.defaultColor + glowColor;
		Color result = Color.Lerp(startColor, endColor, ge.tickTime/ge.duration);
		actor.actorBody.SetColor( result );

	}

	public static void Activate(GameEffect ge, Actor actor)
	{
		ge.SetLock(ge.duration);
	}

	public static void Finish(GameEffect ge, Actor actor)
	{
		actor.actorBody.SetColor(actor.actorBody.defaultColor);	
		ge.SetLock(0f);
	}
}

public class RockEff : RulesEffect
{
	static Color rockColor = new Color(0.5f,0.5f,0.5f,0.5f);

	public static void Assign(GameEffect ge, char effName)
	{
		ge.effName = effName;
		ge.StartUp = Noop;
		ge.Activate = Activate;
		ge.Finish = Finish;
		ge.Tick = Tick;
	}

	public static void Tick(GameEffect ge, Actor actor, float deltaTime)
	{
		ge.tickTime += deltaTime;
		Color startColor = Color.Lerp(rockColor, actor.actorBody.defaultColor, 0.2f);
		Color endColor = Color.Lerp(rockColor, actor.actorBody.defaultColor, 0.8f);
		Color result = Color.Lerp(startColor, endColor, ge.tickTime/ge.duration) ;
		actor.actorBody.SetColor( result );

	}


	public static void Activate(GameEffect ge, Actor actor)
	{
		actor.actorBody.SetColor(Color.black);
		ge.SetLock(ge.duration);
	}

	public static void Finish(GameEffect ge, Actor actor)
	{
		actor.actorBody.SetColor(actor.actorBody.defaultColor);	
		ge.SetLock(0f);
	}
}