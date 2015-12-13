using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

	public Actor actor = null;
	public Engine engine = null;
	public GameAction[] actions;

	public Spriter spriter = null;
	public Light spotLight = null;

	public Vector2 inputVec = Vector2.zero;
	public bool inputA = false;
	public bool inputB = false;

	public void Init()
	{
		actions[0].Activate = AttackActivate;
		actions[0].Cooldown = AttackCooldown;

		actions[1].Activate = DashActivate;
		actions[1].Recover = DashRecover;
		actions[1].Cooldown = DashCooldown;
		

	}


	
	void Update () 
	{
		float deltaTime = Time.deltaTime;
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
		engine.moveLock = true;
		ga.lockTime = 0.6f;
	}

	public static void DashRecover(GameAction ga, Actor actor)
	{
		Engine engine = actor.engine;
		engine.speedLimit = 20;
		engine.moveLock = false;
		ga.lockTime = 1f;
	}


	public static void DashCooldown(GameAction ga, Actor actor)
	{
		Engine engine = actor.engine;
		engine.speedLimit = 30;
		engine.moveLock = false;
		ga.lockTime = 0f;
	}

	public static void AttackActivate(GameAction ga, Actor actor)
	{
		
	}

	public static void AttackCooldown(GameAction ga, Actor actor)
	{
		
	}

}
