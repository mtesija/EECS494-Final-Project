using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class LobbyMenuCameraScript : MonoBehaviour
{
	private string roomName;

	private bool collectHitData = false;
	private bool collectDeathData = true;
	private bool collectBounceData = false;
	private int playTo = 10;


	private Texture2D menuColorPreview;

	private Vector2 scrollPos = Vector2.zero;
	
	private bool connectFailed = false;

	private string serverVersion = "LAMBDA";
	
	public static readonly string SceneNameMenu = "_MainMenu";

	private string[] levelNames = new string[] {"Map 1", "Map 2", "TestScene"};
	private string[] colorNames = new string[] {"Blue", "Green", "Jade", "Orange", "Pink", "Red", "White", "Yellow"};
	private string [] modeNames = new string[] {"Deathmatch"};
	
	string map;

	private GUIContent[] colorBoxList;
	private GUIContent[] modeBoxList;
	private GUIContent[] levelBoxList;
	private ObjectComboBox<GameObject> colorBoxControl;
	private ObjectComboBox<GameObject> modeBoxControl;
	private ObjectComboBox<GameObject> levelBoxControl;
	private GUIStyle listStyle = new GUIStyle();

	public void Start()
	{

		listStyle.normal.textColor = Color.white; 
		Texture2D txt = new Texture2D(2,2);
		for(int i = 0; i < 4; i++) txt.SetPixel(i%2, i/2, Color.black);
		listStyle.onHover.background = txt;
		listStyle.hover.background = txt;
		listStyle.padding.left = 4;
		listStyle.padding.right = 4;
		listStyle.padding.top = 4;
		listStyle.padding.bottom = 4;

		GameObject[] goList = new GameObject[levelNames.Length];
		for(int i = 0; i < levelNames.Length; i++) goList[i] = new GameObject(levelNames[i]);
		levelBoxControl = new ObjectComboBox<GameObject>(new Rect(65, 50, 200, 20), goList, listStyle);

		goList = new GameObject[colorNames.Length];
		for(int i = 0; i < colorNames.Length; i++) goList[i] = new GameObject(colorNames[i]);
		colorBoxControl = new ObjectComboBox<GameObject>(new Rect(70, 450, 150, 20), goList, listStyle);

		goList = new GameObject[modeNames.Length];
		for(int i = 0; i < modeNames.Length; i++) goList[i] = new GameObject(modeNames[i]);
		modeBoxControl = new ObjectComboBox<GameObject>(new Rect(65, 315, 200, 20), goList, listStyle);
	}
	

	public void Awake()
	{
		// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
		PhotonNetwork.automaticallySyncScene = true;
		
		// generate a name for this player, if none is assigned yet
		if (String.IsNullOrEmpty(PhotonNetwork.playerName))
		{
			PhotonNetwork.playerName = "Guest" + Random.Range(1, 9999);
		}
		roomName = "Game" + Random.Range (1, 9999);
		menuColorPreview = new Texture2D(150, 90);

	}
	
	public void OnGUI()
	{
//		if (!PhotonNetwork.connected)
//		{
//			if (PhotonNetwork.connecting)
//			{
//				GUILayout.Label("Connecting to: " + PhotonNetwork.ServerAddress);
//			}
//			else
//			{
//				GUILayout.Label("Not connected. Check console output. Detailed connection state: " + PhotonNetwork.connectionStateDetailed + " Server: " + PhotonNetwork.ServerAddress);
//			}
//			
//			if (this.connectFailed)
//			{
//				GUILayout.Label("Connection failed. Check setup and use Setup Wizard to fix configuration.");
//				GUILayout.Label(String.Format("Server: {0}", new object[] {PhotonNetwork.ServerAddress}));
//				GUILayout.Label("AppId: " + PhotonNetwork.PhotonServerSettings.AppID);
//				
//				if (GUILayout.Button("Try Again", GUILayout.Width(100)))
//				{
//					this.connectFailed = false;
//					PhotonNetwork.ConnectUsingSettings(serverVersion);
//				}
//			}
//			
//			return;
//		}
		
		
		GUI.skin.box.fontStyle = FontStyle.Bold;
		GUI.Box(new Rect((Screen.width - 800) / 2, (Screen.height - 600) / 2, 800, 600), "Lobby: ");
		GUILayout.BeginArea(new Rect((Screen.width - 800) / 2, (Screen.height - 600) / 2, 500, 600));
		
		GUILayout.Space(50);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Player Name:", GUILayout.Width(100));
		GUILayout.Space(200);
		GUILayout.Label("Player Color:", GUILayout.Width(100));
		GUILayout.EndHorizontal();
		GUILayout.Space(20);


		// PLAYERS AND THEIR COLORS
		int numPlayers = 6;
		for(int i = 0; i < numPlayers; i++)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Space(15);

			GUILayout.Label("Player " + i.ToString(), GUILayout.Width(100));
			GUILayout.Space(200);
			GUILayout.Label("Color:", GUILayout.Width(100));

			GUILayout.Space(15);
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();



		// SECOND COLUMN WITH OPTIONS
		GUILayout.BeginArea(new Rect(((Screen.width - 800) / 2) + 500, (Screen.height - 600) / 2, 300, 600));
		GUILayout.Space(50);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Map:", GUILayout.Width(60));
		GUILayout.EndHorizontal();
		GUILayout.Space(5);

		// MAP IMAGE
		GUILayout.BeginHorizontal();
		Texture img = Resources.Load("PM_Assets/Jack-Map") as Texture;
		if( levelBoxControl.SelectedItemIndex == 0 ) img = Resources.Load("PM_Assets/Jack-Map") as Texture;
		else if ( levelBoxControl.SelectedItemIndex == 1 ) img = Resources.Load("PM_Assets/Andrew-Map") as Texture;
		else if ( levelBoxControl.SelectedItemIndex == 2 ) img = Resources.Load("PM_Assets/Test-Map") as Texture;
		GUILayout.Space(50);
		GUILayout.Label(img, GUILayout.Height(220), GUILayout.Width(220));
		levelBoxControl.Show();

		GUILayout.EndHorizontal();

		GUILayout.Space(10);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Mode:", GUILayout.Width(60));
		modeBoxControl.Show();
		GUILayout.Space(30);
		GUILayout.EndHorizontal();

		GUILayout.Space(25);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Play To:", GUILayout.Width(60));
		playTo = Convert.ToInt16(GUILayout.TextField(playTo.ToString(), GUILayout.Width(200)));
		GUILayout.Space(30);
		GUILayout.EndHorizontal();

		GUILayout.Space(20);
		GUILayout.BeginHorizontal();
		GUILayout.Space(10);
		collectDeathData = GUILayout.Toggle(collectDeathData, "Collect Death Data");
		//playerData.collectDeathData = collectDeathData;
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Space(10);
		collectHitData = GUILayout.Toggle(collectHitData, "Collect Hit Data");
		//playerData.collectHitData = collectHitData;
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Space(10);
		collectBounceData = GUILayout.Toggle(collectBounceData, "Collect Bounce Data");
		//playerData.collectBounceData = collectBounceData;
		GUILayout.EndHorizontal();

		GUILayout.Space(20);
		GUILayout.BeginHorizontal();
		GUILayout.Space(70);
		GUILayout.Button("Start Game", GUILayout.Width(160), GUILayout.Height(50));
		GUILayout.EndHorizontal();
		GUILayout.EndArea();













//		// Player name
//		GUILayout.BeginHorizontal();
//		GUILayout.Space(15);
//		GUILayout.Label("Player Name:", GUILayout.Width(80));
//		PhotonNetwork.playerName = GUILayout.TextField(PhotonNetwork.playerName);
//		if (GUI.changed)
//		{
//			// Save name
//			PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
//		}
//		GUILayout.Space(15);
//		GUILayout.EndHorizontal();
//		
//		GUILayout.BeginHorizontal();
//		GUILayout.Space(15);
//		GUILayout.BeginVertical();
//		GUILayout.Label("Player Color:", GUILayout.Width(100));
//		GUILayout.BeginHorizontal();
//		GUILayout.Label("R:", GUILayout.Width(15));
//		r = GUILayout.HorizontalSlider(r, .2f, 1, GUILayout.Width(180));
//		GUILayout.EndHorizontal();
//		GUILayout.BeginHorizontal();
//		GUILayout.Label("G:", GUILayout.Width(15));
//		g = GUILayout.HorizontalSlider(g, .2f, 1, GUILayout.Width(180));
//		GUILayout.EndHorizontal();
//		GUILayout.BeginHorizontal();
//		GUILayout.Label("B:", GUILayout.Width(15));
//		b = GUILayout.HorizontalSlider(b, .2f, 1, GUILayout.Width(180));
//		GUILayout.EndHorizontal();
//		GUILayout.EndVertical();
//		GUILayout.Space(10);
//		playerColor = new Color(r, g, b);
//		//playerData.playerColor = playerColor;
//		GUI.color = playerColor;
//		GUILayout.Label(menuColorPreview);
//		GUI.color = Color.white;
//		GUILayout.Space(15);
//		GUILayout.EndHorizontal();
//		
//		GUILayout.BeginHorizontal();
//		GUILayout.Space(10);
//		collectDeathData = GUILayout.Toggle(collectDeathData, "Collect Death Data");
//		//playerData.collectDeathData = collectDeathData;
//		collectHitData = GUILayout.Toggle(collectHitData, "Collect Hit Data");
//		//playerData.collectHitData = collectHitData;
//		collectBounceData = GUILayout.Toggle(collectBounceData, "Collect Bounce Data");
//		//playerData.collectBounceData = collectBounceData;
//		GUILayout.Space(10);
//		GUILayout.EndHorizontal();
//
//		GUILayout.Space(10);
//
//		// Join room by title
//		GUILayout.BeginHorizontal();
//		GUILayout.Space(15);
//		GUILayout.Label("Room Name:", GUILayout.Width(80));
//		this.roomName = GUILayout.TextField(this.roomName);
//		GUILayout.Space(15);
//		GUILayout.EndHorizontal();
//
//		
//		GUILayout.BeginHorizontal();
//		GUILayout.Space(15);
//		GUILayout.Label("Select Map:", GUILayout.Width(80));
//		levelSelect = GUILayout.SelectionGrid(levelSelect, levelNames, 3);
//		GUILayout.Space(10);
//		if (GUILayout.Button("Create Room", GUILayout.Width(100)))
//		{
//			PhotonNetwork.CreateRoom(this.roomName, new RoomOptions() { maxPlayers = 5 }, null);
//		}
//		GUILayout.Space(15);
//		GUILayout.EndHorizontal();
//
//		GUILayout.Space(10);
//
//		GUILayout.BeginHorizontal();
//		GUILayout.Space(15);
//		if(PhotonNetwork.countOfPlayers == 1)
//		{
//			GUILayout.Label("There is " + PhotonNetwork.countOfPlayers + " user online playing in " + PhotonNetwork.countOfRooms + " games.");
//		}
//		else
//		{
//			GUILayout.Label("There are " + PhotonNetwork.countOfPlayers + " users online playing in " + PhotonNetwork.countOfRooms + " games.");
//		}
//		GUILayout.Space(15);
//		GUILayout.EndHorizontal();
//
//		GUILayout.Space(10);
//
//		if (PhotonNetwork.GetRoomList().Length == 0)
//		{
//			GUILayout.BeginHorizontal();
//			GUILayout.Space(15);
//			GUILayout.Label("No games are currently available.");
//			GUILayout.EndHorizontal();
//		}
//		else
//		{
//			GUILayout.BeginHorizontal();
//			GUILayout.Space(15);
//			GUILayout.BeginVertical();
//			GUILayout.Label(PhotonNetwork.GetRoomList().Length + " games are currently available:");
//			
//			// Room listing: simply call GetRoomList: no need to fetch/poll whatever!
//			this.scrollPos = GUILayout.BeginScrollView(this.scrollPos);
//			foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList())
//			{
//				GUILayout.BeginHorizontal();
//				GUILayout.Label(roomInfo.name + " " + roomInfo.playerCount + "/" + roomInfo.maxPlayers);
//				if (GUILayout.Button("Join"))
//				{
//					PhotonNetwork.JoinRoom(roomInfo.name);
//				}
//				GUILayout.EndHorizontal();
//			}
//			
//			GUILayout.EndScrollView();
//
//			GUILayout.EndHorizontal();
//			GUILayout.Space(15);
//			GUILayout.EndVertical();
//		}
//
//		GUILayout.EndArea();
	}
	
	public void StartGame()
	{
		switch(levelBoxControl.SelectedItemIndex)
		{
		case 0:
			PhotonNetwork.LoadLevel( "Jack-Map" );
			break;
		case 1:
			PhotonNetwork.LoadLevel( "Andrew-Map" );
			break;
		case 2:
			PhotonNetwork.LoadLevel( "TestScene" );
			break;
		}
	}

	public void OnFailedToConnectToPhoton(object parameters)
	{
		this.connectFailed = true;
	}
}
