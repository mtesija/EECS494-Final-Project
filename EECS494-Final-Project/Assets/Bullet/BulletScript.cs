using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
	private float speed = 10;
	private Color color = Color.white;

	private float hitLength = .4f;

	void FixedUpdate()
	{
		Vector3 position = this.transform.position;
		if(position.x > 500 || position.x < -500 || position.y > 500  || position.y < -500 || position.z > 500 || position.z < -500)
		{
			Destroy(this.gameObject);
		}

		RaycastHit hit;
		if(Physics.Raycast(this.transform.position, this.rigidbody.velocity, out hit, hitLength))
		{
			if(hit.transform.CompareTag("Bullet"))
			{
				return;
			}
			else if(hit.transform.CompareTag("Player"))
			{
				Destroy(this.gameObject);
			}

			this.rigidbody.velocity = Vector3.Reflect(this.rigidbody.velocity, hit.normal).normalized * speed;
		}
	}

	public void SetColor(Color inColor)
	{
		color = inColor;
		Material trailMaterial = this.GetComponent<TrailRenderer>().material;
		trailMaterial.SetColor("_TintColor", color);
	}

	public void SetSpeed(float inSpeed)
	{
		speed = inSpeed;
		this.rigidbody.velocity = this.rigidbody.velocity.normalized * speed;
	}
}
