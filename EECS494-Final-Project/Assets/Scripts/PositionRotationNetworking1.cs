using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class PositionRotationNetworking1 : Photon.MonoBehaviour 
{
	//variable for the gun 
	private Vector3 serverPosition_gun;
	private Vector3 clientPosition_gun;
	private Quaternion serverRotation_gun;
	private Quaternion clientRotation_gun;
	private float lerpValue_gun = 0;
	private Transform gun;

	void Awake()
	{
		if(photonView.isMine)
		{
			this.enabled = false;
		}
		gun = transform.Find ("Scar-H");

		serverPosition_gun = gun.position;
		clientPosition_gun = gun.position;
		serverRotation_gun = gun.rotation;
		clientRotation_gun = gun.rotation;
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting)
		{
			Vector3 position_gun = gun.position;
			Quaternion rotation_gun = gun.rotation;
			stream.Serialize(ref position_gun);
			stream.Serialize(ref rotation_gun);
		}
		else
		{
			Vector3 position_gun = Vector3.zero;
			Quaternion rotation_gun = Quaternion.identity;
			stream.Serialize(ref position_gun);
			stream.Serialize(ref rotation_gun);
			serverPosition_gun = position_gun;
			serverRotation_gun = rotation_gun;
			clientPosition_gun = gun.position;
			clientRotation_gun = gun.rotation;
			lerpValue_gun = 0;

		}
	}
	
	public void FixedUpdate()
	{
		lerpValue_gun += Time.deltaTime * 9;
		
		gun.position = Vector3.Lerp(clientPosition_gun, serverPosition_gun, lerpValue_gun);
		gun.rotation = Quaternion.Lerp(clientRotation_gun, serverRotation_gun, lerpValue_gun);


	}
}
