using UnityEngine;
using System.Collections;

public class Creature : MonoBehaviour {

	public int hpOverride = 1;
	public int damageOverride = 3;
	public int ttlOverride = 3;
	public int speedOverride = 3;
	public Actor actor = null;

	
	public Transform target;
	public Vector2 targetVec = Vector2.zero;


	public void Init(string sprite)
	{
		//bullets should move forwards
		actor.hp = hpOverride;
		actor.damage = damageOverride;
		actor.ttl = ttlOverride;
		actor.ttlTimer = ttlOverride;
		actor.engine.speedLimit = speedOverride;
		if( sprite.EndsWith("NW") ) { targetVec = new Vector2(-1,1); return; }
		if( sprite.EndsWith("NE") ) { targetVec = new Vector2(1,1); return; }
		if( sprite.EndsWith("SW") ) { targetVec = new Vector2(-1,-1); return; }
		if( sprite.EndsWith("SE") ) { targetVec = new Vector2(1,-1); return; }
		if( sprite.EndsWith("N") ) { targetVec = new Vector2(0,1); return; }
		if( sprite.EndsWith("E") ) { targetVec = new Vector2(1,0); return; }
		if( sprite.EndsWith("S") ) { targetVec = new Vector2(0,-1); return; }
		if( sprite.EndsWith("W") ) { targetVec = new Vector2(-1,0); return; }
	}


	void Update () 
	{
		Engine engine = actor.engine;
		float deltaTime = Time.deltaTime;
		if( target != null )
		{
			Vector2 diffVec = new Vector2(this.transform.position.x - target.position.x, this.transform.position.z - target.position.z);
			targetVec = diffVec.normalized;
		}
		engine.MoveUpdate(deltaTime, targetVec);

	}
}
