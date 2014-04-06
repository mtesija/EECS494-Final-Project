using UnityEngine;
using System.Collections;

public class BulletScript : Photon.MonoBehaviour
{
	private float speed = 10;
	private Color color = Color.white;

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

	[RPC]
	private void UpdateColor(Color inColor)
	{
		this.color = inColor;
		Material trailMaterial = this.GetComponent<TrailRenderer>().material;
		trailMaterial.SetColor("_TintColor", color);
	}

	public void SetColor(Color inColor)
	{
		//photonView.RPC("UpdateColor", PhotonTargets.All, inColor);
	}

	[RPC]
	private void UpdateSpeed(float inSpeed)
	{
		this.speed = inSpeed;
		this.rigidbody.velocity = this.rigidbody.velocity.normalized * speed;
	}

	public void SetSpeed(float inSpeed)
	{
		photonView.RPC("UpdateSpeed", PhotonTargets.All, inSpeed);
	}

	[RPC]
	private void UpdateSize(float inSize)
	{
		TrailRenderer trailRenderer = this.GetComponent<TrailRenderer>();
		trailRenderer.startWidth = inSize;
	}

	public void SetSize(float inSize)
	{
		photonView.RPC ("UpdateSize", PhotonTargets.All, inSize);
	}
}
