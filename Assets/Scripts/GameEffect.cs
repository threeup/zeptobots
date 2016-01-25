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


	public string displayName = "-";
	public char effName = '-';
	public char effStateName = '-';
	private EffectState effState = EffectState.NONE;
	public float tickTime = 0f;
	public float lockTime = -1f;
	public float totalLockTime = -1f;
	public float duration = 0f;
	public bool IsActive { get { return effState != EffectState.NONE; } } 
	private bool isPaused = false;
	public bool showProgress = false;
	public int priority = 5;
	
	public delegate void EffectEvent(GameEffect ge, ZeptoObject zo);
	public EffectEvent StartUp;
	public EffectEvent Activate;	
	public EffectEvent Finish;

	public delegate void EffectTick(GameEffect ge, ZeptoObject zo, float deltaTime);
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


	public void Advance(ZeptoObject zo)
	{
		switch(effState)
		{
			case EffectState.NONE:  	SetState(zo, EffectState.STARTING); break;
			case EffectState.STARTING: 	SetState(zo, EffectState.ACTIVE); break;
			case EffectState.ACTIVE: 	SetState(zo, EffectState.NONE); break;
		}
	}

	public void JumpToState(ZeptoObject zo, char stateName)
	{
		int attempts = 0;
		while(effStateName != stateName && attempts++ < 100)
		{
			Advance(zo);
		}
	}

	public void SetState(ZeptoObject zo, EffectState nextState)
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
				StartUp(this, zo);
				break;
			case EffectState.ACTIVE: 
				effStateName = 'A';
				Activate(this, zo);
				break;
			case EffectState.NONE: 
				effStateName = '-';
				tickTime = 0f;
				Finish(this, zo);
				break;
		}
	}


	public void Interrupt(ZeptoObject zo)
	{
		SetState(zo, EffectState.NONE);
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
