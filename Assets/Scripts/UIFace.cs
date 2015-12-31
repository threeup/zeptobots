using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIFace : MonoBehaviour {

	public Button btn;
	public Image img;
	public Image outline;
	public Image fill;
	public Text label;
	public Text content;
	public UIFace[] children;

	public void SetActive(bool val)
	{
		this.gameObject.SetActive(val);
	}
}
