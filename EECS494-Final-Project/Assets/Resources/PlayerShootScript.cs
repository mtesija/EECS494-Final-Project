using UnityEngine;
using System.Collections;

public class PlayerShootScript : Photon.MonoBehaviour
{
	float shootDelayTimer = .5f;

	private Color color = Color.white;

	void Start()
	{
		if(this.GetComponent<PhotonView>().isMine)
		{
			color = GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().playerColor;
		}
	}

	void Update()
	{
		Screen.lockCursor = true;

		if(shootDelayTimer > 0)
		{
			shootDelayTimer -= Time.deltaTime;
		}
		else
		{
			if(Input.GetMouseButtonDown(0))
			{
				shootDelayTimer = .5f;

				GameObject bullet = PhotonNetwork.Instantiate("Bullet", this.transform.position, this.transform.rotation, 0) as GameObject;
				PhotonView bulletView = bullet.GetComponent<PhotonView>();
				bulletView.RPC("SetColor", PhotonTargets.All, color.r, color.g, color.b, color.a);
			}
		}
	}
}
