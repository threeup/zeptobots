using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuFactory : MonoBehaviour {

	public GameObject frontBtnProto;
	public GameObject smallBtnProto;
	public GameObject abilityProto;
	public GameObject progressWheelProto;
	public GameObject pipbarProto;
	public GameObject pipProto;
	public GameObject portraitProto;
	public GameObject carouselProto;

	void Awake()
	{
		foreach(Transform t in this.transform)
		{
			t.gameObject.SetActive(false);
		}
		
	}

	public UIFace AddFrontButton(Transform root, float posX, float posY, string contents)
	{
		GameObject go = GameObject.Instantiate(frontBtnProto);
		go.transform.SetParent(root);
		UIFace uf = go.GetComponent<UIFace>();
		uf.content.text = contents;
		RectTransform rt = go.GetComponent<RectTransform>();
		rt.localPosition = new Vector2(posX, posY);
		go.SetActive(true);
		return uf;
	}

	public UIFace AddAbility(Transform root, float posX, float posY, string label, string contents)
	{
		GameObject go = GameObject.Instantiate(abilityProto);
		go.transform.SetParent(root);
		UIFace uf = go.GetComponent<UIFace>();
		uf.label.text = label;
		uf.content.text = contents;
		RectTransform rt = go.GetComponent<RectTransform>();
		rt.localPosition = new Vector2(posX, posY);
		go.SetActive(true);
		return uf;
	}

	public UIFace AddProgressWheel(Transform root, float posX, float posY)
	{
		GameObject go = GameObject.Instantiate(abilityProto);
		go.transform.SetParent(root);
		UIFace uf = go.GetComponent<UIFace>();
		RectTransform rt = go.GetComponent<RectTransform>();
		rt.localPosition = new Vector2(posX, posY);
		go.SetActive(true);
		return uf;
	}
	public UIFace AddPipbar(Transform root, float posX, float posY, int pipCount)
	{
		GameObject go = GameObject.Instantiate(pipbarProto);
		go.transform.SetParent(root);
		UIFace uf = go.GetComponent<UIFace>();
		RectTransform rt = go.GetComponent<RectTransform>();
		rt.localPosition = new Vector2(posX, posY);
		go.SetActive(true);
		for(int i=0; i<pipCount; ++i)
		{
			GameObject pip = GameObject.Instantiate(pipProto);
			pip.transform.parent = go.transform;
			pip.SetActive(true);
		}
		return uf;
	}
	public UIFace AddPortrait(Transform root, float posX, float posY)
	{
		GameObject go = GameObject.Instantiate(portraitProto);
		go.transform.SetParent(root);
		UIFace uf = go.GetComponent<UIFace>();
		RectTransform rt = go.GetComponent<RectTransform>();
		rt.localPosition = new Vector2(posX, posY);
		go.SetActive(true);
		return uf;
	}

	public UIFace AddCarousel(Transform root, float posX, float posY)
	{
		GameObject go =  GameObject.Instantiate(carouselProto);
		HudCarousel hc = go.GetComponent<HudCarousel>();
		hc.Setup(this);
		go.transform.SetParent(root);
		UIFace uf = go.GetComponent<UIFace>();
		RectTransform rt = go.GetComponent<RectTransform>();
		rt.localPosition = new Vector2(posX, posY);
		hc.SetActive(true);
		return uf;
	}

	public UIFace AddSelectable(Transform root, float posX, float posY, string contents)
	{
		GameObject go = GameObject.Instantiate(smallBtnProto);
		go.transform.SetParent(root);
		UIFace uf = go.GetComponent<UIFace>();
		uf.content.text = contents;
		RectTransform rt = go.GetComponent<RectTransform>();
		rt.localPosition = new Vector2(posX, posY);
		go.SetActive(true);
		return uf;
	}


}
