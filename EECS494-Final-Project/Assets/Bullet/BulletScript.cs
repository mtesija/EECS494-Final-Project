using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class BulletScript : Photon.MonoBehaviour
{
	private float minSpeed = 8;
	private float maxSpeed = 14;
	private float speed = 14;

	private float bulletDamage = 5f;

	private Color color = Color.white;

	private float hitLength = .4f;

	private bool isOwner = false;

	private Vector3 serverPosition;
	private Vector3 clientPosition;
	
	private Vector3 serverVelocity;
	
	private float lerpValue = 0;

	void Awake()
	{
		if(photonView.isMine)
		{
			this.isOwner = true;
			speed = maxSpeed;
			this.rigidbody.velocity = this.transform.forward * speed;
		}
		else
		{
			this.serverPosition = this.transform.position;
			this.clientPosition = this.transform.position;
		}
	}

	void FixedUpdate()
	{
		if(this.isOwner)
		{
			RaycastHit hit;
			if(Physics.Raycast(this.transform.position, this.rigidbody.velocity, out hit, hitLength))
			{
				if(hit.transform.CompareTag("Shield"))
				{
					PhotonView hitShieldView = hit.transform.GetComponent<PhotonView>();

					if(hitShieldView.isMine)
					{
						return;
					}

					hitShieldView.RPC("AddAmmo", PhotonTargets.All);
					PhotonNetwork.Destroy(this.gameObject);
				}

				else if(hit.transform.CompareTag("head"))
				{
					PhotonView hitPlayerView = hit.transform.parent.GetComponent<PhotonView>();
					//PhotonView hitPlayerView = hit.transform.GetComponent<PhotonView>();
					if(hitPlayerView.isMine)
					{
						return;
					}
					hitPlayerView.RPC("modify_health", PhotonTargets.All, -bulletDamage);
					hitPlayerView.RPC("hit_head",PhotonTargets.All);
					PhotonNetwork.Destroy(this.gameObject);
				}

				else if(hit.transform.CompareTag("back"))
				{
					PhotonView hitPlayerView = hit.transform.parent.GetComponent<PhotonView>();
					//PhotonView hitPlayerView = hit.transform.GetComponent<PhotonView>();
					if(hitPlayerView.isMine)
					{
						return;
					}
					hitPlayerView.RPC("modify_health", PhotonTargets.All, -bulletDamage);
					hitPlayerView.RPC("hit_back",PhotonTargets.All);
					PhotonNetwork.Destroy(this.gameObject);
				}


				else if(hit.transform.CompareTag("Player"))
				{
					PhotonView hitPlayerView = hit.transform.parent.GetComponent<PhotonView>();
					//PhotonView hitPlayerView = hit.transform.GetComponent<PhotonView>();
					if(hitPlayerView.isMine)
					{
						return;
					}
					hitPlayerView.RPC("modify_health", PhotonTargets.All, -bulletDamage);
					hitPlayerView.RPC("hit_front",PhotonTargets.All);
					PhotonNetwork.Destroy(this.gameObject);
				}


				GameObject hitEffect = PhotonNetwork.Instantiate("BulletHitEffect", this.transform.position - hit.normal * .1f, Quaternion.LookRotation(hit.normal), 0);
				PhotonView hitView = hitEffect.GetComponent<PhotonView>();
				hitView.RPC("SetColor", PhotonTargets.All, this.color.r, this.color.g, this.color.b, this.color.a);
				hitView.RPC("SetSize", PhotonTargets.All, this.transform.localScale.x);

				speed = Mathf.Clamp(speed * .8f, minSpeed, maxSpeed);
				bulletDamage = Mathf.Clamp(bulletDamage - 1, 1, 5);

				this.rigidbody.velocity = Vector3.Reflect(this.rigidbody.velocity, hit.normal).normalized * speed;
			}
		}
		else
		{
			this.lerpValue += Time.deltaTime * 9;
			
			this.rigidbody.velocity = this.serverVelocity;
			this.transform.position = Vector3.Lerp(this.clientPosition, this.serverPosition, this.lerpValue);
		}
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

	[RPC]
	private void SetColor(float r, float g, float b, float a)
	{
		this.color = new Color(r, g, b, a);
		this.GetComponent<TrailRenderer>().material.SetColor("_TintColor", this.color);
		this.GetComponent<MeshRenderer>().material.SetColor("_TintColor", this.color);
	}

	[RPC]
	private void SetSpeed(float inSpeed)
	{
		this.speed = inSpeed;
		this.rigidbody.velocity = this.rigidbody.velocity.normalized * speed;
	}

	[RPC]
	private void SetSize(float inSize)
	{
		this.GetComponent<TrailRenderer>().startWidth = inSize * 1.5f;
		Vector3 scale = new Vector3(inSize, inSize, inSize);
		this.transform.localScale = scale;
	}
}
