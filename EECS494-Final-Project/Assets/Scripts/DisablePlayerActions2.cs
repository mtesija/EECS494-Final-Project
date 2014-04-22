using UnityEngine;
using System.Collections;

public class DisablePlayerActions2 : Photon.MonoBehaviour
{
	void Update()
	{
		
		if(!photonView.isMine)
		{	
			this.gameObject.layer = 10;
		
			this.GetComponent<GameManager>().enabled = false;
			this.GetComponent<PauseScript>().enabled = false;
			this.GetComponent<EndGame>().enabled = false;
			this.GetComponent<PlayerManager>().enabled = false;
			this.GetComponent<CharacterController>().enabled = false;
			this.GetComponent<MouseLook>().enabled = false;
			this.GetComponent<CharacterMotor>().enabled = false;
			this.GetComponent<FPSInputController>().enabled = false;
			//this.GetComponent<CapsuleCollider>().enabled = true;
			this.GetComponent<Animation_network>().enabled = false;

			this.GetComponentInChildren<ThirdPersonCameraController>().enabled = false;

			this.GetComponentInChildren<BoxCollider>().enabled = true;
			CapsuleCollider [] capcol = this.GetComponentsInChildren<CapsuleCollider>();
			foreach(CapsuleCollider cap in capcol){
				cap.enabled = true;
			}


			MouseLook [] ml =  this.GetComponentsInChildren<MouseLook>();
			foreach(MouseLook m in ml){
				m.enabled = false;
			}

			Camera [] ca = this.GetComponentsInChildren<Camera>();
			foreach(Camera c in ca){
				c.enabled = false;
			}
			
			GUILayer [] gl = this.GetComponentsInChildren<GUILayer>();
			foreach(GUILayer g in gl){
				g.enabled = false;
			}
			
			AudioListener [] al = this.GetComponentsInChildren<AudioListener>();
			foreach(AudioListener a in al){
				a.enabled = false;
			}

			/*
			Animator [] at= this.GetComponentsInChildren<Animator>();
			foreach(Animator ani in at){
				ani.enabled = false;
			}


			Animation [] animation = this.GetComponentsInChildren<Animation>();
			foreach(Animation anim in animation){
				anim.enabled = false;
			}
			*/
			AudioSource [] asource = this.GetComponentsInChildren<AudioSource>();
			foreach(AudioSource Asource in asource){
				Asource.enabled = false;
			}
			this.GetComponentInChildren<PlayerScript>().enabled = false;
			this.GetComponentInChildren<LaserScript>().enabled = false;

			ikLimb [] iklimb = this.GetComponentsInChildren<ikLimb>();
			foreach(ikLimb ik in iklimb){
				if(ik != null)
				ik.enabled = false;
			}
			
		}
	}
	
	
}
