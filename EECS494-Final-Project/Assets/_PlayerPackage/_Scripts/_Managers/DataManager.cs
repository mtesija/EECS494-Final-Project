using UnityEngine;
using System.Collections;

public class DataManager
{
	public bool endGame = false;
	public float gameTimer;
	
	public int enemiesKilled = 0;			//Number of enemies killed.
	public int friendliesKilled = 0;		//Number of civillians killed.
	
	private static DataManager _INSTANCE;
	
	public DataManager()
	{
		if (DataManager._INSTANCE != null)
		{
			Debug.LogError("Singleton use DataManager.getInstance()");
		}
	}
	
	public static DataManager getInstance()
	{
		if (DataManager._INSTANCE == null)
		{
			DataManager._INSTANCE = new DataManager();
		}
		
		return DataManager._INSTANCE;
	}
	
	public void Reset()
	{
		endGame = false;
		gameTimer = 0f;
		friendliesKilled = 0;
		enemiesKilled = 0;
	}
}