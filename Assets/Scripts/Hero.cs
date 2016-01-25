using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

	public Actor actor = null;
	public GameAbility[] abilities;

	public Vector2 inputVec = Vector2.zero;
	public bool inputA = false;
	public bool inputB = false;

	public void Init()
	{
		abilities = new GameAbility[2];
		abilities[0] = this.gameObject.AddComponent<GameAbility>();
		abilities[1] = this.gameObject.AddComponent<GameAbility>();
		ShootAb.Assign(abilities[0]);
		DashAb.Assign(abilities[1]);

		
	}


	
	void Update () 
	{
		float deltaTime = Time.deltaTime;
		Engine engine = actor.engine;
		Body body = actor.body;
		engine.MoveUpdate(deltaTime, inputVec);
		
		abilities[0].ActionUpdate(deltaTime, actor, inputA);
		abilities[1].ActionUpdate(deltaTime, actor, inputB);

		if( body && body.localUpdate )
		{
			body.BodyUpdate(deltaTime, engine.facingVec, engine.currentSpeed, abilities[0].isPressed, abilities[1].isPressed);
		}
	}

	public void Select()
	{
		Hud.Instance.SetPortrait(0, actor.Team == 1 ? Color.red : Color.blue);
		//tint?
	}

	public void Deselect()
	{
		//tint?
	}

}
