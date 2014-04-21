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

	private PhotonPlayer[] playerList;
	private Texture2D menuColorPreview;

	private Vector2 scrollPos = Vector2.zero;
	
	private bool connectFailed = false;

	public PlayerDataScript playerData;

	
	public static readonly string SceneNameMenu = "_MainMenu";
	
	private string[] levelNames = new string[] {"Map 1", "Map 2", "TestScene"};
	private string[] colorNames = new string[] {"Blue", "Green", "Grey", "Magenta", "Red", "White", "Yellow"};
	private Color[] colors = new Color[] { Color.blue, Color.green, Color.grey, Color.magenta, Color.red, Color.white, Color.yellow};
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
		listStyle.onHover.background =
		listStyle.hover.background = new Texture2D(2,2);
		listStyle.padding.left =
		listStyle.padding.right =
		listStyle.padding.top =
		listStyle.padding.bottom = 4;

		GameObject[] goList = new GameObject[levelNames.Length];
		for(int i = 0; i < levelNames.Length; i++) goList[i] = new GameObject(levelNames[i]);
		levelBoxControl = new ObjectComboBox<GameObject>(new Rect(65, 70, 200, 20), goList, listStyle);

		goList = new GameObject[colorNames.Length];
		for(int i = 0; i < colorNames.Length; i++) goList[i] = new GameObject(colorNames[i]);
		colorBoxControl = new ObjectComboBox<GameObject>(new Rect(240, 100, 100, 20), goList, listStyle);

		goList = new GameObject[modeNames.Length];
		for(int i = 0; i < modeNames.Length; i++) goList[i] = new GameObject(modeNames[i]);
		modeBoxControl = new ObjectComboBox<GameObject>(new Rect(65, 335, 200, 20), goList, listStyle);
	}
	

	public void Awake()
	{
		// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
		PhotonNetwork.automaticallySyncScene = true;
		
		// generate a name for this player, if none is assigned yet
		roomName = "Game" + Random.Range (1, 9999);
		menuColorPreview = new Texture2D(150, 90);

	}

	public void Update()
	{

	}
	
	public void OnGUI()
	{
		GUIStyle st = new GUIStyle();
		st.fontStyle = FontStyle.Bold;
		st.alignment = TextAnchor.MiddleCenter;
		st.normal.textColor = Color.white;

		GUIStyle pst = new GUIStyle(st);
		pst.normal.textColor = colors[ colorBoxControl.SelectedItemIndex ];

		GUI.skin.box.fontStyle = FontStyle.Bold;
		GUI.skin.box.fontSize = 35;
		GUI.Box(new Rect((Screen.width - 800) / 2, (Screen.height - 600) / 2, 800, 600), "");
		GUI.Box(new Rect((Screen.width - 800) / 2, (Screen.height - 600) / 2, 800, 600), "");
		GUI.Box(new Rect((Screen.width - 800) / 2, (Screen.height - 600) / 2, 800, 600), "");
		GUI.Box(new Rect((Screen.width - 800) / 2, (Screen.height - 600) / 2, 800, 600), "Lobby: "+ PhotonNetwork.room.name);
		GUILayout.BeginArea(new Rect((Screen.width - 800) / 2, (Screen.height - 600) / 2, 500, 600));
		
		GUILayout.Space(70);
		GUILayout.BeginHorizontal();
		GUILayout.Space(15);
		GUILayout.Label("Player Name:", st, GUILayout.Width(100));
		GUILayout.Space(125);
		GUILayout.Label("Player Color:", st, GUILayout.Width(100));
		GUILayout.EndHorizontal();
		GUILayout.Space(15);

		GUILayout.BeginHorizontal();
		GUILayout.Space(15);
		GUILayout.Label(PhotonNetwork.player.name, pst, GUILayout.Width(100));
		GUILayout.Space(125);
		colorBoxControl.Show();
		GUILayout.Space(15);
		GUILayout.EndHorizontal();

		GUILayout.Space(20);
		GUILayout.BeginHorizontal();
		GUILayout.Space(15);
		GUILayout.Label("Other Players:", st, GUILayout.Width(100));
		GUILayout.EndHorizontal();
		
		// PLAYERS AND THEIR COLORS
		foreach(PhotonPlayer p in PhotonNetwork.playerList)
		{
			GUILayout.Space(10);

			if(p.name == PhotonNetwork.player.name) break;

			GUILayout.BeginHorizontal();
			GUILayout.Space(15);
			GUILayout.Label(p.name, st, GUILayout.Width(100));
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();



		// SECOND COLUMN WITH OPTIONS
		GUILayout.BeginArea(new Rect(((Screen.width - 800) / 2) + 500, (Screen.height - 600) / 2, 300, 600));
		GUILayout.Space(70);
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
		playTo = Convert.ToInt16(GUILayout.TextField(playTo.ToString(), GUILayout.Width(60)));
		GUILayout.Label(" Kills", GUILayout.Width(150));
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
		if(GUILayout.Button("Start Game", GUILayout.Width(160), GUILayout.Height(50)))
		{ print("START GAME"); StartGame(); }
		GUILayout.EndHorizontal();
		GUILayout.EndArea();

	}
	
	private void StartGame()
	{
		print("START GAME");
		playerData.playerColor = colors[ colorBoxControl.SelectedItemIndex ];
		//if(PhotonNetwork.playerList.Length <= 1) return;

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
