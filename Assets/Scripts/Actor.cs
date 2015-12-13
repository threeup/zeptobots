using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {

	public int uid = -1;
	public int oid = -1;
	public int tx = -1;
	public int ty = -1;
	public int rx = -1;
	public int ry = -1;
	public string sprite = "R";
	public int hp = -1;
	public int speed = 30;

	public float currentSpeed = 0f;

	private Vector3 desiredPosition = Vector3.zero;
	public Vector2 inputVec = Vector2.zero;
	public Vector2 facingVec = Vector2.zero;

	public bool inputA = false;
	public bool inputB = false;
	public GameAction[] actions;

	public bool localInput = false;
	private bool moveLock = false;
	private bool facingLock = false;

	public Sector lastSector = null;
	public Tile lastTile = null;
	
	public Spriter spriter = null;

	void Awake()
	{
		actions = GetComponents<GameAction>();
	}

	void Start()
	{

		actions[0].Activate = AttackActivate;
		actions[0].Cooldown = AttackCooldown;

		actions[1].Activate = DashActivate;
		actions[1].Recover = DashRecover;
		actions[1].Cooldown = DashCooldown;
		

	}


	
	void Update () 
	{
		float deltaTime = Time.deltaTime;
		
		Vector3 originalPosition = this.transform.position;
		if( !facingLock )
		{
			if(inputVec.x != 0 || inputVec.y != 0)
			{
				facingVec = inputVec;
			}
		}
		if( !moveLock && localInput )
		{
			this.desiredPosition = this.transform.position + new Vector3(inputVec.x, 0, inputVec.y);
		}

		Vector3 diff = this.desiredPosition - this.transform.position;
		float dist = diff.magnitude;
		if( dist > 0.1f )
		{	
			float maxSpeed = speed*deltaTime;
			if( dist > maxSpeed )
			{
				currentSpeed = maxSpeed;
				Vector3 dir = diff/dist;
				this.transform.position = this.transform.position + dir*currentSpeed;
			}
			else
			{
				currentSpeed = dist;
				this.transform.position = this.desiredPosition;	
			}
		}
		else
		{
			currentSpeed = 0f;
		}
		Sector sec = World.Instance.GetSectorAt(this.transform.position);
		lastSector = sec;
		Tile t = sec.GetTileAt(this.transform.position);
		lastTile = t;
		if( t == null || !t.Passable(this) )
		{
			this.transform.position = originalPosition;
			currentSpeed = 0f; 
		}
		else
		{
			this.tx = t.rtx;
			this.ty = t.rty;
			this.rx = (int)Mathf.Round(this.transform.position.x);
			this.ry = (int)Mathf.Round(this.transform.position.z);
		}
		actions[0].ActionUpdate(deltaTime, this, inputA);
		actions[1].ActionUpdate(deltaTime, this, inputB);

		spriter.SpriteUpdate(deltaTime, facingVec, currentSpeed);
	}



	public void Mod(bool authoritative, int uid, int oid,int tx,int ty,int rx,int ry,string sprite,int hp, int speed)
	{
		if( this.uid != uid )
		{
			this.uid = uid;
		}
		if( this.oid != oid )
		{
			this.oid = oid;
		}
		if( authoritative )
		{
			this.tx = tx;
			this.ty = ty;
			this.rx = rx;
			this.ry = ry;
			this.desiredPosition = new Vector3(rx, 0f, ry);
			this.sprite = sprite;
			this.speed = speed;
		}
		if( this.hp != hp )
		{
			this.hp = hp;
		}
	}

	public void Select()
	{
		localInput = true;
	}

	public void Deselect()
	{
		localInput = false;
	}


	public static void DashActivate(GameAction ga, Actor actor)
	{
		float dashDistance = 40f;
		Vector3 delta = new Vector3(actor.facingVec.x, 0, actor.facingVec.y)*dashDistance;
		actor.desiredPosition = actor.transform.position + delta;
		actor.speed = 240;
		actor.moveLock = true;
		ga.lockTime = 0.6f;
	}

	public static void DashRecover(GameAction ga, Actor actor)
	{
		actor.speed = 20;
		actor.moveLock = false;
		ga.lockTime = 1f;
	}


	public static void DashCooldown(GameAction ga, Actor actor)
	{
		actor.speed = 30;
		actor.moveLock = false;
		ga.lockTime = 0f;
	}

	public static void AttackActivate(GameAction ga, Actor actor)
	{
		
	}

	public static void AttackCooldown(GameAction ga, Actor actor)
	{
		
	}

}
