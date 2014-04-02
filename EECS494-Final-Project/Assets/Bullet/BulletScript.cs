using UnityEngine;
using System.Collections;

public class BulletScript : Photon.MonoBehaviour
{
	private float speed = 10;
	private Color color = Color.white;
	private int bounceCount = 0;

	private float hitLength = .4f;

	void FixedUpdate()
	{
		RaycastHit hit;
		if(Physics.Raycast(this.transform.position, this.rigidbody.velocity, out hit, hitLength))
		{
			if(hit.transform.CompareTag("Bullet"))
			{
				return;
			}
			else if(hit.transform.CompareTag("Player"))
			{
				PhotonNetwork.Destroy(this.gameObject);
				Destroy(this.gameObject);
			}
			else
			{
				bounceCount++;
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
