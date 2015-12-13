using UnityEngine;
using System.Collections;

public class GameAction : MonoBehaviour {

	public enum ActionState
	{
		NONE,
		ACTIVE,
		RECOVERY,
		COOLDOWN,

	}
	public bool isPressed = false;
	public bool wasPressed = false;
	public ActionState aState = ActionState.NONE;
	public float lockTime = -1f;
	
	public delegate void ActionEvent(GameAction ga, Actor actor);
	public ActionEvent Activate;
	public ActionEvent Recover;
	public ActionEvent Cooldown;
	public ActionEvent Finish;

	void Awake()
	{
		Activate = Noop;
		Recover = Noop;
		Cooldown = Noop;
		Finish = Noop;
	}

	
	public void ActionUpdate(float deltaTime, Actor actor, bool isPressed) 
	{
		this.isPressed = isPressed;
		
		if( lockTime > 0f)
		{
			lockTime -= deltaTime;
		}
		if( lockTime < 0.001f )
		{
			if( aState != ActionState.NONE )
			{
				Advance(actor);
			}
			else if( isPressed )
			{
			 	Advance(actor);
			}
			wasPressed = isPressed;
		}
	}


	void Advance(Actor actor)
	{
		switch(aState)
		{
			case ActionState.NONE: 
				aState = ActionState.ACTIVE;
				Activate(this, actor);
				break;
			case ActionState.ACTIVE: 
				aState = ActionState.RECOVERY;
				Recover(this, actor);
				break;
			case ActionState.RECOVERY: 
				aState = ActionState.COOLDOWN;
				Cooldown(this, actor);
				break;
			case ActionState.COOLDOWN: 
				aState = ActionState.NONE;
				Finish(this, actor);
				break;
		}
	}

	public static void Noop(GameAction ga, Actor actor)
	{
		return;
	}
	

	
}
