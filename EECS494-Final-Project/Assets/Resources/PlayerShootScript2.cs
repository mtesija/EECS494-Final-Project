using UnityEngine;
using System.Collections;

public class PlayerShootScript2 : Photon.MonoBehaviour
{
	float shootDelayTimer = .5f;

	private Color color = Color.white;

	public GameObject mainCamera;

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

		Ray ray = mainCamera.camera.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, 600f))
		{
			Vector3 relativePosition = hit.point - this.gameObject.transform.position;
			Quaternion rotation = Quaternion.LookRotation(relativePosition);

			this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * 5f);
		}

		if(shootDelayTimer > 0)
		{
			shootDelayTimer -= Time.deltaTime;
		}
		else
		{
			if(Input.GetMouseButtonDown(0))
			{
				shootDelayTimer = 1f;

				GameObject bullet = PhotonNetwork.Instantiate("Bullet", this.transform.position, this.transform.rotation, 0) as GameObject;
				PhotonView bulletView = bullet.GetComponent<PhotonView>();
				bulletView.RPC("SetColor", PhotonTargets.All, color.r, color.g, color.b, color.a);
			}
		}
	}
}
