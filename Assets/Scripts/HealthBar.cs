using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	int hp = 10;
	int maxHP = 10;
	public Renderer sr = null;

	public void SetHP(int hp)
	{
		if( this.hp == hp )
		{
			return;
		}
		this.hp = hp;
		int percentHealth = (int)Mathf.Round(10*hp / maxHP);
		Color color = Color.green;
		if( percentHealth < 3 )
		{
			color = Color.red;
		}
		else if( percentHealth < 6 )
		{
			color = Color.yellow;
		}
		sr.material.color = color;
		if( this.gameObject )
		{
			this.gameObject.transform.localScale = new Vector3(percentHealth*6,10,10);
		}

	}
}
