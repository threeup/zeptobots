using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HudCarousel : MonoBehaviour {

	public MenuFactory factory;
	private UIFace selected;
	private UIFace left;
	private UIFace right;

	//private bool isInitialized = false;

	void Awake()
	{

	}
	public void Setup(MenuFactory factory)
	{
		this.factory = factory;
		Transform root = this.transform;


		selected = factory.AddSelectable(root, 0, 0, "S");
		left = factory.AddSelectable(root, -60f, 20, "L");
		left.btn.onClick.AddListener(CycleLeft);
		right = factory.AddSelectable(root, 60f, 20, "R");
		right.btn.onClick.AddListener(CycleRight);
		SetActive(false);
		//isInitialized = true;
	}

	public void SetActive(bool val)
	{
		selected.SetActive(val);
		left.SetActive(val);
		right.SetActive(val);
	}



	public void CycleLeft()
	{

	}	

	public void CycleRight()
	{
		
	}	




}
