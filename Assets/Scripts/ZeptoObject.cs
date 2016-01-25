using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZeptoObject : MonoBehaviour {

	
	public List<char> effectNames = new List<char>();
	public GameEffect[] effects;

	public Engine engine = null;
	public Hero hero = null;
	public Body body = null;
	public Creature creature = null;
	public HealthBar healthBar = null;

	public virtual void Init()
	{
		effects = new GameEffect[2];
		effects[0] = this.gameObject.AddComponent<GameEffect>();
		effects[1] = this.gameObject.AddComponent<GameEffect>();
	}

	public void AddEff(char effName, float duration)
	{
		if( !effectNames.Contains(effName) )
		{
			GameEffect ge = GetVacantEffect();
			if( ge != null )
			{
				effectNames.Add(effName);
				switch(effName)
				{
					case 'C': ConquerEff.Assign(ge, effName); break;
					case 'G': GlowEff.Assign(ge, effName); break;
					case 'R': RockEff.Assign(ge, effName); break;
					default: break;
				}
				ge.SetPause(false);
				ge.SetDuration(duration);
				ge.Advance(this);
			}
			else
			{
				Debug.Log("no vacant");
			}
		}
		else
		{
			Debug.Log("already has "+effName);
		}
	}

	public void PauseEff(char effName)
	{
		if( effectNames.Contains(effName) )
		{
			GameEffect ge = GetNamedEffect(effName);
			if( ge != null )
			{
				ge.SetPause(true);
			}
		}
	}

	public void RemEff(char effName)
	{
		if( effectNames.Contains(effName) )
		{
			GameEffect ge = GetNamedEffect(effName);
			if( ge != null )
			{
				ge.Interrupt(this);
			}
			effectNames.Remove(effName);
		}
	}

	public GameEffect GetVacantEffect()
	{
		for(int i=0; i<effects.Length; ++i)
		{
			if(!effects[i].IsActive )
			{
				return effects[i];
			}
		}
		return null;
	}

	public GameEffect GetNamedEffect(char effName)
	{
		for(int i=0; i<effects.Length; ++i)
		{
			//if(string.Compare(effects[i].effName, effName) == 0)
			if(effects[i].effName == effName)
			{
				return effects[i];
			}
		}
		return null;
	}

}
