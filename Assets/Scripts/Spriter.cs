using UnityEngine;
using System.Collections;

public class Spriter : MonoBehaviour {


	public SpriteRenderer sr = null;
	public string spriteString = "RD0";
	public bool localUpdate = false;

	public virtual void SpriteUpdate(float deltaTime, Vector2 facing, float speed)
	{
		
	}

	public virtual void SetSprite(string s)
	{
		spriteString = s;
	}
}
