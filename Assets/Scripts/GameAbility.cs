using UnityEngine;
using System.Collections;
public class GameAbility : MonoBehaviour 
{

	public enum AbilityState
	{
		NONE,
		STARTUP,
		ACTIVE,
		RECOVERY,
		COOLDOWN,

	}
	public string abName = "-";
	public bool isPressed = false;
	public bool wasPressed = false;
	public AbilityState abState = AbilityState.NONE;
	public float lockTime = -1f;
	public float totalLockTime = -1f;
	public bool IsActive { get { return abState != AbilityState.NONE; } }

	public float CooldownPercent { 
		get 
		{
			if( abState == AbilityState.RECOVERY )
			{
				return lockTime;
			}
			return 0f;
		}
	}
	
	public delegate void AbilityEvent(GameAbility ga, Actor actor);
	public AbilityEvent StartUp;
	public AbilityEvent Activate;
	public AbilityEvent Recover;
	public AbilityEvent Cooldown;
	public AbilityEvent Finish;

	void Awake()
	{
		StartUp = Noop;
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
			if( IsActive )
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
		switch(abState)
		{
			case AbilityState.NONE: 
				abState = AbilityState.STARTUP;
				StartUp(this, actor);
				break;
			case AbilityState.STARTUP: 
				abState = AbilityState.ACTIVE;
				Activate(this, actor);
				break;
			case AbilityState.ACTIVE: 
				abState = AbilityState.RECOVERY;
				Recover(this, actor);
				break;
			case AbilityState.RECOVERY: 
				abState = AbilityState.COOLDOWN;
				Cooldown(this, actor);
				break;
			case AbilityState.COOLDOWN: 
				abState = AbilityState.NONE;
				Finish(this, actor);
				break;
		}
	}

	public void Interrupt(Actor actor)
	{
		abState = AbilityState.NONE;
		Finish(this, actor);
	}

	public void SetLock(float val)
	{
		lockTime = val;
		totalLockTime = val;
	}

	public static void Noop(GameAbility ga, Actor actor)
	{
		return;
	}
	

	
}
