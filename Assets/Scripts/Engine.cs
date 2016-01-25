using UnityEngine;
using System.Collections;

public class Engine : MonoBehaviour {

	private float currentStep = 0f;
	public float currentSpeed = 0f;

	public Vector3 lastPosition = Vector3.zero;
	public Vector3 desiredPosition = Vector3.zero;
	public Vector2 moveVec = Vector2.zero;
	public Vector2 facingVec = Vector2.zero;
	
	

	public bool moveLock = false;
	public bool facingLock = false;

	public Tile engineTile = null;

	public Actor actor;

	public delegate void EngineEvent();
	public EngineEvent EngineReached;
	public EngineEvent EngineCollided;
	
	public void Init()
	{
		EngineReached += Noop;
		EngineCollided += Noop;		
	}


	public void SetDesiredPosition(Vector3 pos)
	{
		if(moveLock)
		{
			return;
		}
		this.desiredPosition = pos;
	}

	public void Halt()
	{
		this.transform.position = lastPosition;
		this.desiredPosition = lastPosition;
		EngineCollided();
	}

	public void MoveUpdate (float deltaTime, Vector2 inputVec) 
	{
		moveVec = inputVec;
		
		lastPosition = this.transform.position;
		if( !facingLock )
		{
			if(moveVec.x != 0 || moveVec.y != 0)
			{
				facingVec = moveVec;
			}
		}
		if( actor.localInput )
		{
			SetDesiredPosition( this.transform.position + new Vector3(moveVec.x, 0, moveVec.y)*1000f );
		}

		Vector3 diff = this.desiredPosition - this.transform.position;
		float dist = diff.magnitude;
		if( dist > 0.1f )
		{	
			float maxStep = actor.TopSpeed*deltaTime;
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
			EngineReached();
		}
		Vector3 centerPos = this.transform.position- Vector3.forward*5+ Vector3.right*5;
		Sector sec = World.Instance.GetSectorAt(centerPos);
		Tile t = null;
		if( sec != null )
		{
			t = sec.GetTileAt(centerPos);
		}
		bool collide = t == null && engineTile != null;
		if( !collide ) 
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
					actor.TX = t.rtx;
					actor.TY = t.rty;
					
				}
				else
				{
					collide = true;
				}
			}
		}
		if( collide )
		{
			Halt();
			currentStep = 0f; 
		}
		actor.RX = (int)Mathf.Round(this.transform.position.x) + 5;
		actor.RY = -(int)Mathf.Round(this.transform.position.z) - 5;
		actor.FX = (int)Mathf.Clamp(facingVec.x*10000, -1,1);
		actor.FY = (int)Mathf.Clamp(facingVec.y*10000, -1,1);
		currentSpeed = currentStep/deltaTime;

	}

	public void SetFacing(int fx, int fy)
	{
		facingVec = new Vector2(fx, fy);
	}


	public void Noop()
	{
		
	}

}
