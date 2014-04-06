using UnityEngine;
using System.Collections;

public class BulletScript : Photon.MonoBehaviour
{
	public float speed = 10;
	public Color color = Color.white;

	private int bounceCount = 0;
	private const int MAX_BOUNCES = 10;

	private float hitLength = .4f;

	void Awake()
	{
		this.rigidbody.velocity = this.transform.up * speed;
	}

	void FixedUpdate()
	{
		RaycastHit hit;
		if(Physics.Raycast(this.transform.position, this.rigidbody.velocity, out hit, hitLength))
		{
			if(hit.transform.CompareTag("Player"))
			{
				if(photonView.isMine)
				{
					PhotonNetwork.Destroy(this.gameObject);
				}
				else
				{
					this.gameObject.SetActive(false);
				}
			}
			else if(bounceCount >= MAX_BOUNCES)
			{
				if(photonView.isMine)
				{
					PhotonNetwork.Destroy(this.gameObject);
				}
				else
				{
					this.gameObject.SetActive(false);
				}
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
