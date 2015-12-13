using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CamControl : MonoBehaviour {

	public static CamControl Instance;
	public float camHeight;
	public Transform target;
	private float nearSpeed = 60f;
	private float farSpeed = 6000f;
	private Vector3 desiredPosition = Vector3.zero;
	
	void Awake()
	{
		Instance = this;
	}

	void Update()
	{
		float deltaTime = Time.deltaTime;
		if( target != null )
		{
			LookAt(target.position);
		}
		Vector3 diff = this.desiredPosition - this.transform.position;
		float dist = diff.magnitude;
		float speed = dist > 100 ? farSpeed : nearSpeed;
		if( dist > 0.1f )
		{	
			float maxDist = speed*deltaTime;
			if( dist > maxDist )
			{
				Vector3 dir = diff/dist;
				this.transform.position = this.transform.position + dir*maxDist;
			}
			else
			{
				this.transform.position = this.desiredPosition;	
			}
		}
	}

	public void LookAt(Vector3 pos)
	{
		pos.y = camHeight;
		desiredPosition = pos;
	}

	public void FollowObject(Transform target)
	{
		this.target = target;
	}

}
