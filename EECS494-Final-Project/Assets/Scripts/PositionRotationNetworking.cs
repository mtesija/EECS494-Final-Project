using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class PositionRotationNetworking : Photon.MonoBehaviour 
{
	//variable for player
	private Vector3 serverPosition;
	private Vector3 clientPosition;
	private Quaternion serverRotation;
	private Quaternion clientRotation;
	private float lerpValue = 0;
	Animator anim;
	void Awake()
	{
		if(photonView.isMine)
		{
			this.enabled = false;
		}
		anim = this.GetComponentInChildren<Animator> ();
		
		serverPosition = this.transform.position;
		clientPosition = this.transform.position;
		serverRotation = this.transform.rotation;
		clientRotation = this.transform.rotation;
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting)
		{
			Vector3 position = this.transform.position;
			Quaternion rotation = this.transform.rotation;
			stream.Serialize(ref position);
			stream.Serialize(ref rotation);

			stream.SendNext(anim.GetBool("fwd"));
			stream.SendNext(anim.GetBool("bwd"));
			stream.SendNext(anim.GetBool("lft"));
			stream.SendNext(anim.GetBool("rgt"));
			stream.SendNext(anim.GetBool("jump"));
			stream.SendNext(anim.GetBool("fire"));
			stream.SendNext(anim.GetBool("roll"));
			stream.SendNext(anim.GetBool("crouch"));
			stream.SendNext(anim.GetBool("shield"));
			stream.SendNext(anim.GetBool("run"));
			stream.SendNext(anim.GetBool("hitfront"));
			stream.SendNext(anim.GetBool("hithead"));
			stream.SendNext(anim.GetBool("hitback"));
			stream.SendNext(anim.GetBool("die"));

		}
		else
		{
			Vector3 position = Vector3.zero;
			Quaternion rotation = Quaternion.identity;
			stream.Serialize(ref position);
			stream.Serialize(ref rotation);
			serverPosition = position;
			serverRotation = rotation;
			clientPosition = this.transform.position;
			clientRotation = this.transform.rotation;
			lerpValue = 0;
		
			anim.SetBool("fwd",(bool)stream.ReceiveNext());
			anim.SetBool("bwd",(bool)stream.ReceiveNext());
			anim.SetBool("lft",(bool)stream.ReceiveNext());
			anim.SetBool("rgt",(bool)stream.ReceiveNext());
			anim.SetBool("jump",(bool)stream.ReceiveNext());
			anim.SetBool("fire",(bool)stream.ReceiveNext());
			anim.SetBool("roll",(bool)stream.ReceiveNext());
			anim.SetBool("crouch",(bool)stream.ReceiveNext());
			anim.SetBool("shield",(bool)stream.ReceiveNext());
			anim.SetBool("run",(bool)stream.ReceiveNext());
			anim.SetBool("hitfront",(bool)stream.ReceiveNext());
			anim.SetBool("hithead",(bool)stream.ReceiveNext());
			anim.SetBool("hitback",(bool)stream.ReceiveNext());
			anim.SetBool("die",(bool)stream.ReceiveNext());;

		}
	}
	
	public void FixedUpdate()
	{
		lerpValue += Time.deltaTime * 9;

		this.transform.position = Vector3.Lerp(clientPosition, serverPosition, lerpValue);
		this.transform.rotation = Quaternion.Lerp(clientRotation, serverRotation, lerpValue);

	}
}
