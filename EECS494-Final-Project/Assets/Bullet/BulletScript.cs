﻿using UnityEngine;
using System.Collections;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

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
	private AudioSource main;

	private string bulletowner;


	void Awake()
	{
		bulletowner = PhotonNetwork.player.name;
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

	void Start()
	{
		main = gameObject.GetComponent( typeof(AudioSource) ) as AudioSource;
		main.clip = Resources.Load("Sounds/buzz") as AudioClip;
		main.loop = true;
		main.enabled = true;
		main.Play();


	}

	void FixedUpdate()
	{
		if(this.isOwner)
		{
			RaycastHit hit;
			if(Physics.Raycast(this.transform.position, this.rigidbody.velocity, out hit, hitLength))
			{
				if(hit.transform.CompareTag("Shield")||hit.transform.CompareTag("head")||
				   hit.transform.CompareTag("back")||hit.transform.CompareTag("Player")){
					PhotonView hitPlayerView = hit.transform.parent.GetComponent<PhotonView>();
					PlayerManager playermanager = hit.transform.parent.GetComponent<PlayerManager>();
					if(hitPlayerView.isMine)
					{
						return;
					}
					if(hit.transform.CompareTag("Shield")){

					}
					else if(hit.transform.CompareTag("head"))
					{
						Debug.Log("hit in head");
						hitPlayerView.RPC("modify_health", PhotonTargets.All, -bulletDamage);
						if(playermanager.cur_health<=0){
							hitPlayerView.RPC("is_dead",PhotonTargets.All);
							updatekillanddeath(hit);
						}
						else
							hitPlayerView.RPC("hit_head",PhotonTargets.All);
					}
					else if(hit.transform.CompareTag("back")){
						Debug.Log("hit in back");
						hitPlayerView.RPC("modify_health", PhotonTargets.All, -bulletDamage);
						if(playermanager.cur_health<=0){
							hitPlayerView.RPC("is_dead",PhotonTargets.All);
							updatekillanddeath(hit);
						}
						else
							hitPlayerView.RPC("hit_back",PhotonTargets.All);
					}
					else if(hit.transform.CompareTag("Player")){
						hitPlayerView.RPC("modify_health", PhotonTargets.All, -1f);
						if(playermanager.cur_health<=0){
							hitPlayerView.RPC("is_dead",PhotonTargets.All);
							updatekillanddeath(hit);
						}
						else
						hitPlayerView.RPC("hit_front",PhotonTargets.All);
					}
					PhotonNetwork.Destroy(this.gameObject);
				}
				/*
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
					Debug.Log("hit in head");
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
					Debug.Log("hit in back");
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

					Debug.Log("hit in front");
					PhotonPlayer [] playerlist = PhotonNetwork.playerList;
					foreach( PhotonPlayer player in playerlist){
						Debug.Log("player name in hieracy"+hit.transform.root.gameObject.GetComponent<PhotonView>().owner.name);
						Debug.Log("player name: "+ player.name);
						if(hit.transform.root.gameObject.GetComponent<PhotonView>().owner.name == player.name){
							Debug.Log("find players");
							PhotonHashtable someCustomPropertiesToSet = new PhotonHashtable() {{"kill", (int)PhotonNetwork.player.customProperties["kill"]-1}};
							player.SetCustomProperties(someCustomPropertiesToSet);
						}
					}

					PhotonNetwork.Destroy(this.gameObject);		
				}
				*/



				GameObject hitEffect = PhotonNetwork.Instantiate("BulletHitEffect", this.transform.position - hit.normal * .1f, Quaternion.LookRotation(hit.normal), 0);
				PhotonView hitView = hitEffect.GetComponent<PhotonView>();
				hitView.RPC("SetColor", PhotonTargets.All, this.color.r, this.color.g, this.color.b, this.color.a);
				hitView.RPC("SetSize", PhotonTargets.All, this.transform.localScale.x);
				AudioSource main2 = hitEffect.gameObject.GetComponent( typeof(AudioSource) ) as AudioSource;
				main2.clip = Resources.Load("Sounds/bounce") as AudioClip;
				main2.loop = false;
				main2.enabled = true;
				main2.Play();

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


	public void updatekillanddeath(RaycastHit hit)
	{
		PhotonPlayer [] playerlist = PhotonNetwork.playerList;
		foreach( PhotonPlayer player in playerlist){
			Debug.Log("player name in hieracy"+hit.transform.root.gameObject.GetComponent<PhotonView>().owner.name);
			Debug.Log("player name: "+ player.name);
			if(hit.transform.root.gameObject.GetComponent<PhotonView>().owner.name == player.name){
				Debug.Log("find players");
				PhotonHashtable someCustomPropertiesToSet = new PhotonHashtable() {{"death", (int)player.customProperties["death"]+1}};
				player.SetCustomProperties(someCustomPropertiesToSet);
				Debug.Log("kill"+(int)PhotonNetwork.player.customProperties["death"]);
			}
			if(bulletowner == player.name){
				PhotonHashtable someCustomPropertiesToSet = new PhotonHashtable() {{"kill", (int)player.customProperties["kill"]+1}};
				player.SetCustomProperties(someCustomPropertiesToSet);
			}
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
