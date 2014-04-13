using UnityEngine;
using System.Collections;

public class PlayerDataScript : MonoBehaviour {

	public Color playerColor = Color.white;

	public bool collectData = true;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}
}
