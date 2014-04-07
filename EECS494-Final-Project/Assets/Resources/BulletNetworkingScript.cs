using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class BulletNetworkingScript : Photon.MonoBehaviour
{
	private Vector3 serverPosition;
	private Vector3 clientPosition;

	private Vector3 serverVelocity;

	private float lerpValue = 0;

	BulletScript bulletScript;

	void Awake()
	{
		bulletScript = this.GetComponent<BulletScript>();

		if(photonView.isMine)
		{
			this.enabled = false;
		}
		else
		{
			bulletScript.enabled = false;
		}

		this.serverPosition = this.transform.position;
		this.clientPosition = this.transform.position;
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting)
		{			
			stream.SendNext(this.rigidbody.velocity);
			stream.SendNext(this.transform.position);
		}
		else
		{
			this.serverVelocity = (Vector3) stream.ReceiveNext();
			this.serverPosition = (Vector3) stream.ReceiveNext();

			this.clientPosition = this.transform.position;

			this.lerpValue = 0;
		}
	}

	public void FixedUpdate()
	{
		this.lerpValue += Time.deltaTime * 9;

		this.rigidbody.velocity = this.serverVelocity;
		this.transform.position = Vector3.Lerp(this.clientPosition, this.serverPosition, this.lerpValue);
	}
}
