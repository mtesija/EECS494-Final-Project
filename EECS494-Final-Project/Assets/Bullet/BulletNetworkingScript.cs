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

		serverPosition = this.transform.position;
		clientPosition = this.transform.position;
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting)
		{
			Vector3 velocity = this.rigidbody.velocity;
			Vector3 position = this.transform.position;

			stream.Serialize(ref velocity);
			stream.Serialize(ref position);
		}
		else
		{
			Vector3 velocity = this.rigidbody.velocity;
			Vector3 position = Vector3.zero;

			stream.Serialize(ref velocity);
			stream.Serialize(ref position);

			serverVelocity = velocity;
			serverPosition = position;

			clientPosition = this.transform.position;

			lerpValue = 0;
		}
	}

	public void FixedUpdate()
	{
		lerpValue += Time.deltaTime * 9;

		this.rigidbody.velocity = serverVelocity;
		this.transform.position = Vector3.Lerp (clientPosition, serverPosition, lerpValue);
	}
}
