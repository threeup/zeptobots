using UnityEngine;
using System.Collections;

public class ActorBody : MonoBehaviour {


	public Light[] coloredLights = null;
	public Renderer[] allRenderers = null;
	public Renderer[] coloredRenderers = null;
	public string spriteString = "RD0";
	public bool localUpdate = false;
	private Transform thisTransform;

	public Transform larm;
	public Transform rarm;
	public Transform lleg;
	public Transform rleg;

	private float legTime;
	private int legTick;

	void Awake()
	{
		thisTransform = this.transform;
	}
	public virtual void BodyUpdate(float deltaTime, Vector2 facing, float speed, bool larm, bool rarm)
	{
		float rot_y = Mathf.Atan2(facing.y, -facing.x) * Mathf.Rad2Deg;
        thisTransform.rotation = Quaternion.Euler(0f, rot_y - 90, 0f);

        

        SetLeftArm(larm?1:0);
        SetRightArm(rarm?1:0);
        if( speed < 0.1 )
        {
	        SetLeftLeg(0);
	        SetRightLeg(0);
	    }
	    else
	    {
	    	legTime += speed*deltaTime;
	    	if( legTime >= 0.1f )
	    	{
	    		legTime = 0f;
	    		legTick++;
	    		int leftLeg = legTick%2;
		    	int rightLeg = (legTick+1)%2;
		    	SetLeftLeg(leftLeg + 1);
		        SetRightLeg(rightLeg + 1);
	    	}
	    	
	    }
	}

	public virtual void SetSprite(string s)
	{
		spriteString = s;
		if( spriteString[0] == 'R')
		{
			SetColored(Color.red);
			
		}
		if( spriteString[0] == 'B')
		{
			SetColored(Color.blue);
		}
	}

	void SetLeftArm(int v)
	{
		float rotZ = 0;
		float posY = 0;
		float posZ = 0;
		switch(v)
		{
			default:
			case 0: rotZ = 0;   posY = 0f;   posZ = 0; break;
			case 1: rotZ = 70;  posY = 0.8f; posZ = 0.7f; break;
			case 2: rotZ = -70; posY = 0.8f; posZ = -0.7f; break;
		}
		larm.localRotation = Quaternion.Euler(0, -90f, rotZ);
		larm.localPosition = new Vector3(-1.5f, 3.8f+posY, posZ);	
	}

	void SetRightArm(int v)
	{
		float rotZ = 0;
		float posY = 0;
		float posZ = 0;
		switch(v)
		{
			default:
			case 0: rotZ = 0;   posY = 0f;   posZ = 0; break;
			case 1: rotZ = -70;  posY = 0.8f; posZ = 0.7f; break;
			case 2: rotZ = 70; posY = 0.8f; posZ = -0.7f; break;
		}
		rarm.localRotation = Quaternion.Euler(0, 90f, rotZ);
		rarm.localPosition = new Vector3(1.5f, 3.8f+posY, posZ);
	}
	void SetLeftLeg(int v)
	{
		float rotX = 0;
		float posZ = 0;
		switch(v)
		{
			default:
			case 0: rotX = 0;   posZ = 0; break;
			case 1: rotX = 30;  posZ = -1; break;
			case 2: rotX = -30; posZ = 1; break;
		}
		lleg.localRotation = Quaternion.Euler(rotX, 0f, 15f);
		lleg.localPosition = new Vector3(1.5f, 0, posZ);	
	}
	void SetRightLeg(int v)
	{
		float rotX = 0;
		float posZ = 0;
		switch(v)
		{
			default:
			case 0: rotX = 0;   posZ = 0; break;
			case 1: rotX = 30;  posZ = -1; break;
			case 2: rotX = -30; posZ = 1; break;
		}
		rleg.localRotation = Quaternion.Euler(rotX, 0f, -15f);
		rleg.localPosition = new Vector3(-1.5f, 0, posZ);	
	}

	void SetColored(Color c)
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
