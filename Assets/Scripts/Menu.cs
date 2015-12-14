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


	public void GoConnect()
	{
		NetMan.Instance.Launch();
		connectButton.gameObject.SetActive(false);
		desiredState = MenuState.CONNECTED;
	}

	public void GoSpawn()
	{
		cycleButton.gameObject.SetActive(false);
		spawnButton.gameObject.SetActive(false);
		Boss.Instance.RequestSpawn();
		desiredState = MenuState.SPAWNED;
	}

	public void Update()
	{
		if( menuState != desiredState )
		{
			switch(desiredState)
			{
				case MenuState.READY:
					menuState = desiredState;
					connectButton.gameObject.SetActive(true);
					cycleButton.gameObject.SetActive(false);
					spawnButton.gameObject.SetActive(false);
					break;
				case MenuState.CONNECTED:
					if (NetMan.Instance.isConnected)
					{
						menuState = desiredState;
						cycleButton.gameObject.SetActive(true);
						spawnButton.gameObject.SetActive(true);
					}
					break;
				case MenuState.SPAWNED:
					if (Boss.Instance.selectedHero != null)
					{
						menuState = desiredState;
					}
					break;
			}
		}
	}
}
