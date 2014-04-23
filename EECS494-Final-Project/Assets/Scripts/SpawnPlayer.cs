using UnityEngine;
using System.Collections;

public class SpawnPlayer : Photon.MonoBehaviour
{
	public void spawn()
	{
		GameObject newplayer =  PhotonNetwork.Instantiate("Player", this.transform.position, this.transform.rotation, 0);
		newplayer.name = PhotonNetwork.player.name;
	}

	void OnLevelWasLoaded(int level) {
		if (level == 0) {
			Debug.Log("sdad");
			PhotonNetwork.isMessageQueueRunning = true;
		}
		
		if (level == 1) {
			Debug.Log("sdad");
			PhotonNetwork.isMessageQueueRunning = true;
		}
		
		if (level == 2) {
			Debug.Log("sdad");
			PhotonNetwork.isMessageQueueRunning = true;
		}
		
		if (level == 3) {
			Debug.Log("sdad");
			PhotonNetwork.isMessageQueueRunning = true;
		}
	}

}
