using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class PositionRotationNetworking : Photon.MonoBehaviour 
{
	private Vector3 serverPosition;
	private Vector3 clientPosition;
	
	private Quaternion serverRotation;
	private Quaternion clientRotation;
	
	private float lerpValue = 0;
	
	void Awake()
	{
		if(photonView.isMine)
		{
			this.enabled = false;
		}
		
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
		}
	}
	
	public void FixedUpdate()
	{
		lerpValue += Time.deltaTime * 9;

		this.transform.position = Vector3.Lerp(clientPosition, serverPosition, lerpValue);
		this.transform.rotation = Quaternion.Lerp(clientRotation, serverRotation, lerpValue);
	}
}
