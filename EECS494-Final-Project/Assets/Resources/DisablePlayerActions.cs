using UnityEngine;
using System.Collections;

public class DisablePlayerActions : Photon.MonoBehaviour
{
	void Update()
	{
		if(!photonView.isMine)
		{
			this.gameObject.layer = 10;

			this.GetComponent<CapsuleCollider>().enabled = true;

			this.GetComponent<MouseLook>().enabled = false;
			this.GetComponent<FPSInputController>().enabled = false;
			this.GetComponent<CharacterMotor>().enabled = false;
			this.GetComponent<CharacterController>().enabled = false;
			this.GetComponent<PlayerManager>().enabled = false;
			
			this.GetComponentInChildren<Camera>().enabled = false;
			this.GetComponentInChildren<GUILayer>().enabled = false;
			this.GetComponentInChildren<AudioListener>().enabled = false;

			this.GetComponentInChildren<ShieldScript>().enabled = false;

			this.GetComponentInChildren<MouseLook>().enabled = false;
			this.GetComponentInChildren<PlayerShootScript>().enabled = false;
		}
	}
}
