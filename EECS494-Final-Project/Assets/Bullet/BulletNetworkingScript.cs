using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class BulletNetworkingScript : Photon.MonoBehaviour
{
	private Vector3 serverPosition;
	private Vector3 clientPosition;

	private Vector3 serverVelocity;

	private float lerpValue;
	
	void Awake()
	{
		if(photonView.isMine)
		{
			this.enabled = false;
		}
		else
		{
			this.GetComponent<BulletScript>().enabled = false;
		}

		serverPosition = this.transform.position;
		clientPosition = this.transform.position;
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting)
		{
			Vector3 position = this.transform.position;
			Vector3 velocity = this.rigidbody.velocity;

			stream.Serialize(ref position);
			stream.Serialize(ref velocity);
		}
		else
		{
			Vector3 position = this.transform.position;
			Vector3 velocity = this.rigidbody.velocity;

			stream.Serialize(ref position);
			stream.Serialize(ref velocity);

			serverPosition = position;
			serverVelocity = velocity;

			clientPosition = this.transform.position;

			lerpValue = 0;
		}
	}

	public void FixedUpdate()
	{
		lerpValue += Time.deltaTime * 9;

		this.transform.localPosition = Vector3.Lerp(clientPosition, serverPosition, lerpValue);
		this.rigidbody.velocity = serverVelocity;
	}
}
