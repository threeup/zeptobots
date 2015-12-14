using UnityEngine;
using System;
using System.Collections;
using WebSocketSharp;

public class Utils : MonoBehaviour {


	
	public enum Ord
	{
		NE = 0,
		N = 1,
		NW = 2,
		W = 3,
		SW = 4,
		S = 5,
		SE = 6,
		E = 7,
	}

	public static int IntParseFast(string value)
    {
		int result = 0;
		for (int i = 0; i < value.Length; i++)
		{
			char letter = value[i];
			result = 10 * result + (letter - 48);
		}
		return result;
    }

    public static int IntParseFast(char letter)
    {
		int result = (letter - 48);
		return result;
    }

    public static char CharParseFast(int num)
    {
		char result = (char)(num + 48);
		return result;
    }

    public static Ord GetOrdFromVector(Vector2 vec)
    {
    	if( vec.x < -0.5 && vec.y > 0.5) { return Ord.NW; }
    	if( vec.x > 0.5 && vec.y > 0.5) { return Ord.NE; }
    	if( vec.x > 0.5 && vec.y < -0.5) { return Ord.SE; }
    	if( vec.x < -0.5 && vec.y < -0.5) { return Ord.SW; }
    	if( vec.y > 0.5) { return Ord.N; }
    	if( vec.x > 0.5) { return Ord.E; }
    	if( vec.y < -0.5) { return Ord.S; }
    	if( vec.x < -0.5) { return Ord.W; }
    	return Ord.N;
    }

     public static Ord GetRandomOrd()
    {
    	int num = UnityEngine.Random.Range(0,8);
    	return (Ord)num;
    }

    public static Vector2 GetVecFromString(string str)
    {
    	
		if( str.EndsWith("NW") ) { return new Vector2(-1,1); }
		if( str.EndsWith("NE") ) { return new Vector2(1,1); }
		if( str.EndsWith("SW") ) { return new Vector2(-1,-1); }
		if( str.EndsWith("SE") ) { return new Vector2(1,-1); }
		if( str.EndsWith("N") ) { return new Vector2(0,1); }
		if( str.EndsWith("E") ) { return new Vector2(1,0); }
		if( str.EndsWith("S") ) { return new Vector2(0,-1); }
		if( str.EndsWith("W") ) { return new Vector2(-1,0); }
		return Vector2.zero;
    }

}
