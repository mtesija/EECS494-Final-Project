using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class MainMenuCameraScript : MonoBehaviour
{
	private string roomName;

	private Color playerColor = Color.red;
	float r = 0;
	float b = 0;
	float g = 0;
	
	private bool collectHitData = false;
	private bool collectDeathData = true;
	private bool collectBounceData = false;
	
	private Texture2D menuColorPreview;

	private Vector2 scrollPos = Vector2.zero;
	
	private bool connectFailed = false;

	private string serverVersion = "ETA";
	
	public static readonly string SceneNameMenu = "_MainMenu";

	private int levelSelect;
	private string[] levelNames = new string[] {"Map 1", "Map 2", "Test"};

	public static readonly string SceneNameGame = "Jack-Map";
	public static readonly string SceneNameGame2 = "Andrew-Map";
	public static readonly string SceneNameGame3 = "TestScene";

	public PlayerDataScript playerData;
	
	public void Awake()
	{
		// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
		PhotonNetwork.automaticallySyncScene = true;
		
		// the following line checks if this client was just created (and not yet online). if so, we connect
		if (PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated)
		{
			// Connect to the photon master-server. We use the settings saved in PhotonServerSettings (a .asset file in this project)
			PhotonNetwork.ConnectUsingSettings(serverVersion);
		}
		
		// generate a name for this player, if none is assigned yet
		if (String.IsNullOrEmpty(PhotonNetwork.playerName))
		{
			PhotonNetwork.playerName = "Guest" + Random.Range(1, 9999);
		}
		roomName = "Game" + Random.Range (1, 9999);
		levelSelect = Mathf.Abs(Random.Range(-1, 1));
		r = Random.Range(0f, 1f);
		b = Random.Range(0f, 1f);
		g = Random.Range(0f, 1f);
		menuColorPreview = new Texture2D(150, 90);
	}
	
	public void OnGUI()
	{
		if (!PhotonNetwork.connected)
		{
			if (PhotonNetwork.connecting)
			{
				GUILayout.Label("Connecting to: " + PhotonNetwork.ServerAddress);
			}
			else
			{
				GUILayout.Label("Not connected. Check console output. Detailed connection state: " + PhotonNetwork.connectionStateDetailed + " Server: " + PhotonNetwork.ServerAddress);
			}
			
			if (this.connectFailed)
			{
				GUILayout.Label("Connection failed. Check setup and use Setup Wizard to fix configuration.");
				GUILayout.Label(String.Format("Server: {0}", new object[] {PhotonNetwork.ServerAddress}));
				GUILayout.Label("AppId: " + PhotonNetwork.PhotonServerSettings.AppID);
				
				if (GUILayout.Button("Try Again", GUILayout.Width(100)))
				{
					this.connectFailed = false;
					PhotonNetwork.ConnectUsingSettings(serverVersion);
				}
			}
			
			return;
		}
		
		
		GUI.skin.box.fontStyle = FontStyle.Bold;
		GUI.Box(new Rect((Screen.width - 400) / 2, (Screen.height - 400) / 2, 400, 400), "Create or Join a Room");
		GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 400) / 2, 400, 400));
		
		GUILayout.Space(25);
		
		// Player name
		GUILayout.BeginHorizontal();
		GUILayout.Space(15);
		GUILayout.Label("Player Name:", GUILayout.Width(80));
		PhotonNetwork.playerName = GUILayout.TextField(PhotonNetwork.playerName);
		if (GUI.changed)
		{
			// Save name
			PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
		}
		GUILayout.Space(15);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(15);
		GUILayout.BeginVertical();
		GUILayout.Label("Player Color:", GUILayout.Width(100));
		GUILayout.BeginHorizontal();
		GUILayout.Label("R:", GUILayout.Width(15));
		r = GUILayout.HorizontalSlider(r, 0, 1, GUILayout.Width(180));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("G:", GUILayout.Width(15));
		g = GUILayout.HorizontalSlider(g, 0, 1, GUILayout.Width(180));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("B:", GUILayout.Width(15));
		b = GUILayout.HorizontalSlider(b, 0, 1, GUILayout.Width(180));
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.Space(10);
		playerColor = new Color(r, g, b);
		playerData.playerColor = playerColor;
		GUI.color = playerColor;
		GUILayout.Label(menuColorPreview);
		GUI.color = Color.white;
		GUILayout.Space(15);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(10);
		collectDeathData = GUILayout.Toggle(collectDeathData, "Collect Death Data");
		playerData.collectDeathData = collectDeathData;
		collectHitData = GUILayout.Toggle(collectHitData, "Collect Hit Data");
		playerData.collectHitData = collectHitData;
		collectBounceData = GUILayout.Toggle(collectBounceData, "Collect Bounce Data");
		playerData.collectBounceData = collectBounceData;
		GUILayout.Space(10);
		GUILayout.EndHorizontal();

		GUILayout.Space(10);

		// Join room by title
		GUILayout.BeginHorizontal();
		GUILayout.Space(15);
		GUILayout.Label("Room Name:", GUILayout.Width(80));
		this.roomName = GUILayout.TextField(this.roomName);
		GUILayout.Space(15);
		GUILayout.EndHorizontal();

		
		GUILayout.BeginHorizontal();
		GUILayout.Space(15);
		GUILayout.Label("Select Map:", GUILayout.Width(80));
		levelSelect = GUILayout.SelectionGrid(levelSelect, levelNames, 3);
		GUILayout.Space(10);
		if (GUILayout.Button("Create Room", GUILayout.Width(100)))
		{
			PhotonNetwork.CreateRoom(this.roomName, new RoomOptions() { maxPlayers = 5 }, null);
		}
		GUILayout.Space(15);
		GUILayout.EndHorizontal();

		GUILayout.Space(10);

		GUILayout.BeginHorizontal();
		GUILayout.Space(15);
		if(PhotonNetwork.countOfPlayers == 1)
		{
			GUILayout.Label("There is " + PhotonNetwork.countOfPlayers + " user online playing in " + PhotonNetwork.countOfRooms + " games.");
		}
		else
		{
			GUILayout.Label("There are " + PhotonNetwork.countOfPlayers + " users online playing in " + PhotonNetwork.countOfRooms + " games.");
		}
		GUILayout.Space(15);
		GUILayout.EndHorizontal();

		GUILayout.Space(10);

		if (PhotonNetwork.GetRoomList().Length == 0)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Space(15);
			GUILayout.Label("No games are currently available.");
			GUILayout.EndHorizontal();
		}
		else
		{
			GUILayout.BeginHorizontal();
			GUILayout.Space(15);
			GUILayout.BeginVertical();
			GUILayout.Label(PhotonNetwork.GetRoomList().Length + " games are currently available:");
			
			// Room listing: simply call GetRoomList: no need to fetch/poll whatever!
			this.scrollPos = GUILayout.BeginScrollView(this.scrollPos);
			foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList())
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(roomInfo.name + " " + roomInfo.playerCount + "/" + roomInfo.maxPlayers);
				if (GUILayout.Button("Join"))
				{
					PhotonNetwork.JoinRoom(roomInfo.name);
				}
				GUILayout.EndHorizontal();
			}
			
			GUILayout.EndScrollView();

			GUILayout.EndHorizontal();
			GUILayout.Space(15);
			GUILayout.EndVertical();
		}

		GUILayout.EndArea();
	}
	
	public void OnCreatedRoom()
	{
		if(levelSelect == 0)
		{
			PhotonNetwork.LoadLevel(SceneNameGame);
		}
		else if(levelSelect == 1)
		{
			PhotonNetwork.LoadLevel(SceneNameGame2);
		}
		else if(levelSelect == 2)
		{
			PhotonNetwork.LoadLevel(SceneNameGame3);
		}
	}

	public void OnFailedToConnectToPhoton(object parameters)
	{
		this.connectFailed = true;
	}
}
