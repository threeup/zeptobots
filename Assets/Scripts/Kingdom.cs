using UnityEngine;
using System.Collections;

public class Kingdom : MonoBehaviour {

	public int ownerOID = -1;

	public Tile tile;

	private float spawnTimer = 5f;


	void Start()
	{
		tile = this.transform.parent.GetComponent<Tile>();
	}

	public void RequestKingdom(int localOID, bool localIsRed)
	{

		ownerOID = localOID;
		string actorSprite = localIsRed ? "Rd1" : "Bd1";
		NetMan.Instance.Send("requestactoradd|"+ownerOID+"|"+tile.rtx+"|"+tile.rty+"|"+actorSprite);
		string worldSprite = localIsRed ? "R" : "B";
		NetMan.Instance.Send("requestworldmod|"+tile.sx+"|"+tile.sy+"|"+tile.ltx+"|"+tile.lty+"|"+worldSprite);
	}

	public void ConquerKingdom(int localOID, bool localIsRed)
	{
		ownerOID = localOID;
		string worldSprite = localIsRed ? "R" : "B";
		NetMan.Instance.Send("requestworldmod|"+tile.sx+"|"+tile.sy+"|"+tile.ltx+"|"+tile.lty+"|"+worldSprite);
	}

	public void Update()
	{
		if( ownerOID > 0 )
		{
			spawnTimer -= Time.deltaTime;
			if( spawnTimer < 0f)
			{
				spawnTimer = 20f;
				string actorSprite = "H";
				NetMan.Instance.Send("requestactoradd|"+ownerOID+"|"+tile.rtx+"|"+tile.rty+"|"+actorSprite);
			}
		}
	}

	public void Added(Actor a)
	{
		Debug.Log("Somone is here");
	}
}
