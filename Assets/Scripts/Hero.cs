using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

	public Actor actor = null;
	public GameAbility[] actions;

	public Vector2 inputVec = Vector2.zero;
	public bool inputA = false;
	public bool inputB = false;

	public void Init()
	{
		actions = new GameAbility[2];
		actions[0] = this.gameObject.AddComponent<GameAbility>();
		actions[1] = this.gameObject.AddComponent<GameAbility>();
		ShootAb.Assign(actions[0]);
		DashAb.Assign(actions[1]);

		
	}


	
	void Update () 
	{
		float deltaTime = Time.deltaTime;
		Engine engine = actor.engine;
		ActorBody actorBody = actor.actorBody;
		engine.MoveUpdate(deltaTime, inputVec);
		
		actions[0].ActionUpdate(deltaTime, actor, inputA);
		actions[1].ActionUpdate(deltaTime, actor, inputB);

		if( actorBody && actorBody.localUpdate )
		{
			actorBody.BodyUpdate(deltaTime, engine.facingVec, engine.currentSpeed, actions[0].isPressed, actions[1].isPressed);
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
