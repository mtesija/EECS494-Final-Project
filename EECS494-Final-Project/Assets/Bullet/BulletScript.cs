using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
	private float speed = 10;
	private Color color = Color.white;

	private float hitLength = .2f;

	void FixedUpdate()
	{
		RaycastHit hit;
		if(Physics.Raycast(this.transform.position, this.rigidbody.velocity, out hit, hitLength))
		{
			if(hit.transform.tag == "Bullet")
			{
				return;
			}
			else if(hit.transform.tag == "Player")
			{
				//Player got hit here
			}

			this.rigidbody.velocity = Vector3.Reflect(this.rigidbody.velocity, hit.normal).normalized * speed;
		}
	}

	public void SetColor(Color inColor)
	{
		color = inColor;
		Material trailMaterial = this.GetComponent<TrailRenderer>().material;
		trailMaterial.color = color;
	}

	public void SetSpeed(float inSpeed)
	{
		speed = inSpeed;
		this.rigidbody.velocity = this.rigidbody.velocity.normalized * speed;
	}
}
