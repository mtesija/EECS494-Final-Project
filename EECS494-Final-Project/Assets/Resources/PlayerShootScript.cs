using UnityEngine;
using System.Collections;

public class PlayerShootScript : Photon.MonoBehaviour
{
	public float ammo = 10;

	float shootDelayTimer = .15f;
	
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

		if(shootDelayTimer > 0)
		{
			shootDelayTimer -= Time.deltaTime;
		}
		else
		{
			if(Input.GetMouseButtonDown(0) && ammo > 0)
			{
				shootDelayTimer = .3f;
				ammo--;
				
				GameObject bullet = PhotonNetwork.Instantiate("Bullet", this.transform.position, this.transform.rotation, 0) as GameObject;
				PhotonView bulletView = bullet.GetComponent<PhotonView>();
				bulletView.RPC("SetColor", PhotonTargets.All, color.r, color.g, color.b, color.a);
			}
		}
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width / 2 - 10, Screen.height / 2 - 10, 20, 20));
		
		GUILayout.Label("+");
		
		GUILayout.EndArea();

		GUILayout.BeginArea(new Rect(Screen.width * 4 / 5, Screen.height * 19 / 20, Screen.width / 5 , Screen.height / 20));
		
		GUILayout.Label("Ammo: " + ammo);
		
		GUILayout.EndArea();
	}
}
