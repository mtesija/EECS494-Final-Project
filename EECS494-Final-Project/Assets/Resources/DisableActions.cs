using UnityEngine;
using System.Collections;

public class DisableActions : Photon.MonoBehaviour {

	public GameObject pivot;
	public GameObject camera;
	public GameObject laser;
	public GameObject shield;

	void Awake()
	{
		if(photonView.isMine)
		{
			return;
		}

		this.GetComponent<CapsuleCollider>().enabled = true;

		this.GetComponent<PlayerManager>().enabled = false;
		this.GetComponent<MouseLook>().enabled = false;
		this.GetComponent<PlayerColorScript>().enabled = false;
		this.GetComponent<CharacterController>().enabled = false;
		this.GetComponent<CharacterMotor>().enabled = false;
		this.GetComponent<FPSInputController>().enabled = false;

		pivot.GetComponent<MouseLook>().enabled = false;

		camera.SetActive(false);

		laser.SetActive(false);

		shield.SetActive(false);


	}
}
