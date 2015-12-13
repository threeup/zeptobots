using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	public enum TileType
	{
		NONE = 0,
		SOLID = 1,
		SPAWN = 2,
		FARM = 3,
	}

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

	public TileType tileType = TileType.NONE;
	public int ltx;
	public int lty;
	public int rtx;
	public int rty;
	private int imageIndex = -1;
	private int solidImageIndex;
	public int desiredImageIndex;
	private int defaultImageIndex = 20;

	public List<Actor> occupyList = new List<Actor>();
	public Tile[] nbors = new Tile[8];
	public SpriteRenderer spriteRend = null;
	public TileMaterial tileMat = null;

	public void Init(int ltx, int lty, int rtx, int rty)
	{
		this.ltx = ltx;
		this.lty = lty;
		this.rtx = rtx;
		this.rty = rty;
	}

	public void AddNbor(Tile t)
	{
		if( t == null )
		{
			return;
		}
		bool isLeft = t.ltx < this.ltx;
		bool isRight = t.ltx > this.ltx;
		bool isUp = t.lty > this.lty;
		bool isDown = t.lty < this.lty;
		if( isLeft && isUp ) { 		nbors[(int)Ord.NW] = t; t.nbors[(int)Ord.SE] = this; return; }
		if( isRight && isUp ) { 	nbors[(int)Ord.NE] = t; t.nbors[(int)Ord.SW] = this; return; }
		if( isLeft && isDown ) { 	nbors[(int)Ord.SW] = t; t.nbors[(int)Ord.NE] = this; return; }
		if( isRight && isDown ) { 	nbors[(int)Ord.SE] = t; t.nbors[(int)Ord.NW] = this; return; }
		if( isLeft ) { 				nbors[(int)Ord.W] = t; t.nbors[(int)Ord.E] = this; return; }
		if( isRight ) { 			nbors[(int)Ord.E] = t; t.nbors[(int)Ord.W] = this; return; }
		if( isUp ) { 				nbors[(int)Ord.N] = t; t.nbors[(int)Ord.S] = this; return; }
		if( isDown ) { 				nbors[(int)Ord.S] = t; t.nbors[(int)Ord.N] = this; return; }
	}

	public void RefreshSprite()
	{
		bool solid = tileType == TileType.SOLID;
		solidImageIndex = GetTileImageIndex();
		desiredImageIndex = solid?solidImageIndex:defaultImageIndex;
		//spriteRend.sprite = tileMat.GetSprite(desiredImageIndex);
	}

	public void Update()
	{
		if( desiredImageIndex != imageIndex )
		{
			imageIndex = desiredImageIndex;
			spriteRend.sprite = tileMat.GetSprite(imageIndex);
		}
	}

	char GetSolid(Tile t)
	{
		if(t == null || t.tileType == this.tileType)
		{
			return '1';
		}
		return '0';
	}

	bool TestCornerTemplate(string template, string str) {
		for (int i = 0; i < template.Length; i++) {
			if (template[i]!='X' && template[i]!=str[i]) {
				return false;
			}
		}
		return true;
	}
	int CountOnes(string str) {
		int result=0;
		for (int i = 0; i < str.Length; i++) {
			if (str[i]=='1') {
				result++;
			}
		}
		return result;
	}

	int GetTileImageIndex() {
		int result=0;
		
		char[] sidesArray = { 
			GetSolid(nbors[(int)Ord.N]), 
			GetSolid(nbors[(int)Ord.E]), 
			GetSolid(nbors[(int)Ord.S]), 
			GetSolid(nbors[(int)Ord.W]) };
		char[] cornersArray = { 
			GetSolid(nbors[(int)Ord.NE]), 
			GetSolid(nbors[(int)Ord.NW]), 
			GetSolid(nbors[(int)Ord.SW]), 
			GetSolid(nbors[(int)Ord.SE]) };
		string sides = new string(sidesArray);
		string corners = new string(cornersArray);
		
		switch (CountOnes(sides)) {
		case 1:
			switch (sides) {
			case "0010": if (TestCornerTemplate("XXXX", corners)) {result= 3;} break;
			case "0001": if (TestCornerTemplate("XXXX", corners)) {result=26;} break;
			case "1000": if (TestCornerTemplate("XXXX", corners)) {result=19;} break;
			case "0100": if (TestCornerTemplate("XXXX", corners)) {result=24;} break;
			}
			break;
			
		case 2:
			switch (sides) {
			case "1010": if (TestCornerTemplate("XXXX", corners)) {result=11;} break;
			case "0101": if (TestCornerTemplate("XXXX", corners)) {result=25;} break;
			case "0110": if (TestCornerTemplate("XX0X", corners)) {result= 5;} 
				if (TestCornerTemplate("XX1X", corners)) {result= 0;} break;
			case "0011": if (TestCornerTemplate("XXX0", corners)) {result= 7;} 
				if (TestCornerTemplate("XXX1", corners)) {result= 2;} break;
			case "1001": if (TestCornerTemplate("0XXX", corners)) {result=23;} 
				if (TestCornerTemplate("1XXX", corners)) {result=18;} break;
			case "1100": if (TestCornerTemplate("X0XX", corners)) {result=21;} 
				if (TestCornerTemplate("X1XX", corners)) {result=16;} break;
			}
			break;
			
		case 3:
			switch (sides) {
			case "0111": if (TestCornerTemplate("XX11", corners)) {result= 1;}
				if (TestCornerTemplate("XX01", corners)) {result=34;}
				if (TestCornerTemplate("XX10", corners)) {result=37;}
				if (TestCornerTemplate("XX00", corners)) {result= 6;} break;
				
			case "1011": if (TestCornerTemplate("1XX1", corners)) {result=10;}
				if (TestCornerTemplate("1XX0", corners)) {result=35;}
				if (TestCornerTemplate("0XX1", corners)) {result=45;}
				if (TestCornerTemplate("0XX0", corners)) {result=15;} break;
				
			case "1101": if (TestCornerTemplate("11XX", corners)) {result=17;}
				if (TestCornerTemplate("01XX", corners)) {result=43;}
				if (TestCornerTemplate("10XX", corners)) {result=44;}
				if (TestCornerTemplate("00XX", corners)) {result=22;} break;
				
			case "1110": if (TestCornerTemplate("X11X", corners)) {result= 8;}
				if (TestCornerTemplate("X01X", corners)) {result=42;}
				if (TestCornerTemplate("X10X", corners)) {result=36;}
				if (TestCornerTemplate("X00X", corners)) {result=13;} break;
			}
			break;
			
		case 4:
			if (TestCornerTemplate("1111", corners)) {result= 9; break;}
			if (TestCornerTemplate("0000", corners)) {result=14; break;}
			if (TestCornerTemplate("1010", corners)) {result= 4; break;}
			if (TestCornerTemplate("0101", corners)) {result=12; break;}
			
			if (TestCornerTemplate("1000", corners)) {result=38; break;}
			if (TestCornerTemplate("0100", corners)) {result=39; break;}
			if (TestCornerTemplate("0010", corners)) {result=47; break;}
			if (TestCornerTemplate("0001", corners)) {result=46; break;}
			
			if (TestCornerTemplate("1101", corners)) {result=32; break;}
			if (TestCornerTemplate("1110", corners)) {result=33; break;}
			if (TestCornerTemplate("0111", corners)) {result=41; break;}
			if (TestCornerTemplate("1011", corners)) {result=40; break;}
			
			if (TestCornerTemplate("0XX0", corners)) {result=28; break;}
			if (TestCornerTemplate("00XX", corners)) {result=29; break;}
			if (TestCornerTemplate("X00X", corners)) {result=30; break;}
			if (TestCornerTemplate("XX00", corners)) {result=31; break;}
			break;
			
		default: result=(27); break;
		}
		
		return result;
	}

	public bool Passable(Actor actor)
	{
		return tileType != TileType.SOLID || !actor.localInput;
	}
	

	public bool TryAdd(Actor actor)
	{
		if( Passable(actor) )
		{
			occupyList.Add(actor);
			return true;
		}
		return false;
	}

	public void Remove(Actor actor)
	{
		occupyList.Remove(actor);
	}
}
