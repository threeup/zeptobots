using UnityEngine;
using System.Collections;

public class SpriterHero : Spriter {

	public Sprite[] runLeft = new Sprite[4];
	public Sprite[] runRight = new Sprite[4];
	public Sprite[] runUp = new Sprite[4];
	public Sprite[] runDown = new Sprite[4];

	public enum SpriteSet
	{
		LEFT,
		RIGHT,
		UP,
		DOWN,
	}
	private SpriteSet lastSet = SpriteSet.DOWN;
	private int setIndex = 1;
	private float timer = 1f;

	public override void SpriteUpdate(float deltaTime, Vector2 facing, float speed)
	{
		SpriteSet currentSet = SpriteSet.DOWN;
		if( facing.y > 0.1f )
		{
			currentSet = SpriteSet.UP;
		}
		else if( facing.y < -0.1f )
		{
			currentSet = SpriteSet.DOWN;
		}
		else if( facing.x > 0.1f )
		{
			currentSet = SpriteSet.RIGHT;
		}
		else if( facing.x < -0.1f )
		{
			currentSet = SpriteSet.LEFT;
		}
		if( speed < 0.01f )
		{
			//stopped = true;
			SetSprite(currentSet,1);
		}
		else
		{
			if( currentSet == lastSet )
			{
				timer -= deltaTime;
				if( timer < 0f )
				{
					timer = 0.1f/speed;
					if( setIndex == 3 )
					{
						setIndex = 0;
					}
					else
					{
						setIndex++;
					}
					SetSprite(currentSet,setIndex);	
				}
			}
			else
			{
				SetSprite(currentSet,setIndex);	
				lastSet = currentSet;
			}
			
		}
	}

	void SetSprite(SpriteSet s, int idx)
	{
		char[] charArr = spriteString.ToCharArray();
		charArr[2] = Utils.CharParseFast(idx);
		switch(s)
		{
			case SpriteSet.LEFT: 	charArr[1] = 'l'; sr.sprite = runLeft[idx]; break;
			case SpriteSet.RIGHT: 	charArr[1] = 'r'; sr.sprite = runRight[idx]; break;
			case SpriteSet.UP: 		charArr[1] = 'u'; sr.sprite = runUp[idx]; break;
			case SpriteSet.DOWN: 	charArr[1] = 'd'; sr.sprite = runDown[idx]; break;
		}
		spriteString = new string(charArr);
	}

	public override void SetSprite(string s)
	{
		spriteString = s;
		char setchar = s[1];
		int idx = Utils.IntParseFast(s[2]);
		switch(setchar)
		{
			case 'l': lastSet = SpriteSet.LEFT; 	sr.sprite = runLeft[idx]; break;
			case 'r': lastSet = SpriteSet.RIGHT; 	sr.sprite = runRight[idx]; break;
			case 'u': lastSet = SpriteSet.UP; 		sr.sprite = runUp[idx]; break;
			case 'd': lastSet = SpriteSet.DOWN; 	sr.sprite = runDown[idx]; break;
		}
	}
}
