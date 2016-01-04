using UnityEngine;
using System.Collections;

public class Creature : MonoBehaviour {

	public Actor actor = null;

	
	public Transform target;
	public Vector2 targetVec = Vector2.zero;


	public void Init(string sprite)
	{
		targetVec = actor.engine.facingVec;
		actor.engine.EngineCollided += CreatureCollide;
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

	public void CreatureCollide()
	{
		targetVec = Vector2.zero;
		actor.TakeDeath();
	}
}
