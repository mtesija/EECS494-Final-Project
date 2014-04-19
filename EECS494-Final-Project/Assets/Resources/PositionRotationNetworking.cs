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


	//variable for the gun 
	private Vector3 serverPosition_gun;
	private Vector3 clientPosition_gun;
	private Quaternion serverRotation_gun;
	private Quaternion clientRotation_gun;
	private float lerpValue_gun = 0;
	private Animator anim;
	private Transform gun;

	void Awake()
	{
		if(photonView.isMine)
		{
			this.enabled = false;
		}
		anim = this.GetComponentInChildren<Animator> ();
		gun = transform.Find ("/Player(Clone)/Robot Kyle/Root/ADSHolder/Scar-H");

		serverPosition_gun = gun.position;
		clientPosition_gun = gun.position;
		serverRotation_gun = gun.rotation;
		clientRotation_gun = gun.rotation;

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


			Vector3 position_gun = gun.position;
			Quaternion rotation_gun = gun.rotation;
			stream.Serialize(ref position_gun);
			stream.Serialize(ref rotation_gun);


			stream.SendNext(anim.GetBool("fwd"));
			stream.SendNext(anim.GetBool("bwd"));
			stream.SendNext(anim.GetBool("lft"));
			stream.SendNext(anim.GetBool("rgt"));
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


			Vector3 position_gun = Vector3.zero;
			Quaternion rotation_gun = Quaternion.identity;
			stream.Serialize(ref position_gun);
			stream.Serialize(ref rotation_gun);
			serverPosition_gun = position_gun;
			serverRotation_gun = rotation_gun;
			clientPosition_gun = gun.position;
			clientRotation_gun = gun.rotation;
			lerpValue_gun = 0;


			anim.SetBool("fwd",(bool)stream.ReceiveNext());
			anim.SetBool("bwd",(bool)stream.ReceiveNext());
			anim.SetBool("lft",(bool)stream.ReceiveNext());
			anim.SetBool("rgt",(bool)stream.ReceiveNext());


		}
	}
	
	public void FixedUpdate()
	{
		lerpValue += Time.deltaTime * 9;

		this.transform.position = Vector3.Lerp(clientPosition, serverPosition, lerpValue);
		this.transform.rotation = Quaternion.Lerp(clientRotation, serverRotation, lerpValue);


		lerpValue_gun += Time.deltaTime * 9;
		
		gun.position = Vector3.Lerp(clientPosition_gun, serverPosition_gun, lerpValue_gun);
		gun.rotation = Quaternion.Lerp(clientRotation_gun, serverRotation_gun, lerpValue_gun);


	}
}
