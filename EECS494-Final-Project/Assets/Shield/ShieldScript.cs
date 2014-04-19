using UnityEngine;
using System.Collections;

public class ShieldScript : Photon.MonoBehaviour {
	
	public GameObject shield;

	public PlayerManager playerManager;
	
	private float energy = 50;
	private float maxEnergy = 50;
	
	public float refreshTimer = .5f;
	
	void Update()
	{
		if(refreshTimer <= 0)
		{
			if(shield.activeSelf == false)
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
		
		if(Input.GetMouseButtonDown(1) && shield.activeSelf == false && energy >= 5)
		{
			this.photonView.RPC("Activate", PhotonTargets.All);
			refreshTimer = 0;
		}
		
		if(Input.GetMouseButtonUp(1) && shield.activeSelf == true || energy <= 0)
		{
			this.photonView.RPC("Deactivate", PhotonTargets.All);
		}
	}

	[RPC]
	void SetShieldColor(float r, float g, float b)
	{
		shield.renderer.material.SetColor("Main Color", new Color(r, g, b, .75f));
	}
	
	[RPC]
	void Activate()
	{
		shield.SetActive(true);
	}
	
	[RPC]
	void Deactivate()
	{
		shield.SetActive(false);
	}
}

