using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour 
{
	public bool pause = false;
	private bool pauseScreen = false;
	private bool quitScreen = false;
	
	public bool continueScreen = false;
	
	public MouseLook pauseLookingX;
	public MouseLook pauseLookingY;
	
	public PlayerScript playerScript;
	public CharacterMotor characterMotor;
	
	void Start()
	{
		playerScript = FindObjectOfType(typeof(PlayerScript)) as PlayerScript;
		characterMotor = FindObjectOfType(typeof(CharacterMotor)) as CharacterMotor;
		
		continueScreen = true;
		pause = true;
	}
	
	void Update () 
	{		
		if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape) || continueScreen)
		{
			//When paused, disable everything the player can do. Looking, Moving, etc. And enable the mouse cursor.
			if(pause == false)
			{
				characterMotor.canControl = false;

				pause = true;
				
				if(!DataManager.getInstance().endGame)
				{
					pauseScreen = true;	
				}
				
				Screen.lockCursor = false;
				Screen.showCursor = true;
				
				pauseLookingX.enabled = false;
				pauseLookingY.enabled = false;
				
			} else {
			
				if(!DataManager.getInstance().endGame)
				{
					//When unpaused, resume everything the player can do. Looking, Moving, etc. And enable the mouse cursor.
					characterMotor.canControl = true;
					
					pause = false;
					pauseScreen = false;
					quitScreen = false;
					
					Screen.lockCursor = true;
					Screen.showCursor = false;
					
					pauseLookingX.enabled = true;
					pauseLookingY.enabled = true;
					
				}
			}
			continueScreen = false;
		}
	}
	
	//Pause Menu with buttons the player can interact with.
	void OnGUI()
	{
		if(pauseScreen)
		{
			GUI.Box(new Rect(Screen.width * 0.4f, Screen.height * 0.4f, Screen.width * 0.25f, Screen.height * 0.35f), "The game has been paused");
			if(GUI.Button(new Rect(Screen.width * 0.45f , Screen.height * 0.45f , Screen.width * 0.15f, Screen.height * 0.10f), "Continue"))
			{	
				continueScreen = true;
				pauseScreen = false;
			}
			
			if(GUI.Button(new Rect(Screen.width * 0.45f , Screen.height * 0.55f , Screen.width * 0.15f, Screen.height * 0.10f), "Restart"))
			{	
				Application.LoadLevel(Application.loadedLevel);
			}
			
			if(GUI.Button(new Rect(Screen.width * 0.45f , Screen.height * 0.65f , Screen.width * 0.15f, Screen.height * 0.10f), "Quit"))
			{
				pauseScreen = false;
				quitScreen = true;
			}
		}
		
		if(quitScreen)
		{
			GUI.Box(new Rect(Screen.width * 0.4f, Screen.height * 0.4f, Screen.width * 0.25f, Screen.height * 0.25f), "Are you sure you want to quit?");
			if(GUI.Button(new Rect(Screen.width * 0.45f , Screen.height * 0.45f , Screen.width * 0.15f, Screen.height * 0.10f), "Quit"))
			{
				quitScreen = false;
				//Application.Quit();
				Application.LoadLevel("MainMenu");
			}
			
			if(GUI.Button(new Rect(Screen.width * 0.45f , Screen.height * 0.55f , Screen.width * 0.15f, Screen.height * 0.10f), "Back"))
			{
				quitScreen = false;
				pauseScreen = true;
			}
		}
	}
}
