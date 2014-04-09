using UnityEngine;
using System.Collections;

public class HitEffectScript : Photon.MonoBehaviour
{
	private Color color = Color.white;

	void Update()
	{
		if(this.photonView.isMine && this.particleSystem.isStopped)
		{
			PhotonNetwork.Destroy(this.gameObject);
		}
	}

	[RPC]
	private void SetColor(float r, float g, float b, float a)
	{
		this.color = new Color(r, g, b, a);
		this.particleSystem.startColor = color;
	}
	
	[RPC]
	private void SetSize(float inSize)
	{
		this.particleSystem.startSize = inSize * 5f;
	}
}
