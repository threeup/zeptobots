using UnityEngine;
using System.Collections;

public class RulesEffect 
{

	public static void Noop(GameEffect ge, ZeptoObject zo)
	{
		return;
	}

	public static void NoopTick(GameEffect ge, ZeptoObject zo, float deltaTime)
	{
		return;
	}
	
}

public class GlowEff : RulesEffect
{
	static Color glowColor = new Color(1f,1f,0f,0.5f);

	public static void Assign(GameEffect ge, char effName)
	{
		ge.displayName = "Glow";
		ge.showProgress = false;
		ge.effName = effName;
		ge.StartUp = Noop;
		ge.Activate = Activate;
		ge.Finish = Finish;
		ge.Tick = Tick;
	}	

	public static void Tick(GameEffect ge, ZeptoObject zo, float deltaTime)
	{
		ge.tickTime += deltaTime;
		Color startColor = Color.Lerp(zo.body.defaultColor, glowColor, 0.5f);
		Color endColor = zo.body.defaultColor + glowColor;
		Color result = Color.Lerp(startColor, endColor, ge.tickTime/ge.duration);
		zo.body.SetColor( result );

	}

	public static void Activate(GameEffect ge, ZeptoObject zo)
	{
		ge.SetLock(ge.duration);
	}

	public static void Finish(GameEffect ge, ZeptoObject zo)
	{
		zo.body.SetColor(zo.body.defaultColor);	
		ge.SetLock(0f);
	}
}

public class RockEff : RulesEffect
{
	static Color rockColor = new Color(0.5f,0.5f,0.5f,0.5f);

	public static void Assign(GameEffect ge, char effName)
	{
		ge.displayName = "Rock";
		ge.showProgress = false;
		ge.effName = effName;
		ge.StartUp = Noop;
		ge.Activate = Activate;
		ge.Finish = Finish;
		ge.Tick = Tick;
	}

	public static void Tick(GameEffect ge, ZeptoObject zo, float deltaTime)
	{
		ge.tickTime += deltaTime;
		Color startColor = Color.Lerp(rockColor, zo.body.defaultColor, 0.2f);
		Color endColor = Color.Lerp(rockColor, zo.body.defaultColor, 0.8f);
		Color result = Color.Lerp(startColor, endColor, ge.tickTime/ge.duration) ;
		zo.body.SetColor( result );

	}


	public static void Activate(GameEffect ge, ZeptoObject zo)
	{
		zo.body.SetColor(Color.black);
		ge.SetLock(ge.duration);
	}

	public static void Finish(GameEffect ge, ZeptoObject zo)
	{
		zo.body.SetColor(zo.body.defaultColor);	
		ge.SetLock(0f);
	}
}

public class ConquerEff : RulesEffect
{
	static Color conquerColor = new Color(1f,1f,0f,0.5f);

	public static void Assign(GameEffect ge, char effName)
	{
		ge.displayName = "Conquer";
		ge.showProgress = false;
		ge.effName = effName;
		ge.showProgress = true;
		ge.StartUp = Noop;
		ge.Activate = Activate;
		ge.Finish = Finish;
		ge.Tick = Tick;
	}	

	public static void Tick(GameEffect ge, ZeptoObject zo, float deltaTime)
	{
		ge.tickTime += deltaTime;
		zo.body.SetColor( conquerColor );

	}

	public static void Activate(GameEffect ge, ZeptoObject zo)
	{
		ge.SetLock(ge.duration);
	}

	public static void Finish(GameEffect ge, ZeptoObject zo)
	{
		zo.body.SetColor(zo.body.defaultColor);	
		ge.SetLock(0f);
	}
}