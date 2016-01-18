using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct ActorQuickData {

	public int tx;
	public int ty;
	public int rx;
	public int ry;
	public int fx;
	public int fy;
	public int hp;
	public int topSpeed;
	public int damage;
	public int ttl;
	public string actionString;
	public string effectString;

	public ActorQuickData(int hp)
	{
		this.tx = -1;
		this.ty = -1;
		this.rx = -1;
		this.ry = -1;
		this.fx = -1;
		this.fy = -1;
		this.hp = hp;
		this.topSpeed = 1;
		this.damage = 1;
		this.ttl = -1;
		this.actionString = "";
		this.effectString = "";
	}

	public ActorQuickData(ActorBasicData abd)
	{
		this.tx = abd.tx;
    	this.ty = abd.ty;
		this.rx = abd.tx*10+5;
		this.ry = abd.ty*10-5;
		this.fx = 0;
		this.fy = -1;
		this.hp = abd.hp;
		this.topSpeed = abd.topSpeed;
		this.damage = abd.damage;
		this.ttl = -1;
		this.actionString = "";
		this.effectString = "";
	}

}



[System.Serializable]
public struct ActorBasicData {

	public int uid;
	public int oid;
	public int team; //red is 1 blue is 0
	public string visualString;
	public int tx;
	public int ty;
	public int hp;
	public int topSpeed;
	public int damage;

	public ActorBasicData(string visualString)
	{
		this.uid = -1;
		this.oid = -1;
		this.team = -1;
		this.visualString = visualString;
		this.tx = -1;
		this.ty = -1;
		this.hp = -1;
		this.topSpeed = 1;
		this.damage = 1;
	}

}
