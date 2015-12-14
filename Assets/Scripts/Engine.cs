using UnityEngine;
using System.Collections;

public class Engine : MonoBehaviour {

	public int speedLimit = 30;
	public float currentStep = 0f;

	public Vector3 desiredPosition = Vector3.zero;
	public Vector2 moveVec = Vector2.zero;
	public Vector2 facingVec = Vector2.zero;

	public bool moveLock = false;
	public bool facingLock = false;

	public Tile engineTile = null;

	public Actor actor;
	
	public void MoveUpdate (float deltaTime, Vector2 inputVec) 
	{
		moveVec = inputVec;
		
		Vector3 originalPosition = this.transform.position;
		if( !facingLock )
		{
			if(moveVec.x != 0 || moveVec.y != 0)
			{
				facingVec = moveVec;
			}
		}
		if( !moveLock && actor.localInput )
		{
			this.desiredPosition = this.transform.position + new Vector3(moveVec.x, 0, inputVec.y);
		}

		Vector3 diff = this.desiredPosition - this.transform.position;
		float dist = diff.magnitude;
		if( dist > 0.1f )
		{	
			float maxStep = speedLimit*deltaTime;
			if( dist > maxStep )
			{
				currentStep = maxStep;
				Vector3 dir = diff/dist;
				this.transform.position = this.transform.position + dir*currentStep;
			}
			else
			{
				currentStep = dist;
				this.transform.position = this.desiredPosition;	
			}
		}
		else
		{
			currentStep = 0f;
		}
		Vector3 centerPos = this.transform.position- Vector3.forward*5+ Vector3.right*5;
		Sector sec = World.Instance.GetSectorAt(centerPos);
		Tile t = null;
		if( sec != null )
		{
			t = sec.GetTileAt(centerPos);
		}
		if( t == null && engineTile != null )
		{
			this.transform.position = originalPosition;
			currentStep = 0f; 
		}
		else 
		{
			if( t != engineTile )
			{
				bool added = t.TryAdd(this.actor);
				if( added )
				{
					if( engineTile != null )
					{
						engineTile.Remove(this.actor);
					}
					engineTile = t;
					actor.tx = t.rtx;
					actor.ty = t.rty;
					actor.rx = (int)Mathf.Round(this.transform.position.x);
					actor.ry = (int)Mathf.Round(this.transform.position.z);
				}
				else
				{
					this.transform.position = originalPosition;
					currentStep = 0f; 
				}
			}

		}
	}




}
