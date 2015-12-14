using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {

	public int uid = -1;
	public int oid = -1;
	public int team = -1; //red is 1 blue is 0
	public int tx = -1;
	public int ty = -1;
	public int rx = -1;
	public int ry = -1;
	public int hp = -1;
	public int damage = 1;
	public int ttl = -1;
	

	public bool localInput = false;

	public Engine engine = null;
	public Hero hero = null;
	public Creature creature = null;
	public Spriter spriter = null;
	public HealthBar healthBar = null;

	private System.Text.StringBuilder sb = new System.Text.StringBuilder();


	private float invulnerableTime = -1f;
	public float ttlTimer = -1f;

	public void Update()
	{
		float deltaTime = Time.deltaTime;
		if( invulnerableTime > 0f )
		{
			invulnerableTime -= deltaTime;
		}
		if( ttlTimer > 0f && ttlTimer < 999f )
		{
			ttlTimer -= deltaTime;
			ttl = (int)Mathf.Ceil(ttlTimer);
		}
		if( healthBar != null )
		{
			healthBar.SetHP(this.hp);
		}
	}
	public void Mod(bool authoritative, int uid, int oid, 
		int team, int tx,int ty,int rx,int ry, string sprite,
		int hp, int speed, int damage, int ttl,
		int fx, int fy)
	{
		if( this.uid != uid )
		{
			this.uid = uid;
		}
		if( this.oid != oid )
		{
			this.oid = oid;
		}
		if( this.team != team )
		{
			this.team = team;
		}
		if( authoritative )
		{
			this.tx = tx;
			this.ty = ty;
			this.rx = rx;
			this.ry = ry;
			this.engine.desiredPosition = new Vector3(rx-5, 0f, -ry-5);
			this.spriter.SetSprite(sprite);
			this.engine.speedLimit = speed;
			this.engine.facingX = fx;
			this.engine.facingY = fy;
			this.engine.facingVec = new Vector2(fx, fy);
			this.damage = damage;
			this.ttl = ttl;
			this.ttlTimer = ttl;
		}
		if( this.hp != hp )
		{
			this.hp = hp;
		}
	}

	public string GetOutString(int localOID)
	{
		sb.Length = 0;
		sb.Append("requestactormod|");
		sb.Append(this.uid);
		sb.Append("|");
		sb.Append(localOID);
		sb.Append("|");
		sb.Append(this.team);
		sb.Append("|");
		sb.Append(this.tx);
		sb.Append("|");
		sb.Append(this.ty);
		sb.Append("|");
		sb.Append(this.rx);
		sb.Append("|");
		sb.Append(this.ry);
		sb.Append("|");
		sb.Append(this.spriter.spriteString);
		sb.Append("|");
		sb.Append(this.hp);
		sb.Append("|");
		sb.Append(this.engine.speedLimit);
		sb.Append("|");
		sb.Append(this.damage);
		sb.Append("|");
		sb.Append(this.ttl);
		sb.Append("|");
		sb.Append(this.engine.facingX);
		sb.Append("|");
		sb.Append(this.engine.facingY);
		sb.Append("\n");
		return sb.ToString();
	}

	public void SetLocal(bool val)
	{
		localInput = val;
		spriter.localUpdate = val;
	}

	public void TakeDamage(int val)
	{
		if( invulnerableTime < 0f )
		{
			this.hp -= val;
			this.invulnerableTime = 1f;
		}
	}

}
