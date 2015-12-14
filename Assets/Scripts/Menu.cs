using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour {

	public static Menu Instance;
	public enum MenuState
	{
		NONE,
		READY,
		CONNECTED,
		SPAWNED,
		DEAD,
	}

	public MenuState menuState = MenuState.NONE;
	public MenuState desiredState = MenuState.READY;

	public Button connectButton;
	public Button cycleButton;
	public Button spawnButton;
	public Button timeoutButton;
	public Image background;
	public Image buttonA;
	public Image buttonB;

	private float timeoutTime = -1f;

	void Awake()
	{
		Instance = this;
	}


	public void GoConnect()
	{
		NetMan.Instance.Launch();
		connectButton.gameObject.SetActive(false);
		desiredState = MenuState.CONNECTED;
		timeoutTime = 3f;
	}

	public void GoSpawn()
	{
		cycleButton.gameObject.SetActive(false);
		spawnButton.gameObject.SetActive(false);
		Boss.Instance.RequestSpawn();
		desiredState = MenuState.SPAWNED;
		timeoutTime = 3f;
	}

	public void Update()
	{
		if( timeoutTime > 0f )
		{
			timeoutTime -= Time.deltaTime;
			if (timeoutTime <= 0f)
			{
				timeoutButton.gameObject.SetActive(true);
			} 
		}
		if( menuState != desiredState )
		{
			switch(desiredState)
			{
				case MenuState.READY:
					menuState = desiredState;
					connectButton.gameObject.SetActive(true);
					cycleButton.gameObject.SetActive(false);
					spawnButton.gameObject.SetActive(false);
					buttonA.gameObject.SetActive(false);
					buttonB.gameObject.SetActive(false);
					timeoutButton.gameObject.SetActive(false);
					background.gameObject.SetActive(true);
					break;
				case MenuState.CONNECTED:
					if (NetMan.Instance.isConnected )
					{
						Boss.Instance.SelectRandomSpawn();
						if( Boss.Instance.selectedKingdom != null )
						{
							menuState = desiredState;
							cycleButton.gameObject.SetActive(true);
							spawnButton.gameObject.SetActive(true);
							timeoutButton.gameObject.SetActive(false);
							background.gameObject.SetActive(false);
						}
					}
					break;
				case MenuState.SPAWNED:
					if (Boss.Instance.selectedHero != null)
					{
						menuState = desiredState;
						buttonA.gameObject.SetActive(true);
						buttonB.gameObject.SetActive(true);
						timeoutButton.gameObject.SetActive(false);
					}
					break;
			}
		}
	}

	public void SetButtonA(bool val)
	{
		if( val )
		{
			buttonA.color = Color.green;
		}
		else
		{
			buttonA.color = Color.white;
		}
	}

	public void SetButtonB(bool val)
	{
		if( val )
		{
			buttonB.color = Color.green;
		}
		else
		{
			buttonB.color = Color.white;
		}
	}
}
