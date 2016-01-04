using UnityEngine;
using System.Collections;
public class GameAbility : MonoBehaviour 
{

	public enum AbilityState
	{
		NONE,
		STARTING,
		CHARGING,
		ACTIVE,
		RECOVERING,
		DOWNCOOLING,

	}
	public char abName = '-';
	public char abStateName = '-';
	public bool isPressed = false;
	public bool wasPressed = false;
	public AbilityState abState = AbilityState.NONE;
	public float chargeTime = -1f;
	public float lockTime = -1f;
	public float totalLockTime = -1f;
	public bool IsActive { get { return abState != AbilityState.NONE; } }

	public float CooldownPercent { 
		get 
		{
			if( abState == AbilityState.RECOVERING || abState == AbilityState.DOWNCOOLING )
			{
				return lockTime;
			}
			return 0f;
		}
	}
	
	public delegate void AbilityEvent(GameAbility ga, Actor actor);
	public AbilityEvent StartUp;
	public AbilityEvent Charge;
	public AbilityEvent Activate;
	public AbilityEvent Recover;
	public AbilityEvent Cooldown;
	public AbilityEvent Finish;

	public RulesAbility ab = null;

	void Awake()
	{

	}

	
	public void ActionUpdate(float deltaTime, Actor actor, bool isPressed) 
	{
		this.isPressed = isPressed;
		if( abState == AbilityState.NONE && !isPressed )
		{
			return;
		}
		if( abState == AbilityState.CHARGING )
		{
			if( isPressed )
			{
				chargeTime += deltaTime;
			}
			else
			{
				lockTime = 0f;
			}
		}
		
		if( lockTime > 0f )
		{
			lockTime -= deltaTime;
		}
		if( lockTime < 0.001f )
		{
			Advance(actor);
			wasPressed = isPressed;
		}
	}

	public void Advance(Actor actor)
	{
		switch(abState)
		{
			case AbilityState.NONE:  		SetState(actor, AbilityState.STARTING); break;
			case AbilityState.STARTING: 	SetState(actor, AbilityState.CHARGING); break;
			case AbilityState.CHARGING: 	SetState(actor, AbilityState.ACTIVE); break;
			case AbilityState.ACTIVE: 		SetState(actor, AbilityState.RECOVERING); break;
			case AbilityState.RECOVERING: 	SetState(actor, AbilityState.DOWNCOOLING); break;
			case AbilityState.DOWNCOOLING: 	SetState(actor, AbilityState.NONE); break;
		}
	}


	public void JumpToState(Actor actor, char stateName)
	{
		int attempts = 0;
		while(abStateName != stateName && attempts++ < 100)
		{
			Advance(actor);
		}
	}

	public void SetState(Actor actor, AbilityState nextState )
	{
		if( abState == nextState )
		{
			return;
		}
		abState = nextState;
		switch(abState)
		{
			case AbilityState.STARTING: 
				abStateName = 'S';
				chargeTime = 0f;
				StartUp(this, actor);
				break;
			case AbilityState.CHARGING: 
				abStateName = 'C';
				chargeTime = 0f;
				Charge(this, actor);
				break;
			case AbilityState.ACTIVE: 
				abStateName = 'A';
				Activate(this, actor);
				break;
			case AbilityState.RECOVERING: 
				abStateName = 'R';
				Recover(this, actor);
				break;
			case AbilityState.DOWNCOOLING: 
				abStateName = 'D';
				Cooldown(this, actor);
				break;
			case AbilityState.NONE: 
				abStateName = '-';
				Finish(this, actor);
				break;
		}
	}

	public void Interrupt(Actor actor)
	{
		SetState(actor, AbilityState.NONE);
	}

	public void SetLock(float val)
	{
		lockTime = val;
		totalLockTime = val;
	}


	

	
}
