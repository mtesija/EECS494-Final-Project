using UnityEngine;
using System.Collections;

public class SpawnRandomizer : MonoBehaviour {
	
	public SpawnPlayer spawn1;
	public SpawnPlayer spawn2;
	public SpawnPlayer spawn3;
	public SpawnPlayer spawn4;
	public SpawnPlayer spawn5;
	public SpawnPlayer spawn6;
	public SpawnPlayer spawn7;
	public SpawnPlayer spawn8;

	// Use this for initialization
	void Awake() {
		Spawn ();
	}
	
	// Update is called once per frame
	public void Spawn () 
	{
		int r = Mathf.Abs(Random.Range (-7, 7));

		if(r == 0)
		{
			spawn8.spawn();
		}
		else if(r == 1)
		{
			spawn1.spawn();
		}
		else if(r == 2)
		{
			spawn2.spawn();
		}
		else if(r == 3)
		{
			spawn3.spawn();
		}
		else if(r == 4)
		{
			spawn4.spawn();
		}
		else if(r == 5)
		{
			spawn5.spawn();
		}
		else if(r == 6)
		{
			spawn6.spawn();
		}
		else if(r == 7)
		{
			spawn7.spawn();
		}
	}
}
