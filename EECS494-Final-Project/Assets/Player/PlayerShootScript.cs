﻿using UnityEngine;
using System.Collections;

public class PlayerShootScript : Photon.MonoBehaviour
{
	float shootDelayTimer = .5f;

	public GameObject player;

	void Start()
	{
	}

	void Update()
	{
		Screen.lockCursor = true;

		if(shootDelayTimer > 0)
		{
			shootDelayTimer -= Time.deltaTime;
		}
		else
		{
			if(Input.GetMouseButtonDown(0))
			{
				shootDelayTimer = .5f;

				GameObject bullet = PhotonNetwork.Instantiate("Bullet", this.transform.position, Quaternion.identity, 0) as GameObject;
				BulletScript bulletScript = bullet.GetComponent<BulletScript>();
				bullet.rigidbody.velocity = this.transform.position - player.transform.position;
				bulletScript.SetColor(Color.green);
				bulletScript.SetSpeed(10);
			}
		}
	}
}
