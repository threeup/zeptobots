using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Hud : MonoBehaviour {

	public static Hud Instance;

	
	public MenuFactory factory;
	private UIFace abilityA;
	private UIFace carouselA;
	private UIFace abilityB;
	private UIFace carouselB;
	private UIFace portrait;

	//private bool isInitialized = false;

	void Awake()
	{
		Instance = this;

	}
	void Start()
	{
		Transform root = this.transform;
		float leftX = -220f;
		float rightX = 220f;
		float bottomY = -200f;

		abilityA = factory.AddAbility(root, rightX-75f, bottomY, "Ctrl", "99");
		carouselA = factory.AddCarousel(root, rightX-75f, bottomY+150f);
		
		abilityB = factory.AddAbility(root, rightX-0f, bottomY, "Space", "99");
		carouselB = factory.AddCarousel(root, rightX-0f, bottomY+150f);
		
		portrait = factory.AddPortrait(root, leftX, bottomY);
		portrait.fill.color = Color.magenta; 
		


		SetActive(false);
		//isInitialized = true;
	}

	public void SetActive(bool val)
	{
		abilityA.SetActive(val);
		carouselA.SetActive(val);
		abilityB.SetActive(val);
		carouselB.SetActive(val);
		portrait.SetActive(val);
	}


	public void SetPortrait(int portraitType, Color color)
	{
		portrait.fill.color = color; 
		bool activePortrait = portraitType >= 0;
		portrait.SetActive(activePortrait);
	}

	public void SetButton(int btnIdx, float cooldown, bool isActive, bool pressed)
	{
		UIFace ab = btnIdx == 0 ? abilityA : abilityB;
		int percVal = (int)(cooldown*10f);

		if(!isActive)
		{
			ab.img.color = pressed ? Color.blue : Color.green;
		}
		else
		{
			ab.img.color = pressed ? Color.red : Color.white;
		}
		if( percVal == 0 )
		{
			ab.content.text = "";
		}
		else
		{
			ab.content.text = percVal.ToString();
		}

	}


}
