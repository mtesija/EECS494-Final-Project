using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour
{
	float redTimer = 0;
	float greenTimer = 1;
	float blueTimer = 2;
	float yellowTimer = 3;
	
	bool redSpawn = false;
	bool greenSpawn = false;
	bool blueSpawn = false;
	bool yellowSpawn = false;

	float bulletSpeed = 10;

	public GameObject bullet;

	void Update()
	{
		redTimer -= Time.deltaTime;
		greenTimer -= Time.deltaTime;
		blueTimer -= Time.deltaTime;
		yellowTimer -= Time.deltaTime;
		
		if(redTimer <= 0 && !redSpawn)
		{
			redSpawn = true;
			GameObject redBullet = Instantiate(bullet, this.transform.position, Quaternion.identity) as GameObject;
			redBullet.rigidbody.velocity = new Vector3(1, 0, 1);
			BulletScript redBulletScript = redBullet.GetComponent<BulletScript>();
			redBulletScript.SetColor(Color.red);
			redBulletScript.SetSpeed(bulletSpeed);
		}

		if(greenTimer <= 0 && !greenSpawn)
		{
			greenSpawn = true;
			GameObject greenBullet = Instantiate(bullet, this.transform.position, Quaternion.identity) as GameObject;
			greenBullet.rigidbody.velocity = new Vector3(-1, 0, 1);
			BulletScript greenBulletScript = greenBullet.GetComponent<BulletScript>();
			greenBulletScript.SetColor(Color.green);
			greenBulletScript.SetSpeed(bulletSpeed);
		}

		if(blueTimer <= 0 && !blueSpawn)
		{
			blueSpawn = true;
			GameObject blueBullet = Instantiate(bullet, this.transform.position, Quaternion.identity) as GameObject;
			blueBullet.rigidbody.velocity = new Vector3(1, 0, -1);
			BulletScript blueBulletScript = blueBullet.GetComponent<BulletScript>();
			blueBulletScript.SetColor(Color.blue);
			blueBulletScript.SetSpeed(bulletSpeed);
		}

		if(yellowTimer <= 0 && !yellowSpawn)
		{
			yellowSpawn = true;
			GameObject yellowBullet = Instantiate(bullet, this.transform.position, Quaternion.identity) as GameObject;
			yellowBullet.rigidbody.velocity = new Vector3(-1, 0, -1);
			BulletScript yellowBulletScript = yellowBullet.GetComponent<BulletScript>();
			yellowBulletScript.SetColor(Color.yellow);
			yellowBulletScript.SetSpeed(bulletSpeed);
		}
	}
}
