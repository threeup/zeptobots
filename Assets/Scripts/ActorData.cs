using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct ActorData {

	public int uid;
	public int oid;
	public int team; //red is 1 blue is 0
	public string spriteString;
	public int hp;
	public int tx;
	public int ty;
	public int rx;
	public int ry;
	public int fx;
	public int fy;
	public int speedLimit;
	public int damage;
	public int ttl;
	public string actionString;
	public string effectString;

	public ActorData(string str)
	{
		uid = -1;
		oid = -1;
		team = -1;
		spriteString = str;
		hp = -1;
		tx = -1;
		ty = -1;
		rx = -1;
		ry = -1;
		fx = -1;
		fy = -1;
		speedLimit = 1;
		damage = 1;
		ttl = -1;
		actionString = "";
		effectString = "";
	}

}
