using UnityEngine;
using System.Collections;

public class PauseScript : Photon.MonoBehaviour 
{
	public bool pause;
	public bool continueScreen;

	private bool quitScreen = false;
	
	public bool menuOpen = false;

	public PlayerShootScript2 shoot;
	public MouseLook mousey;
	public MouseLook mousex;

	void Update() 
	{		
		if(menuOpen)
		{
			shoot.enabled = false;
			mousex.enabled = false;
			mousey.enabled = false;
			Screen.showCursor = true;
			Screen.lockCursor = false;
		}
		else if(!quitScreen)
		{
			shoot.enabled = true;
			mousex.enabled = true;
			mousey.enabled = true;
			Screen.showCursor = false;
			Screen.lockCursor = true;
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			menuOpen = !menuOpen;
		}
	}

	void OnGUI()
	{
		if(menuOpen)
		{
			GUI.Box(new Rect(Screen.width * 0.4f, Screen.height * 0.4f, Screen.width * 0.25f, Screen.height * 0.25f), "Menu");
			if(GUI.Button(new Rect(Screen.width * 0.45f , Screen.height * 0.45f , Screen.width * 0.15f, Screen.height * 0.10f), "Continue"))
			{	
				menuOpen = false;
			}
			
			if(GUI.Button(new Rect(Screen.width * 0.45f , Screen.height * 0.55f , Screen.width * 0.15f, Screen.height * 0.10f), "Quit"))
			{
				menuOpen = false;
				quitScreen = true;
			}
		}
		
		if(quitScreen)
		{
			GUI.Box(new Rect(Screen.width * 0.4f, Screen.height * 0.4f, Screen.width * 0.25f, Screen.height * 0.25f), "Are you sure?");
			if(GUI.Button(new Rect(Screen.width * 0.45f , Screen.height * 0.45f , Screen.width * 0.15f, Screen.height * 0.10f), "Quit"))
			{
				quitScreen = false;

				GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

				foreach(GameObject bullet in bullets)
				{
					PhotonNetwork.Destroy(bullet);
				}
				
				GameObject[] effects = GameObject.FindGameObjectsWithTag("hitEffect");
				
				foreach(GameObject effect in effects)
				{
					PhotonNetwork.Destroy(effect);
				}

				PhotonNetwork.Destroy(this.gameObject);
				PhotonNetwork.LeaveRoom();
				PhotonNetwork.LoadLevel("_MainMenu");
			}
			
			if(GUI.Button(new Rect(Screen.width * 0.45f , Screen.height * 0.55f , Screen.width * 0.15f, Screen.height * 0.10f), "Back"))
			{
				quitScreen = false;
				menuOpen = true;
			}
		}
	}

	void OnLeaveRoom()
	{
		PhotonNetwork.LoadLevel("_MainMenu");
	}
}
