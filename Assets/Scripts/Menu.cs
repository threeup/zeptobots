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

	public MenuFactory factory;
	public GameObject titleSplash;
	public Button timeoutButton;
	private UIFace connectButton;
	private UIFace localButton;
	private UIFace cycleButton;
	private UIFace spawnButton;
	
	private float timeoutTime = -1f;

	private bool isInitialized = false;

	void Awake()
	{
		Instance = this;

	}
	void Start()
	{
		Transform root = this.transform;

		connectButton = factory.AddFrontButton(root, -170f, 70f, "Connect To Server");
		connectButton.btn.onClick.AddListener(delegate {this.GoConnect();});
		localButton = factory.AddFrontButton(root, 170f, 70f, "Local Playground");
		localButton.btn.onClick.AddListener(delegate {this.GoLocal();});
		cycleButton = factory.AddFrontButton(root, -170f, -70f, "Cycle Spawn");
		cycleButton.btn.onClick.AddListener(delegate {Boss.Instance.SelectRandomSpawn();});
		spawnButton = factory.AddFrontButton(root, 170f, -70f, "Jump In!");
		spawnButton.btn.onClick.AddListener(delegate {this.GoSpawn();});

		isInitialized = true;
	}

	public void GoConnect()
	{
		NetMan.Instance.Launch(false);
		connectButton.gameObject.SetActive(false);
		localButton.gameObject.SetActive(false);
		desiredState = MenuState.CONNECTED;
		timeoutTime = 8f;
	}

	public void GoLocal()
	{
		NetMan.Instance.Launch(true);
		connectButton.gameObject.SetActive(false);
		localButton.gameObject.SetActive(false);
		desiredState = MenuState.CONNECTED;
		timeoutTime = 8f;
	}

	public void GoSpawn()
	{
		cycleButton.gameObject.SetActive(false);
		spawnButton.gameObject.SetActive(false);
		Boss.Instance.RequestSpawn();
		desiredState = MenuState.SPAWNED;
		timeoutTime = 8f;
	}

	public void Update()
	{
		if( !isInitialized )
		{
			return;
		}
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
					localButton.gameObject.SetActive(true);
					cycleButton.gameObject.SetActive(false);
					spawnButton.gameObject.SetActive(false);
					timeoutButton.gameObject.SetActive(false);
					titleSplash.gameObject.SetActive(true);
					timeoutTime = -1f;
					break;
				case MenuState.CONNECTED:
					if (NetMan.Instance.IsConnected)
					{
						Boss.Instance.SelectRandomSpawn();
						if( Boss.Instance.selectedKingdom != null )
						{
							menuState = desiredState;
							Hud.Instance.SetActive(false);
							cycleButton.gameObject.SetActive(true);
							spawnButton.gameObject.SetActive(true);
							timeoutButton.gameObject.SetActive(false);
							titleSplash.gameObject.SetActive(false);
							timeoutTime = -1f;
						}
					}
					break;
				case MenuState.SPAWNED:
					if (Boss.Instance.selectedHero != null)
					{
						menuState = desiredState;
						Hud.Instance.SetActive(true);
						timeoutButton.gameObject.SetActive(false);
						timeoutTime = -1f;
					}
					break;
			}
		}
	}

	
}
