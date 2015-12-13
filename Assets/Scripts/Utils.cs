using UnityEngine;
using System;
using System.Collections;
using WebSocketSharp;

public class Utils : MonoBehaviour {


	
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

}
