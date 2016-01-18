using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour {

	public ActorQuickData aqd = new ActorQuickData();
	public ActorBasicData abd = new ActorBasicData();

	public int UID { get { return abd.uid; } set { abd.uid = value; } }
	public int OID { get { return abd.oid; } set { abd.oid = value; } }
	public int Team { get { return abd.team; } set { abd.team = value; } }
	public string VisualString { get { return abd.visualString; } set { abd.visualString = value; } }
	public int HP { get { return aqd.hp; } set { aqd.hp = value; } }
	public int BaseHP { get { return abd.hp; } set { abd.hp = value; } }
	public int TX { get { return aqd.tx; } set { aqd.tx = value; } }
	public int BaseTX { get { return abd.tx; } set { abd.tx = value; } }
	public int TY { get { return aqd.ty; } set { aqd.ty = value; } }
	public int BaseTY { get { return abd.ty; } set { abd.ty = value; } }
	public int RX { get { return aqd.rx; } set { aqd.rx = value; } }
	public int RY { get { return aqd.ry; } set { aqd.ry = value; } }
	public int FX { get { return aqd.fx; } set { aqd.fx = value; } }
	public int FY { get { return aqd.fy; } set { aqd.fy = value; } }
	public int TopSpeed { get { return aqd.topSpeed; } set { aqd.topSpeed = value; } }
	public int BaseTopSpeed { get { return abd.topSpeed; } set { abd.topSpeed = value; } }
	public int Damage { get { return aqd.damage; } set { aqd.damage = value; } }
	public int TTL { get { return aqd.ttl; } set { aqd.ttl = value; } }
	

	public bool localInput = false;

	public Engine engine = null;
	public Hero hero = null;
	public ActorBody actorBody = null;
	public Creature creature = null;
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
			TTL = (int)Mathf.Ceil(ttlTimer);
		}
		if( healthBar != null )
		{
			healthBar.SetHP(HP);
		}
		for(int i=0; i<effects.Length; ++i)
		{
			effects[i].EffectUpdate(deltaTime, this);
		}
	}
	public void ModBase(bool authoritative, ActorBasicData abd)
	{
		this.abd = abd;
		if( this.actorBody )
		{
			this.actorBody.SetSprite(abd.visualString);
		}
	}

	public void ModQuick(bool authoritative, ActorQuickData aqd)
	{
		this.aqd = aqd;
		this.engine.desiredPosition = new Vector3(aqd.rx-5, 0f, -aqd.ry-5);
		this.engine.SetFacing(aqd.fx, aqd.fy);
		this.ttlTimer = aqd.ttl;
		
	}


	public string SerializeActorData()
	{
		Pck.RefreshActorQuickData(sb, ref aqd, this);
		sb.Length = 0;
		sb.Append("requestactor|");
		Pck.PackData(sb, abd);
		Pck.PackData(sb, aqd);
		sb.Append("\n");
		return sb.ToString();
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
		aqd.hp = -1;
	}

	public void TakeDamage(int val)
	{
		if( invulnerableTime < 0f )
		{
			aqd.hp -= val;
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
