using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class BulletNetworkingScript : Photon.MonoBehaviour
{
	private Vector3 serverVelocity;
	private Color serverColor;

	BulletScript bulletScript;

	void Start()
	{
		bulletScript = this.GetComponent<BulletScript>();
	}

	void Awake()
	{
		if(photonView.isMine)
		{
			this.enabled = false;
		}
		else
		{
			bulletScript.enabled = false;
		}
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting)
		{
			Vector3 velocity = this.rigidbody.velocity;
			Color color = bulletScript.color;

			stream.Serialize(ref velocity);
			stream.Serialize(ref color.r);
			stream.Serialize(ref color.g);
			stream.Serialize(ref color.b);
			stream.Serialize(ref color.a);
		}
		else
		{
			Vector3 velocity = this.rigidbody.velocity;
			Color color = Color.white;

			stream.Serialize(ref velocity);
			stream.Serialize(ref color.r);
			stream.Serialize(ref color.g);
			stream.Serialize(ref color.b);
			stream.Serialize(ref color.a);

			serverVelocity = velocity;
			serverColor = color;
		}
	}

	public void FixedUpdate()
	{
		this.rigidbody.velocity = serverVelocity;
		bulletScript.SetColor(serverColor);
	}
}
