﻿using UnityEngine;
using System.Collections;

public class SpawnPlayer : Photon.MonoBehaviour
{

	public void spawn()
	{
		GameObject newplayer =  PhotonNetwork.Instantiate("Player", this.transform.position, this.transform.rotation, 0);
		newplayer.name = PhotonNetwork.player.name;
	}
}