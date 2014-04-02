using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class BulletNetworkingScript : Photon.MonoBehaviour
{
	private Vector3 serverVelocity;
	private Color serverColor;

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
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting)
		{
			Vector3 velocity = this.rigidbody.velocity;
			Color color = bulletScript.color;
			float r = color.r;
			float g = color.g;
			float b = color.b;
			float a = color.a;

			stream.Serialize(ref velocity);
			stream.Serialize(ref r);
			stream.Serialize(ref g);
			stream.Serialize(ref b);
			stream.Serialize(ref a);
		}
		else
		{
			Vector3 velocity = this.rigidbody.velocity;
			float r = 0;
			float g = 0;
			float b = 0;
			float a = 0;

			stream.Serialize(ref velocity);
			stream.Serialize(ref r);
			stream.Serialize(ref g);
			stream.Serialize(ref b);
			stream.Serialize(ref a);

			serverVelocity = velocity;
			serverColor = new Color(r, g, b, a);
		}
	}

	public void FixedUpdate()
	{
		this.rigidbody.velocity = serverVelocity;
		bulletScript.SetColor(serverColor);
	}
}
