using UnityEngine;
using System.Collections;
public class GameEffect : MonoBehaviour 
{

	public enum EffectState
	{
		NONE,
		STARTUP,
		ACTIVE,
	}

	public string effName = "-";
	public EffectState effState = EffectState.NONE;
	public float lockTime = -1f;
	public float totalLockTime = -1f;
	public bool IsActive { get { return effState != EffectState.NONE; } } 
	
	public delegate void EffectEvent(GameEffect ga, Actor actor);
	public EffectEvent StartUp;
	public EffectEvent Activate;	
	public EffectEvent Finish;

	void Awake()
	{
		StartUp = Noop;
		Activate = Noop;
		Finish = Noop;
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
	}


	public void Advance(Actor actor)
	{
		switch(effState)
		{
			case EffectState.NONE: 
				effState = EffectState.STARTUP;
				StartUp(this, actor);
				break;
			case EffectState.STARTUP: 
				effState = EffectState.ACTIVE;
				Activate(this, actor);
				break;
			case EffectState.ACTIVE: 
				effState = EffectState.NONE;
				Finish(this, actor);
				break;
		}
	}
	public void Interrupt(Actor actor)
	{
		effState = EffectState.NONE;
		Finish(this, actor);
	}

	public void SetLock(float val)
	{
		lockTime = val;
		totalLockTime = val;
	}

	public static void Noop(GameEffect ge, Actor actor)
	{
		return;
	}
	

	
}
