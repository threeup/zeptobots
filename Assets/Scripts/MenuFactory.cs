using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuFactory : MonoBehaviour {

	public GameObject frontBtnProto;
	public GameObject abilityProto;
	public GameObject progressWheelProto;
	public GameObject pipbarProto;
	public GameObject pipProto;
	public GameObject portraitProto;

	void Awake()
	{
		frontBtnProto.SetActive(false);
		abilityProto.SetActive(false);
		progressWheelProto.SetActive(false);
		pipbarProto.SetActive(false);
		pipProto.SetActive(false);
		portraitProto.SetActive(false);
	}

	public UIFace AddFrontButton(Transform root, float posX, float posY, string contents)
	{
		GameObject go = GameObject.Instantiate(frontBtnProto);
		go.SetActive(true);
		go.transform.SetParent(root);
		UIFace uf = go.GetComponent<UIFace>();
		uf.content.text = contents;
		RectTransform rt = go.GetComponent<RectTransform>();
		rt.localPosition = new Vector2(posX, posY);
		return uf;
	}

	public UIFace AddAbility(Transform root, float posX, float posY, string label, string contents)
	{
		GameObject go = GameObject.Instantiate(abilityProto);
		go.SetActive(true);
		go.transform.SetParent(root);
		UIFace uf = go.GetComponent<UIFace>();
		uf.label.text = label;
		uf.content.text = contents;
		RectTransform rt = go.GetComponent<RectTransform>();
		rt.localPosition = new Vector2(posX, posY);
		return uf;
	}

	public UIFace AddProgressWheel(Transform root, float posX, float posY)
	{
		GameObject go = GameObject.Instantiate(abilityProto);
		go.SetActive(true);
		go.transform.SetParent(root);
		UIFace uf = go.GetComponent<UIFace>();
		RectTransform rt = go.GetComponent<RectTransform>();
		rt.localPosition = new Vector2(posX, posY);
		return uf;
	}
	public UIFace AddPipbar(Transform root, float posX, float posY, int pipCount)
	{
		GameObject go = GameObject.Instantiate(pipbarProto);
		go.SetActive(true);
		go.transform.SetParent(root);
		UIFace uf = go.GetComponent<UIFace>();
		RectTransform rt = go.GetComponent<RectTransform>();
		rt.localPosition = new Vector2(posX, posY);
		for(int i=0; i<pipCount; ++i)
		{
			GameObject pip = GameObject.Instantiate(pipProto);
			pip.SetActive(true);
			pip.transform.parent = go.transform;
		}
		return uf;
	}
	public UIFace AddPortrait(Transform root, float posX, float posY)
	{
		GameObject go = GameObject.Instantiate(portraitProto);
		go.SetActive(true);
		go.transform.SetParent(root);
		UIFace uf = go.GetComponent<UIFace>();
		RectTransform rt = go.GetComponent<RectTransform>();
		rt.localPosition = new Vector2(posX, posY);
		return uf;
	}
}
