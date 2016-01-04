using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour {

	public string outString = "-";

	private int uid = -1;
	public int UID { get { return uid; } set { uid = value; } }
	private int oid = -1;
	public int OID { get { return oid; } set { oid = value; } }
	private int team = -1; //red is 1 blue is 0
	public int Team { get { return team; } set { team = value; } }
	private int tx = -1;
	public int TX { get { return tx; } set { tx = value; } }
	private int ty = -1;
	public int TY { get { return ty; } set { ty = value; } }
	private int rx = -1;
	public int RX { get { return rx; } set { rx = value; } }
	private int ry = -1;
	public int RY { get { return ry; } set { ry = value; } }
	private int hp = -1;
	public int HP { get { return hp; } set { hp = value; } }
	private int damage = 1;
	public int Damage { get { return damage; } set { damage = value; } }
	private int ttl = -1;
	public int TTL { get { return ttl; } set { ttl = value; } }
	

	public bool localInput = false;

	public Engine engine = null;
	public Hero hero = null;
	public Creature creature = null;
	public ActorBody actorBody = null;
	public HealthBar healthBar = null;
	public List<char> effectNames = new List<char>();
	public GameEffect[] effects;

	private System.Text.StringBuilder sb = new System.Text.StringBuilder();


	private float invulnerableTime = -1f;
	public float ttlTimer = -1f;

	public void Init(string sprite)
	{
		effects = new GameEffect[2];
		effects[0] = this.gameObject.AddComponent<GameEffect>();
		effects[1] = this.gameObject.AddComponent<GameEffect>();
		engine.Init();
		if( hero )
		{
			hero.Init();
		}
		if( creature )
		{
			creature.Init(sprite);
		}
	}

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
		for(int i=0; i<effects.Length; ++i)
		{
			effects[i].EffectUpdate(deltaTime, this);
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
			if( this.actorBody )
			{
				this.actorBody.SetSprite(sprite);
			}
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

	public void PrepOutString()
	{
		sb.Length = 0;
		sb.Append("requestactor|");
		sb.Append(this.uid);
		sb.Append("|");
		sb.Append(this.oid);
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
		sb.Append(this.engine.facingX);
		sb.Append("|");
		sb.Append(this.engine.facingY);
		sb.Append("|");
		if( this.actorBody )
		{
			sb.Append(this.actorBody.spriteString); //10
		}
		else
		{
			sb.Append("  ");
		}
		sb.Append("|");
		sb.Append(this.hp);
		sb.Append("|");
		sb.Append(this.engine.speedLimit);
		sb.Append("|");
		sb.Append(this.damage);
		sb.Append("|");
		sb.Append(this.ttl);
		sb.Append("|");
		for(int i=0; i<effects.Length; ++i)
		{
			sb.Append(effects[i].effName);
			sb.Append(effects[i].effStateName);
			sb.Append("|");
		}
		if( hero )
		{
			for(int i=0; i<hero.actions.Length; ++i)
			{
				sb.Append(hero.actions[i].abName);
				sb.Append(hero.actions[i].abStateName);
				sb.Append("|");
			}
		}
		sb.Append("\n");
		outString = sb.ToString();
	}

	public void SetLocal(bool val)
	{
		localInput = val;
		if( this.actorBody )
		{
			this.actorBody.localUpdate = val;
		}
	}

	public void TakeDeath()
	{
		this.hp = -1;
	}

	public void TakeDamage(int val)
	{
		if( invulnerableTime < 0f )
		{
			this.hp -= val;
			this.invulnerableTime = 1f;
		}
	}

	public void AddEff(char effName, float duration)
	{
		if( !effectNames.Contains(effName) )
		{
			GameEffect ge = GetVacantEffect();
			if( ge != null )
			{
				effectNames.Add(effName);
				switch(effName)
				{
					case 'G': GlowEff.Assign(ge, effName); break;
					case 'R': RockEff.Assign(ge, effName); break;
					default: break;
				}
				ge.SetPause(false);
				ge.SetDuration(duration);
				ge.Advance(this);
			}
		}
	}

	public void PauseEff(char effName)
	{
		if( effectNames.Contains(effName) )
		{
			GameEffect ge = GetNamedEffect(effName);
			if( ge != null )
			{
				ge.SetPause(true);
			}
		}
	}

	public void RemEff(char effName)
	{
		if( effectNames.Contains(effName) )
		{
			GameEffect ge = GetNamedEffect(effName);
			if( ge != null )
			{
				ge.Interrupt(this);
			}
			effectNames.Remove(effName);
		}
	}

	public GameEffect GetVacantEffect()
	{
		for(int i=0; i<effects.Length; ++i)
		{
			if(!effects[i].IsActive )
			{
				return effects[i];
			}
		}
		return null;
	}

	public GameEffect GetNamedEffect(char effName)
	{
		for(int i=0; i<effects.Length; ++i)
		{
			//if(string.Compare(effects[i].effName, effName) == 0)
			if(effects[i].effName == effName)
			{
				return effects[i];
			}
		}
		return null;
	}



}
