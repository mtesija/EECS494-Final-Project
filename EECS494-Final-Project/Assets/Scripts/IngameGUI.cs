using UnityEngine;
using System;
using System.Collections;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class IngameGUI : MonoBehaviour {

	public PlayerDataScript playerdatascript;
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


	public Rect KillInfoRect = new Rect(0, 0, 200, 100);


	public Rect GameoverInfoRect = new Rect(600, 0, 200, 100);
	public int GameoverWindowId = 103;

	public int KillWindowId = 102;

	private bool gameover = false;
	private bool reloadlevel = false;
	private float timedelay = 8;
	private string winner = "null";
	public int killstowin = 1;
	public void Start()
	{
		playerdatascript = GameObject.Find ("PlayerData").GetComponent<PlayerDataScript> ();
		if (playerdatascript == null) {
			Debug.Log("Not find in guiscript");
		}
		this.KillInfoRect.x = Screen.width - this.KillInfoRect.width;
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
		PhotonPlayer [] playerlist = PhotonNetwork.playerList;
		if (!gameover) {
						foreach (PhotonPlayer pp in playerlist) {
								if ((int)pp.customProperties ["kill"] >= killstowin) {
										this.GetComponent<CharacterController> ().enabled = false;
										this.GetComponent<PlayerScript> ().enabled = false;
										winner = pp.name;
										gameover = true;
								}
						}
				}
		if (gameover) {
			timedelay -=1*Time.deltaTime;
			if(timedelay<0){
				PhotonNetwork.LeaveRoom ();
				PhotonNetwork.Disconnect();
				if(reloadlevel == false){
					Application.LoadLevel ("_MainMenu");
					reloadlevel = true;
				}
			}
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
		this.statsRect = GUILayout.Window(this.WindowId, this.statsRect, this.ShootemStatsWindow, "Scoreboard");

		this.KillInfoRect = GUILayout.Window(this.KillWindowId, this.KillInfoRect, this.KillInfoWindow, "Kill Feed");

		if (gameover) {
			this.GameoverInfoRect = GUILayout.Window(this.GameoverWindowId, this.GameoverInfoRect, this.GameoverInfoWindow, "Game Over!");

		}
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
		GUILayout.Label ("Player:");
		GUILayout.Label ("Kills:");
		GUILayout.Label ("Deaths:");
		GUILayout.EndHorizontal ();
		PhotonPlayer [] playerlist = PhotonNetwork.playerList;

		int [] playerlistkey = new int[PhotonNetwork.playerList.Length];

		for (int x = 0; x < playerlistkey.Length; x++) {
				playerlistkey [x] = (int)playerlist [x].customProperties ["kill"];
		}
		Array.Sort(playerlistkey,playerlist);
		Array.Reverse(playerlist);
		foreach (PhotonPlayer player in playerlist) {
						//Debug.Log(player.name+((int)player.customProperties ["kill"]).ToString ());
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


	
	public void KillInfoWindow(int windowID){
		/*
		for (int i =0; i<playerdatascript.killed.Length; i++) {
			GUILayout.BeginHorizontal ();
			if(playerdatascript.killer[i] != null){	
				Debug.Log(i);
				GUILayout.Label (playerdatascript.killer[i].name);
				GUILayout.Label ("kill");
				GUILayout.Label (playerdatascript.killed[i].name);
			}
			GUILayout.EndHorizontal ();
		}
		GUI.DragWindow();
		*/
		
		if (PhotonNetwork.room != null) {
						string player1 = ((string)PhotonNetwork.room.customProperties ["killinfo1"]); 
						GUILayout.BeginHorizontal ();
						if(player1 != "NULL")
							GUILayout.Label (player1);
						//GUILayout.Label (playerdatascript.killed[i].name);
						GUILayout.EndHorizontal ();
						string player2 = ((string)PhotonNetwork.room.customProperties ["killinfo2"]); 
						GUILayout.BeginHorizontal ();
						if(player2 != "NULL")
							GUILayout.Label (player2);
						//GUILayout.Label (playerdatascript.killed[i].name);
						GUILayout.EndHorizontal ();
						string player3 = ((string)PhotonNetwork.room.customProperties ["killinfo3"]); 
						GUILayout.BeginHorizontal ();
						if(player1 != "NULL")
							GUILayout.Label (player3);
						//GUILayout.Label (playerdatascript.killed[i].name);
						GUILayout.EndHorizontal ();
		}


		/*
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
		*/

	}

	public void GameoverInfoWindow(int windowID){
		GUILayout.Label("The winner is "+winner+"!");
		GUILayout.Label ("The game will end in " + timedelay.ToString ());
	}


}
