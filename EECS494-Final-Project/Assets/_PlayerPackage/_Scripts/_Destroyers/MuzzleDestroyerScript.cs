using UnityEngine;
using System.Collections;

public class MuzzleDestroyerScript : MonoBehaviour 
{
	void Start()
	{
		Destroy(this.gameObject, 0.02f);	
		//When instantiated, destroy yourself after 0.02 seconds.
	}
}
