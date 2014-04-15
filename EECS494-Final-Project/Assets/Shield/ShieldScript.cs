using UnityEngine;
using System.Collections;

public class ShieldScript : Photon.MonoBehaviour {
	
	public GameObject shield;
	
	public float energy = 100;
	public float maxEnergy = 100;
	
	public float refreshTimer = .5f;
	
	void Update()
	{
		if(refreshTimer <= 0)
		{
			if(shield.activeSelf == false)
			{
				energy = Mathf.Clamp(energy + 5, 0, maxEnergy);
			}
			else if(energy >= 0)
			{
				energy = Mathf.Clamp(energy - 10, 0, maxEnergy);
			}
			
			refreshTimer = .5f;
		}
		else
		{
			refreshTimer -= Time.deltaTime;
		}
		
		if(Input.GetMouseButtonDown(1) && shield.activeSelf == false && energy > 0)
		{
			this.photonView.RPC("Activate", PhotonTargets.All);
		}
		
		if(Input.GetMouseButtonUp(1) && shield.activeSelf == true || energy <= 0)
		{
			this.photonView.RPC("Deactivate", PhotonTargets.All);
		}
		
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

