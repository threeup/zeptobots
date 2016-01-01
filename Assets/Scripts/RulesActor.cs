using UnityEngine;
using System.Collections;

public class RulesActor
{

}

public class RulesHero : RulesActor
{
	public static void Lookup(out int hp, out int speedlimit, out int dmg, out int ttl)
	{
		hp = 10; 
		speedlimit = 30; 
		dmg = 1; 
		ttl = 100000; 
	}
}

public class RulesBullet : RulesActor
{
	public static void Lookup(out int hp, out int speedlimit, out int dmg, out int ttl)
	{
		hp = 1; 
		speedlimit = 150; 
		dmg = 10; 
		ttl = 3; 
	}
}

public class RulesDog : RulesActor
{
	public static void Lookup(out int hp, out int speedlimit, out int dmg, out int ttl)
	{
		hp = 2; 
		speedlimit = 25; 
		dmg = 1; 
		ttl = 1000; 
	}
}