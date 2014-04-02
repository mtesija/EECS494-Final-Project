using UnityEngine;
using System.Collections;

public class DisablePlayerActions : Photon.MonoBehaviour
{
	void Awake()
	{
		if(!photonView.isMine)
		{
			this.GetComponent<MouseLook>().enabled = false;
			this.GetComponent<FPSInputController>().enabled = false;
			this.GetComponent<CharacterMotor>().enabled = false;
			this.GetComponent<CharacterController>().enabled = false;

			//this.GetComponentInChildren<MouseLook>().enabled = false;
			//this.GetComponentInChildren<PlayerShootScript>().enabled = false;
		}
	}
}
