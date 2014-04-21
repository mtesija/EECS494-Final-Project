using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class Animation_network : Photon.MonoBehaviour {
	
	public Animator anim;
	void Start()
	{
		anim = gameObject.transform.root.GetComponentInChildren<Animator> ();
	}

	[RPC]
	void hit_front(){
		if(!photonView.isMine){
			anim.SetTrigger("hitfront");
		}
		anim.SetTrigger("hitfront");
	}


	[RPC]
	void hit_back(){
		if(!photonView.isMine){
			anim.SetTrigger("hitback");
		}
		anim.SetTrigger("hitback");
	}


	[RPC]
	void hit_head(){
		if(!photonView.isMine){
			anim.SetTrigger("hithead");
		}
		anim.SetTrigger("hithead");
	}

}
