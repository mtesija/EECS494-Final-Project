using UnityEngine;
using System.Collections;

public class MainMenuSpawnerScript : MonoBehaviour
{
	float bulletSpeed = 10;

	public GameObject bullet;

	void Start()
	{
		GameObject redBullet = Instantiate(bullet, this.transform.position, Quaternion.identity) as GameObject;
		redBullet.rigidbody.velocity = new Vector3(Random.Range(.5f, 1), Random.Range(-1, -.25f), Random.Range(-1, 1));
		BulletScript redBulletScript = redBullet.GetComponent<BulletScript>();
		redBulletScript.SetColor(Color.red);
		redBulletScript.SetSpeed(bulletSpeed);
		
		GameObject greenBullet = Instantiate(bullet, this.transform.position, Quaternion.identity) as GameObject;
		greenBullet.rigidbody.velocity = new Vector3(Random.Range(-1, -.1f), Random.Range(.25f, 1), Random.Range(-1, -.5f));
		BulletScript greenBulletScript = greenBullet.GetComponent<BulletScript>();
		greenBulletScript.SetColor(Color.green);
		greenBulletScript.SetSpeed(bulletSpeed);
		
		GameObject blueBullet = Instantiate(bullet, this.transform.position, Quaternion.identity) as GameObject;
		blueBullet.rigidbody.velocity = new Vector3(Random.Range(-1, -.25f), Random.Range(.5f, 1), Random.Range(-1, 1));
		BulletScript blueBulletScript = blueBullet.GetComponent<BulletScript>();
		blueBulletScript.SetColor(Color.blue);
		blueBulletScript.SetSpeed(bulletSpeed);
		
		GameObject yellowBullet = Instantiate(bullet, this.transform.position, Quaternion.identity) as GameObject;
		yellowBullet.rigidbody.velocity = new Vector3(Random.Range(.1f, 1), Random.Range(-1, -.25f), Random.Range(.25f, 1));
		BulletScript yellowBulletScript = yellowBullet.GetComponent<BulletScript>();
		yellowBulletScript.SetColor(Color.yellow);
		yellowBulletScript.SetSpeed(bulletSpeed);
	}
}
