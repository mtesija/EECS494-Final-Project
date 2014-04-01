using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	private bool mainMenuScreen = true;
	private bool controlsScreen = false;
	
	void OnGUI()
	{
		//Main menu screen.
		if(mainMenuScreen)
		{
			GUI.Box(new Rect(Screen.width * 0.25f, Screen.height * 0.25f, Screen.width * 0.50f, Screen.height * 0.50f), "Main Menu");
			
			if(GUI.Button(new Rect(Screen.width * 0.3f , Screen.height * 0.3f , Screen.width * 0.15f, Screen.height * 0.10f), "Play"))
			{				
				Application.LoadLevel("Game");
			}
			
			if(GUI.Button(new Rect(Screen.width * 0.3f , Screen.height * 0.45f , Screen.width * 0.15f, Screen.height * 0.10f), "Options"))
			{
				mainMenuScreen = false;
				controlsScreen = true;
			}
			
			if(GUI.Button(new Rect(Screen.width * 0.3f , Screen.height * 0.6f , Screen.width * 0.15f, Screen.height * 0.10f), "Quit"))
			{
				Application.Quit();
			}
		}
		
		//Controls screen.
		if(controlsScreen)
		{
			GUI.Box(new Rect(Screen.width * 0.25f, Screen.height * 0.25f, Screen.width * 0.50f, Screen.height * 0.50f), "Controls");
			if(GUI.Button(new Rect(Screen.width * 0.3f , Screen.height * 0.3f , Screen.width * 0.15f, Screen.height * 0.10f), "Back"))
			{
				mainMenuScreen = true;
				controlsScreen = false;
			}
		}
	}
}