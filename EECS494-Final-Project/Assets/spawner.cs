using UnityEngine;
using System.Collections;

public class spawner : MonoBehaviour {

	public GameObject bullet;

	private float timer = 0;

	void Update()
	{
		timer -= Time.deltaTime;

		if(timer <= 0)
		{
			GameObject bulletClone = Instantiate(bullet, this.transform.position, Quaternion.identity) as GameObject;
			BulletScript script = bulletClone.GetComponent<BulletScript>();

			float rand = Mathf.Floor(Random.Range(0, 3.999f));
			if(rand == 0)
			{
				script.x = 1;
				script.y = 1;
			}
			else if(rand == 1)
			{
				script.x = -1;
				script.y = 1;
			}
			else if(rand == 2)
			{
				script.x = 1;
				script.y = -1;
			}
			else if(rand == 3)
			{
				script.x = -1;
				script.y = -1;
			}

			timer = 2;
		}
	}
}
