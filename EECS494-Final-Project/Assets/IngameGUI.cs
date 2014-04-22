using UnityEngine;
using System;
using System.Collections;
using ExitGames.Client.Photon;

public class IngameGUI : MonoBehaviour {

	/// <summary>Shows or hides GUI (does not affect if stats are collected).</summary>
	public bool statsWindowOn = true;
	
	/// <summary>Option to turn collecting stats on or off (used in Update()).</summary>
	public bool statsOn = true;
	
	/// <summary>Shows additional "health" values of connection.</summary>
	public bool healthStatsVisible;
	
	/// <summary>Shows additional "lower level" traffic stats.</summary>
	public bool trafficStatsOn;
	
	/// <summary>Show buttons to control stats and reset them.</summary>
	public bool buttonsOn;
	
	/// <summary>Positioning rect for window.</summary>
	public Rect statsRect = new Rect(0, 100, 200, 200);
	
	/// <summary>Unity GUI Window ID (must be unique or will cause issues).</summary>
	public int WindowId = 100;
	
	
	public void Start()
	{
		this.statsRect.x = Screen.width - this.statsRect.width;
	}
	
	/// <summary>Checks for shift+tab input combination (to toggle statsOn).</summary>
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			this.statsWindowOn = !this.statsWindowOn;
			this.statsOn = true;    // enable stats when showing the window
		}
	}
	
	public void OnGUI()
	{
		if (PhotonNetwork.networkingPeer.TrafficStatsEnabled != statsOn)
		{
			PhotonNetwork.networkingPeer.TrafficStatsEnabled = this.statsOn;
		}
		
		if (!this.statsWindowOn)
		{
			return;
		}
		this.statsRect = GUILayout.Window(this.WindowId, this.statsRect, this.ShootemStatsWindow, "Scoreboard(Tab)");
	}
	
	public void ShootemStatsWindow(int windowID)
	{
		bool statsToLog = false;
		TrafficStatsGameLevel gls = PhotonNetwork.networkingPeer.TrafficStatsGameLevel;
		long elapsedMs = PhotonNetwork.networkingPeer.TrafficStatsElapsedMs / 1000;
		if (elapsedMs == 0)
		{
			elapsedMs = 1;
		}
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("PlayerName");
		GUILayout.Label ("Kill");
		GUILayout.Label ("Death");
		GUILayout.EndHorizontal ();
		PhotonPlayer [] playerlist = PhotonNetwork.playerList;

		int [] playerlistkey = new int[PhotonNetwork.playerList.Length];

		for (int x = 0; x < playerlistkey.Length; x++) {
				playerlistkey [x] = (int)playerlist [x].customProperties ["kill"];
		}
		Array.Sort(playerlistkey,playerlist);
		Array.Reverse(playerlist);
		foreach (PhotonPlayer player in playerlist) {
						Debug.Log(player.name+((int)player.customProperties ["kill"]).ToString ());
						string kill = ((int)player.customProperties ["kill"]).ToString ();
						string death = ((int)player.customProperties ["death"]).ToString ();
						string name = player.name;
						GUILayout.BeginHorizontal ();
						GUILayout.Label (name);
						GUILayout.Label (kill);
						GUILayout.Label (death);
						GUILayout.EndHorizontal ();
				}
		if (GUI.changed)
		{
			//Debug.Log("gui changed");
			this.statsRect.height = 100;
		}
		GUI.DragWindow();
	}
}
