﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class BulletScript : Photon.MonoBehaviour
{
	private float speed = 10;
	private Color color = Color.white;

	private int bounceCount = 0;
	private const int MAX_BOUNCES = 10;

	private float hitLength = .4f;

	private bool isOwner = false;

	private Vector3 serverPosition;
	private Vector3 clientPosition;
	
	private Vector3 serverVelocity;
	
	private float lerpValue = 0;

	void Awake()
	{
		if(photonView.isMine)
		{
			this.isOwner = true;
			this.rigidbody.velocity = this.transform.up * speed;
		}
		else
		{
			this.serverPosition = this.transform.position;
			this.clientPosition = this.transform.position;
		}
	}

	void FixedUpdate()
	{
		if(this.isOwner)
		{
			RaycastHit hit;
			if(Physics.Raycast(this.transform.position, this.rigidbody.velocity, out hit, hitLength))
			{
				if(hit.transform.CompareTag("Player"))
				{
					//PhotonNetwork.Destroy(this.gameObject);
				}
				else if(bounceCount >= MAX_BOUNCES)
				{
					//PhotonNetwork.Destroy(this.gameObject);
				}
				else
				{
					bounceCount++;
				}

				this.rigidbody.velocity = Vector3.Reflect(this.rigidbody.velocity, hit.normal).normalized * speed;
			}
		}
		else
		{
			this.lerpValue += Time.deltaTime * 9;
			
			this.rigidbody.velocity = this.serverVelocity;
			this.transform.position = Vector3.Lerp(this.clientPosition, this.serverPosition, this.lerpValue);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(this.isOwner)
		{
			return;
		}

		if(stream.isWriting)
		{			
			stream.SendNext(this.rigidbody.velocity);
			stream.SendNext(this.transform.position);
		}
		else
		{
			this.serverVelocity = (Vector3) stream.ReceiveNext();
			this.serverPosition = (Vector3) stream.ReceiveNext();
			
			this.clientPosition = this.transform.position;
			
			this.lerpValue = 0;
		}
	}

	[RPC]
	void SetColor(float r, float g, float b, float a)
	{
		this.color = new Color(r, g, b, a);
		Material trailMaterial = this.GetComponent<TrailRenderer>().material;
		trailMaterial.SetColor("_TintColor", color);
	}

	[RPC]
	private void SetSpeed(float inSpeed)
	{
		this.speed = inSpeed;
		this.rigidbody.velocity = this.rigidbody.velocity.normalized * speed;
	}

	[RPC]
	private void SetSize(float inSize)
	{
		TrailRenderer trailRenderer = this.GetComponent<TrailRenderer>();
		trailRenderer.startWidth = inSize;
	}
}