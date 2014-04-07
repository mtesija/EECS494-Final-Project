using UnityEngine;
using System.Collections;

public class PlayerShootScript : Photon.MonoBehaviour
{
	float shootDelayTimer = .5f;

	public GameObject player;

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
				Color color = new Color(0, 1, 0, 1);
				bulletView.RPC("SetColor", PhotonTargets.All, color.r, color.g, color.b, color.a);
			}
		}
	}
}
