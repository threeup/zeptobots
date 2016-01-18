using UnityEngine;
using System.Collections;

public class RulesActor
{

}

public class RulesHero : RulesActor
{
	public static void Lookup(out int defaulthp, out int defaultSpeedLimit, out int dmg, out int ttl)
	{
		defaulthp = 10; 
		defaultSpeedLimit = 18; 
		dmg = 1; 
		ttl = 100000; 
	}
}

public class RulesBullet : RulesActor
{
	public static void Lookup(out int defaulthp, out int defaultSpeedLimit, out int dmg, out int ttl)
	{
		defaulthp = 1; 
		defaultSpeedLimit = 150; 
		dmg = 10; 
		ttl = 3; 
	}
}

public class RulesDog : RulesActor
{
	public static void Lookup(out int defaulthp, out int defaultSpeedLimit, out int dmg, out int ttl)
	{
		defaulthp = 2; 
		defaultSpeedLimit = 25; 
		dmg = 1; 
		ttl = 1000; 
	}
}