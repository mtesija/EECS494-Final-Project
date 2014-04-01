using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour 
{
	public PauseScript pauseScript;
	
	private bool _starOne = false;
	private bool _starTwo = false;
	private bool _starThree = false;
	
	private bool _checkStarsOnce = true;
	
	public GUISkin mySkin;
	
	void Start()
	{
		pauseScript = FindObjectOfType(typeof(PauseScript)) as PauseScript;
	}
	
	void Update()
	{
		//If the player has ended the game, it continues to update the continueScreen bool to true, so that the player cant get out of the endScreen state. Only by restarting / quitting.
		if(DataManager.getInstance().endGame)
		{
			pauseScript.continueScreen = true;
		}
	}
	
	void OnGUI()
	{
		GUI.skin = mySkin;
		
		if(DataManager.getInstance().endGame)
		{
			//641 and 714 are the exact width and height of the GUI Image.
			//The GUI image can be accessed by: (Project Folder): mySkin -> Custom Styles -> EndGameScreen.
			GUI.Box(new Rect(Screen.width * 0.25f, Screen.height * 0.075f, 641, 714), "", "EndGameScreen");
			if(GUI.Button(new Rect(Screen.width * 0.40f , Screen.height * 0.7f , Screen.width * 0.15f, Screen.height * 0.10f), "Restart"))
			{
				Application.LoadLevel(Application.loadedLevel);
				DataManager.getInstance().endGame = false;
			}
			
			if(GUI.Button(new Rect(Screen.width * 0.40f , Screen.height * 0.8f , Screen.width * 0.15f, Screen.height * 0.10f), "Quit"))
			{
				Application.LoadLevel("MainMenu");
				//Application.Quit();
			}
			
			
			//End game Star calculation based on 3 variables. Amount of Hostages killed, Amount of Enemies killed and the time this all took place in.
			if(_checkStarsOnce) //Execute this star checking process once.
			{
				if(DataManager.getInstance().friendliesKilled >= 3 && DataManager.getInstance().friendliesKilled <= 4) //3 || 4
				{
					if(DataManager.getInstance().enemiesKilled >= 10)
					{
						COUROTINE_StarOne();
						print ("1 Medal");	
					}
					else
					{
						print ("No Medal");
					}
				}
				else if(DataManager.getInstance().friendliesKilled >= 1 && DataManager.getInstance().friendliesKilled <= 2) //1 || 2
				{
					if(DataManager.getInstance().enemiesKilled >= 15)
					{
						if(DataManager.getInstance().gameTimer <= 40)
						{
							COUROTINE_StarTwo();
							print ("2 Medal");
						}
						else
						{
							COUROTINE_StarOne();
							print ("1 Medal");
						}
					}
					else if(DataManager.getInstance().enemiesKilled >= 10 && DataManager.getInstance().enemiesKilled <= 14)
					{
						COUROTINE_StarOne();
						print ("1 Medal");
					}
					else
					{
						print ("No Medal");
					}
				}
				else if(DataManager.getInstance().friendliesKilled == 0) //0
				{
					if(DataManager.getInstance().enemiesKilled >= 20)
					{
						if(DataManager.getInstance().gameTimer <= 40)
						{
							COUROTINE_StarThree();
							print ("3 Medal");
						}
						else
						{
							COUROTINE_StarTwo();
							print ("2 Medal");
						}
					}
					else if(DataManager.getInstance().enemiesKilled >= 10 && DataManager.getInstance().enemiesKilled <= 19)
					{
						COUROTINE_StarTwo();
						print ("2 Medal");
					}
					else if(DataManager.getInstance().enemiesKilled >= 1 && DataManager.getInstance().enemiesKilled <= 9)
					{
						COUROTINE_StarOne();
						print ("1 Medal");
					}
					else
					{
						print ("No Medal");
					}
				}
				else 
				{
					print ("No Medal");
				}
				
				_checkStarsOnce = false; //Execute this star checking process once.
			}
			
			//Yellow stars that spawn ontop of the grey stars.
			//198 and 198 are the exact width and height of the GUI Image.
			//The GUI image can be accessed by: (Project Folder): mySkin -> Custom Styles -> GoldStar.
			//Positions of the stars are different on a different resolution. These positions are made for a resolution of 1400 x 800.
			if(_starOne)
			{
				GUI.Box(new Rect(Screen.width * 0.282f , Screen.height * 0.395f , 198, 198), "", "GoldStar");
			}
			
			if(_starTwo)
			{
				GUI.Box(new Rect(Screen.width * 0.415f , Screen.height * 0.395f , 198, 198), "", "GoldStar");
			}
			
			if(_starThree)
			{
				GUI.Box(new Rect(Screen.width * 0.546f , Screen.height * 0.395f , 198, 198), "", "GoldStar");
			}
		}
	}
	
	//When there functions are executed, they execute a timed function trigger (StartCoroutine).
	private void COUROTINE_StarOne()
	{
		StartCoroutine(PopOneStar());
	}
	
	private void COUROTINE_StarTwo()
	{
		StartCoroutine(PopTwoStars());
	}
	
	private void COUROTINE_StarThree()
	{
		StartCoroutine(PopThreeStars());
	}
	
	//When triggered, countdown, and wait for seperate timers to give a Pop-Up 1 by 1 star effect at the end of the game. 
	private IEnumerator PopOneStar()
	{
		yield return new WaitForSeconds(0.5f);
		_starOne = true;
	}
	
	private IEnumerator PopTwoStars()
	{
		yield return new WaitForSeconds(0.5f);
		_starOne = true;
		yield return new WaitForSeconds(0.5f);
		_starTwo = true;
	}
	
	private IEnumerator PopThreeStars()
	{
		yield return new WaitForSeconds(0.5f);
		_starOne = true;
		yield return new WaitForSeconds(0.5f);
		_starTwo = true;
		yield return new WaitForSeconds(0.5f);
		_starThree = true;		
	}
}
