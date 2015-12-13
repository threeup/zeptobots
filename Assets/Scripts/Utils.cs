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

}
