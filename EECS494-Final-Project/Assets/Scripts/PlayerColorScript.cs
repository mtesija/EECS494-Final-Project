using UnityEngine;
using System.Collections;

public class PlayerColorScript : Photon.MonoBehaviour {

	private Color color;
	private PhotonView pView;

	void Awake()
	{
		pView = this.GetComponent<PhotonView>();

		if(pView.isMine)
		{
			color = GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().playerColor;
			pView.RPC("SetColor", PhotonTargets.All, color.r, color.g, color.b);
		}
	}

	[RPC]
	void SetColor(float r, float g, float b)
	{
		color = new Color(r, g, b);
		this.renderer.material.color = color;
	}
}
