using UnityEngine;
using System.Collections;
public class GameEffect : MonoBehaviour 
{

	public enum EffectState
	{
		NONE,
		STARTING,
		ACTIVE,
	}

	public char effName = '-';
	public char effStateName = '-';
	private EffectState effState = EffectState.NONE;
	public float tickTime = 0f;
	public float lockTime = -1f;
	public float totalLockTime = -1f;
	public float duration = 0f;
	public bool IsActive { get { return effState != EffectState.NONE; } } 
	private bool isPaused = false;
	
	public delegate void EffectEvent(GameEffect ga, Actor actor);
	public EffectEvent StartUp;
	public EffectEvent Activate;	
	public EffectEvent Finish;

	public delegate void EffectTick(GameEffect ga, Actor actor, float deltaTime);
	public EffectTick Tick;

	void Awake()
	{

	}

	
	public void EffectUpdate(float deltaTime, Actor actor) 
	{

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
		}
		if( !isPaused && IsActive )
		{
			Tick(this, actor, deltaTime);
		}
	}


	public void Advance(Actor actor)
	{
		switch(effState)
		{
			case EffectState.NONE:  	SetState(actor, EffectState.STARTING); break;
			case EffectState.STARTING: 	SetState(actor, EffectState.ACTIVE); break;
			case EffectState.ACTIVE: 	SetState(actor, EffectState.NONE); break;
		}
	}

	public void JumpToState(Actor actor, char stateName)
	{
		int attempts = 0;
		while(effStateName != stateName && attempts++ < 100)
		{
			Advance(actor);
		}
	}

	public void SetState(Actor actor, EffectState nextState)
	{
		if( effState == nextState )
		{
			return;
		}
		effState = nextState;
		switch(effState)
		{
			case EffectState.STARTING: 
				effStateName = 'S';
				tickTime = 0f;
				StartUp(this, actor);
				break;
			case EffectState.ACTIVE: 
				effStateName = 'A';
				Activate(this, actor);
				break;
			case EffectState.NONE: 
				effStateName = '-';
				tickTime = 0f;
				Finish(this, actor);
				break;
		}
	}


	public void Interrupt(Actor actor)
	{
		SetState(actor, EffectState.NONE);
	}

	public void SetDuration(float val)
	{
		duration = val;
	}

	public void SetLock(float val)
	{
		lockTime = val;
		totalLockTime = val;
	}

	public void SetPause(bool val)
	{
		isPaused = val;
	}


}
