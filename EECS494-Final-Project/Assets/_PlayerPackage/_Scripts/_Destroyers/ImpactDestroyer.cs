using UnityEngine;
using System.Collections;

public class ImpactDestroyer : MonoBehaviour 
{
	void Start()
	{
		Destroy(this.gameObject, 30);	
		//When instantiated, destroy yourself after 30 seconds.
	}
}
