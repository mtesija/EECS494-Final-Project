using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public PauseScript pauseScript;
	public PlayerScript playerScript;
	
	void Start()
	{
		DataManager.getInstance().Reset();	
		//Reset the game components at the start of the game, creating a clean and fresh environment to play in.
		
		pauseScript = FindObjectOfType(typeof(PauseScript)) as PauseScript;
		playerScript = FindObjectOfType(typeof(PlayerScript)) as PlayerScript;
	}
	
	void Update()
	{	
		//Lock cursor if game is playing.
		if(!pauseScript.pause)
		{
			Screen.lockCursor = true;	
		}
		
		if(DataManager.getInstance().enemiesKilled >= 1 || DataManager.getInstance().friendliesKilled >= 1)
		{
			if(!pauseScript.pause)
			{
				if(!DataManager.getInstance().endGame)
				{
					DataManager.getInstance().gameTimer += 1 * Time.deltaTime;		
				}
			}
		}
		
		if(DataManager.getInstance().enemiesKilled == 23)
		{
			DataManager.getInstance().endGame = true;
			
		}
	}
	
	void OnGUI()
	{
		//GUI.Label(new Rect(Screen.width * 0.15f, Screen.height * 0.75f , 300, 100), "Bullets left: "      + playerScript.bulletCounter);
		
		//GUI.Label(new Rect(Screen.width * 0.15f, Screen.height * 0.7f  , 300, 100), "Magazines left: "    + playerScript.magazineCounter);
		/*
		GUI.Label(new Rect(Screen.width * 0.15f, Screen.height * 0.35f , 300, 100), "TERRORISTS KILLED: " + DataManager.getInstance().enemiesKilled);
		
		GUI.Label(new Rect(Screen.width * 0.15f, Screen.height * 0.4f  , 300, 100), "CIVILLIANS KILLED: " + DataManager.getInstance().friendliesKilled);
		
		GUI.Label(new Rect(Screen.width * 0.15f, Screen.height * 0.5f , 300, 100), "Press ESC to restart the game");
		
		GUI.Label(new Rect(Screen.width * 0.15f, Screen.height * 0.55f , 300, 100), "Shoot at a ball to start the timer. Shoot all the balls to view your total score");
		
		GUI.Label(new Rect(Screen.width * 0.15f, Screen.height * 0.6f  , 300, 100), "Game Timer: " + DataManager.getInstance().gameTimer.ToString("F2")); //F2 means, show 2 decimals behind the Timer. (x.xx format).
		*/
	}
}
