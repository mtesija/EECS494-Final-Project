using UnityEngine;
using System.Collections;

public class PlayerDataScript : MonoBehaviour {

	public Color playerColor = Color.white;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}
}
