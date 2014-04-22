using UnityEngine;
using System.Collections;

public class SpawnPlayer : Photon.MonoBehaviour
{

	public void spawn()
	{
		PhotonNetwork.Instantiate("Player", this.transform.position, this.transform.rotation, 0);
	}
}
