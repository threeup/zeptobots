using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FloatingUI : MonoBehaviour {

	public static FloatingUI Instance;

	
	public MenuFactory factory;
	private UIFace effectProgress;

	//private bool isInitialized = false;

	void Awake()
	{
		Instance = this;

	}
	void Start()
	{
		Transform root = this.transform;
		float bottomY = -200f;
		
		effectProgress = factory.AddProgressWheel(root, 0, bottomY);


		SetActive(false);
		//isInitialized = true;
	}

	public void SetActive(bool val)
	{

		effectProgress.SetActive(val);
	}



	public void SetEffectProgress(string displayName, float? progress)
	{
		if( progress.HasValue)
		{
			effectProgress.SetActive(true);
			effectProgress.fill.fillAmount = progress.Value;
			int rounded = (int)(progress.Value * 100f);
			effectProgress.label.text = displayName;
			effectProgress.content.text = rounded.ToString();
		}
		else
		{
			effectProgress.SetActive(false);
		}
	}


	public void UpdateHero(Hero h)
	{
		GameEffect[] effs = h.actor.effects;
		GameEffect bestEff = null;
		int bestPriority = -1;
		for(int i=0; i<effs.Length; ++i)
		{
			GameEffect eff = effs[i];
			if( eff.showProgress && eff.priority > bestPriority )
			{
				bestEff = eff;
			}
		}
		if( bestEff != null && bestEff.totalLockTime > 0)
		{
			this.SetEffectProgress(bestEff.displayName, bestEff.lockTime/bestEff.totalLockTime);
		}
		else
		{
			this.SetEffectProgress("-",null);
		}
	}

}
