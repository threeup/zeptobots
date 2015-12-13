using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {

	public int uid = -1;
	public int oid = -1;
	public int tx = -1;
	public int ty = -1;
	public int rx = -1;
	public int ry = -1;
	public int hp = -1;

	public bool localInput = false;

	public Engine engine = null;
	public Hero hero = null;
	public Spriter spriter = null;

	private System.Text.StringBuilder sb = new System.Text.StringBuilder();


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
			this.engine.desiredPosition = new Vector3(rx, 0f, ry);
			this.spriter.SetSprite(sprite);
			this.engine.speedLimit = speed;
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
		sb.Append("\n");
		return sb.ToString();
	}

	public void SetLocal(bool val)
	{
		localInput = val;
		spriter.localUpdate = val;
	}

}
