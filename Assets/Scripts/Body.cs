using UnityEngine;
using System.Collections;

public class Body : MonoBehaviour {

	public Material flatMaterial;
	public Material tintedMaterial;
	public Light[] coloredLights = null;
	public Renderer[] allRenderers = null;
	public Renderer[] coloredRenderers = null;
	public string spriteString = "HR";
	public bool localUpdate = false;
	public Color defaultColor = Color.white;
	protected Transform thisTransform;

	protected virtual void Awake()
	{
		thisTransform = this.transform;
		for(int i=0; i<allRenderers.Length; ++i)
		{
			allRenderers[i].material = flatMaterial;
		}
		for(int i=0; i<coloredRenderers.Length; ++i)
		{
			coloredRenderers[i].material = flatMaterial;
		}
	}
	public virtual void BodyUpdate(float deltaTime, Vector2 facing, float speed, bool larmSwing, bool rarmSwing)
	{
		float rot_y = Mathf.Atan2(facing.y, -facing.x) * Mathf.Rad2Deg;
        thisTransform.rotation = Quaternion.Euler(0f, rot_y - 90, 0f);
	}

	public virtual void SetSprite(string s)
	{
		spriteString = s;
		if( spriteString[1] == 'R')
		{
			defaultColor = Color.red;
		}
		if( spriteString[1] == 'B')
		{
			defaultColor = Color.blue;
		}
		SetColor(defaultColor);
	}

	public void SetColor(Color c)
	{
		for(int i=0; i<coloredRenderers.Length; ++i)
		{
			coloredRenderers[i].material.color = c;
		}
		for(int i=0; i<coloredLights.Length; ++i)
		{
			coloredLights[i].color = c;
		}
	}
}
