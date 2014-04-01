using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour 
{
	private Color shotColor = new Color(0.5f, 0.5f, 0.5f, 1);
	public bool isShot = false;
	
	void Update()
	{
		if(isShot)
		{
			this.renderer.material.color = shotColor;
			//Positive Score Sound.
		}
	}
	
	//DO NOT DELETE.
	//Script to help determine which target is being looked at and shot at.
	//Further use of this script is found in the PlayerScript.cs document.
}