using UnityEngine;
using System.Collections;

public class RulesAbility 
{

	public static void Noop(GameAbility ga, Actor actor)
	{
		return;
	}
}

public class DashAb : RulesAbility
{

	public static void Assign(GameAbility ga)
	{
		ga.abName = 'D';
		ga.StartUp = StartUp;
		ga.Charge = Charge;
		ga.Activate = Activate;
		ga.Recover = Recover;
		ga.Cooldown = Cooldown;
		ga.Finish = Noop;
	}
	public static void StartUp(GameAbility ga, Actor actor)
	{
		ga.SetLock(0.1f);
	}
	public static void Charge(GameAbility ga, Actor actor)
	{
		actor.AddEff('G', 2.6f);
		ga.SetLock(2.0f);
	}

	public static void Activate(GameAbility ga, Actor actor)
	{
		actor.PauseEff('G');
		float dashDistance = 40f;
		Engine engine = actor.engine;
		Vector3 delta = new Vector3(engine.facingVec.x, 0, engine.facingVec.y)*dashDistance;
		engine.desiredPosition = engine.transform.position + delta;
		engine.speedLimit = 240;
		actor.Damage = 3;
		engine.moveLock = true;
		ga.SetLock(0.6f);
	}

	public static void Recover(GameAbility ga, Actor actor)
	{
		actor.RemEff('G');
		actor.AddEff('R', 2.5f);
		Engine engine = actor.engine;
		engine.speedLimit = 20;
		actor.Damage = 0;
		engine.moveLock = false;
		ga.SetLock(2.5f);
	}


	public static void Cooldown(GameAbility ga, Actor actor)
	{
		actor.RemEff('R');
		Engine engine = actor.engine;
		engine.speedLimit = 30;
		actor.Damage = 1;
		engine.moveLock = false;
		ga.SetLock(2.0f);
	}
}

public class ShootAb : RulesAbility {

	public static void Assign(GameAbility ga)
	{
		ga.abName = 'S';
		ga.StartUp = Noop;
		ga.Charge = Noop;
		ga.Activate = Activate;
		ga.Recover = Recover;
		ga.Cooldown = Cooldown;
		ga.Finish = Noop;
	}
	public static void Activate(GameAbility ga, Actor actor)
	{
		Engine engine = actor.engine;
		engine.speedLimit = 10;
		actor.Damage = 0;
		ga.SetLock(0.30f);
	}

	public static void Recover(GameAbility ga, Actor actor)
	{
		Engine engine = actor.engine;
		string shootSprite = actor.Team == 1 ? "*R" : "*B";
		int fx = (int)engine.facingVec.x;
		int fy = (int)engine.facingVec.y;
		int rx = actor.RX + fx*2;
		int ry = actor.RY - fy*2;
		int tx = (int)Mathf.Round((rx-5)/10f);
		int ty = (int)Mathf.Round((ry-5)/10f);
		NetMan.Instance.SendReqActor(-1, actor.OID, actor.Team,tx,ty,rx,ry,fx,fy,shootSprite,-1,-1,-1,-1);

		engine.speedLimit = 30;
		actor.Damage = 1;
		ga.SetLock(0.7f);
	}

	public static void Cooldown(GameAbility ga, Actor actor)
	{
		ga.SetLock(0f);
	}
}

public class PunchAb : RulesAbility {

	public static void Assign(GameAbility ga)
	{
		ga.abName = 'P';
		ga.StartUp = Noop;
		ga.Charge = Noop;
		ga.Activate = Activate;
		ga.Recover = Recover;
		ga.Cooldown = Cooldown;
		ga.Finish = Noop;
	}
	public static void Activate(GameAbility ga, Actor actor)
	{
		float dashDistance = 10f;
		Engine engine = actor.engine;
		Vector3 delta = new Vector3(engine.facingVec.x, 0, engine.facingVec.y)*dashDistance;
		engine.desiredPosition = engine.transform.position + delta;
		engine.speedLimit = 120;
		actor.Damage = 4;
		ga.SetLock(0.30f);
	}

	public static void Recover(GameAbility ga, Actor actor)
	{
		Engine engine = actor.engine;
		engine.speedLimit = 30;
		actor.Damage = 1;
		ga.SetLock(0.7f);
	}

	public static void Cooldown(GameAbility ga, Actor actor)
	{
		
		ga.SetLock(0f);
	}

}
