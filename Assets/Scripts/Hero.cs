using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

	public Actor actor = null;
	public GameAction[] actions;

	public Light spotLight = null;

	public Vector2 inputVec = Vector2.zero;
	public bool inputA = false;
	public bool inputB = false;

	public void Init()
	{
		actions[0].Activate = ShootActivate;
		actions[0].Recover = ShootRecover;
		actions[0].Cooldown = ShootCooldown;

		actions[1].Activate = DashActivate;
		actions[1].Recover = DashRecover;
		actions[1].Cooldown = DashCooldown;
		

	}


	
	void Update () 
	{
		float deltaTime = Time.deltaTime;
		Engine engine = actor.engine;
		Spriter spriter = actor.spriter;
		engine.MoveUpdate(deltaTime, inputVec);
		
		actions[0].ActionUpdate(deltaTime, actor, inputA);
		actions[1].ActionUpdate(deltaTime, actor, inputB);

		if( spriter.localUpdate )
		{
			spriter.SpriteUpdate(deltaTime, engine.facingVec, engine.currentStep);
		}
		float rot_y = Mathf.Atan2(engine.facingVec.y, -engine.facingVec.x) * Mathf.Rad2Deg;
        spotLight.transform.rotation = Quaternion.Euler(30f, rot_y - 90, 0f);
	}

	public void Select()
	{
		//tint?
	}

	public void Deselect()
	{
		//tint?
	}


	public static void DashActivate(GameAction ga, Actor actor)
	{
		float dashDistance = 40f;
		Engine engine = actor.engine;
		Vector3 delta = new Vector3(engine.facingVec.x, 0, engine.facingVec.y)*dashDistance;
		engine.desiredPosition = engine.transform.position + delta;
		engine.speedLimit = 240;
		actor.damage = 3;
		engine.moveLock = true;
		ga.lockTime = 0.6f;
	}

	public static void DashRecover(GameAction ga, Actor actor)
	{
		Engine engine = actor.engine;
		engine.speedLimit = 20;
		actor.damage = 0;
		engine.moveLock = false;
		ga.lockTime = 2.5f;
	}


	public static void DashCooldown(GameAction ga, Actor actor)
	{
		Engine engine = actor.engine;
		engine.speedLimit = 30;
		actor.damage = 1;
		engine.moveLock = false;
		ga.lockTime = 0f;
	}

	public static void ShootActivate(GameAction ga, Actor actor)
	{
		Engine engine = actor.engine;
		engine.speedLimit = 10;
		actor.damage = 0;
		ga.lockTime = 0.30f;
	}

	public static void ShootRecover(GameAction ga, Actor actor)
	{
		Engine engine = actor.engine;
		string shootSprite = "*";
		Utils.Ord ord = Utils.GetOrdFromVector(engine.facingVec);
		shootSprite += ord.ToString();
		NetMan.Instance.Send("requestactoradd|"+actor.oid+"|"+actor.team+"|"+actor.rx+"|"+actor.ry+"|"+shootSprite);

		engine.speedLimit = 30;
		actor.damage = 1;
		ga.lockTime = 0.7f;
	}

	public static void ShootCooldown(GameAction ga, Actor actor)
	{
		ga.lockTime = 0f;
	}

	public static void PunchActivate(GameAction ga, Actor actor)
	{
		float dashDistance = 10f;
		Engine engine = actor.engine;
		Vector3 delta = new Vector3(engine.facingVec.x, 0, engine.facingVec.y)*dashDistance;
		engine.desiredPosition = engine.transform.position + delta;
		engine.speedLimit = 120;
		actor.damage = 4;
		ga.lockTime = 0.30f;
	}

	public static void PunchRecover(GameAction ga, Actor actor)
	{
		Engine engine = actor.engine;
		engine.speedLimit = 30;
		actor.damage = 1;
		ga.lockTime = 0.7f;
	}

	public static void PunchCooldown(GameAction ga, Actor actor)
	{
		
		ga.lockTime = 0f;
	}

}
