using UnityEngine;
using System.Collections;

public class PlayerDataScript : MonoBehaviour {

	public Color playerColor = Color.white;
	
	public bool collectHitData = false;
	public bool collectDeathData = true;
	public bool collectBounceData = false;
	public int playTo = 10;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}
}
