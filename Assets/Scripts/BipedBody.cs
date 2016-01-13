using UnityEngine;
using System.Collections;

public class BipedBody : ActorBody {


	public Transform larm;
	public Transform rarm;
	public Transform lleg;
	public Transform rleg;

	private float legDistance;
	private int legTick;

	protected override void Awake()
	{
		base.Awake();
	}
	public override void BodyUpdate(float deltaTime, Vector2 facing, float speed, bool larmSwing, bool rarmSwing)
	{
		base.BodyUpdate(deltaTime, facing, speed, larmSwing, rarmSwing);
		
        

        SetLeftArm(larmSwing?1:0);
        SetRightArm(rarmSwing?1:0);
        if( speed < 0.1 )
        {
	        SetLeftLeg(0);
	        SetRightLeg(0);
	    }
	    else
	    {
	    	legDistance += speed*deltaTime;
	    	if( legDistance >= 3.5f )
	    	{
	    		legDistance = 0f;
	    		legTick++;
	    		int leftLeg = legTick%2;
		    	int rightLeg = (legTick+1)%2;
		    	SetLeftLeg(leftLeg + 1);
		        SetRightLeg(rightLeg + 1);
	    	}
	    	
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

}
