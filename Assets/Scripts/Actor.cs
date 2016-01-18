using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour {

	public ActorData ad = new ActorData();

	public int UID { get { return ad.uid; } set { ad.uid = value; } }
	public int OID { get { return ad.oid; } set { ad.oid = value; } }
	public int Team { get { return ad.team; } set { ad.team = value; } }
	public string SpriteString { get { return ad.spriteString; } set { ad.spriteString = value; } }
	public int HP { get { return ad.hp; } set { ad.hp = value; } }
	public int defaulthp { get { return ad.defaulthp; } set { ad.defaulthp = value; } }
	public int TX { get { return ad.tx; } set { ad.tx = value; } }
	public int TY { get { return ad.ty; } set { ad.ty = value; } }
	public int RX { get { return ad.rx; } set { ad.rx = value; } }
	public int RY { get { return ad.ry; } set { ad.ry = value; } }
	public int FX { get { return ad.fx; } set { ad.fx = value; } }
	public int FY { get { return ad.fy; } set { ad.fy = value; } }
	public int CurrentSpeedLimit { get { return ad.currentSpeedLimit; } set { ad.currentSpeedLimit = value; } }
	public int DefaultSpeedLimit { get { return ad.defaultSpeedLimit; } set { ad.defaultSpeedLimit = value; } }
	public int Damage { get { return ad.damage; } set { ad.damage = value; } }
	public int TTL { get { return ad.ttl; } set { ad.ttl = value; } }
	

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
	public void Mod(bool authoritative, string[] chunks)
	{
		int uid = Utils.IntParseFast(chunks[(int)Pck.Ac.UID]);
		int oid = Utils.IntParseFast(chunks[(int)Pck.Ac.OID]);
		int team = Utils.IntParseFast(chunks[(int)Pck.Ac.TEAM]);
		if( ad.uid != uid )
		{
			ad.uid = uid;
		}
		if( ad.oid != oid )
		{
			ad.oid = oid;
		}
		if( ad.team != team )
		{
			ad.team = team;
		}
		int hp = Utils.IntParseFast(chunks[(int)Pck.Ac.HP]);
		int defaulthp = Utils.IntParseFast(chunks[(int)Pck.Ac.DEFAULTHP]);
		if( ad.hp != hp || ad.defaulthp != defaulthp )
		{
			ad.hp = hp;
			ad.defaulthp = defaulthp;
		}
		if( authoritative )
		{
			ad.tx = Utils.IntParseFast(chunks[(int)Pck.Ac.TX]);
			ad.ty = Utils.IntParseFast(chunks[(int)Pck.Ac.TY]);
			ad.rx = Utils.IntParseFast(chunks[(int)Pck.Ac.RX]);
			ad.ry = Utils.IntParseFast(chunks[(int)Pck.Ac.RY]);
			this.engine.desiredPosition = new Vector3(ad.rx-5, 0f, -ad.ry-5);
			ad.spriteString = chunks[(int)Pck.Ac.SPRITE];
			if( this.actorBody )
			{
				this.actorBody.SetSprite(ad.spriteString);
			}
			ad.currentSpeedLimit = Utils.IntParseFast(chunks[(int)Pck.Ac.CURRENTSPEEDLIMIT]);
			ad.defaultSpeedLimit = Utils.IntParseFast(chunks[(int)Pck.Ac.DEFAULTSPEEDLIMIT]);
			ad.fx = Utils.IntParseFast(chunks[(int)Pck.Ac.FX]);
			ad.fy = Utils.IntParseFast(chunks[(int)Pck.Ac.FY]);
			this.engine.SetFacing(ad.fx, ad.fy);
			
			ad.damage = Utils.IntParseFast(chunks[(int)Pck.Ac.DAMAGE]);
			ad.ttl = Utils.IntParseFast(chunks[(int)Pck.Ac.TTL]);
			this.ttlTimer = ad.ttl;
		}
		
	}


	public string SerializeActorData()
	{
		Pck.DynamicActorData(sb, ref ad, this);
		sb.Length = 0;
		sb.Append("requestactor|");
		Pck.PackActorData(sb,ad);
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
		ad.hp = -1;
	}

	public void TakeDamage(int val)
	{
		if( invulnerableTime < 0f )
		{
			ad.hp -= val;
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
