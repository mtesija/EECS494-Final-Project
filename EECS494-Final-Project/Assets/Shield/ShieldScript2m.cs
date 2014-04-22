using UnityEngine;
using System.Collections;

public class ShieldScript2m : Photon.MonoBehaviour {

	public PlayerShootScript gun;
	public PlayerShootScript2 gun2;
	public PlayerScript gun3;

	public PlayerManager playerManager;
	
	private float energy = 50;
	private float maxEnergy = 50;
	
	public float refreshTimer = .5f;
	
	void Update()
	{
		if(refreshTimer <= 0)
		{
			if(this.renderer.enabled == false)
			{
				energy = Mathf.Clamp(energy + 2, 0, maxEnergy);
				playerManager.modify_secondary(2);
			}
			else if(energy >= 0)
			{
				energy = Mathf.Clamp(energy - 5, 0, maxEnergy);
				playerManager.modify_secondary(-5);
			}
			
			refreshTimer = .5f;
		}
		else
		{
			refreshTimer -= Time.deltaTime;
		}
		
		if(Input.GetMouseButtonDown(1) && this.renderer.enabled == false && energy >= 5)
		{
			this.photonView.RPC("Activate", PhotonTargets.All);
			refreshTimer = 0;
		}
		
		if(Input.GetMouseButtonUp(1) && this.renderer.enabled == true || energy <= 0)
		{
			this.photonView.RPC("Deactivate", PhotonTargets.All);
		}
	}

	[RPC]
	void SetShieldColor(float r, float g, float b)
	{
		this.renderer.material.SetColor("Main Color", new Color(r, g, b, .75f));
	}
	
	[RPC]
	void Activate()
	{
		this.renderer.enabled = true;
		this.collider.enabled = true;
	}
	
	[RPC]
	void Deactivate()
	{
		this.renderer.enabled = false;
		this.collider.enabled = false;
	}

	[RPC]
	void AddAmmo()
	{
		if(gun != null)
		{
			gun.ammo += 3;
		}
		else if(gun2 != null)
		{
			gun2.ammo += 3;
		}
		else if(gun3 != null)
		{
			gun3.bulletCounter += 3;
		}
	}
}

