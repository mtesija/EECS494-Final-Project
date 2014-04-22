using UnityEngine;
using System.Collections;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerDataScript : MonoBehaviour {

	public Color playerColor = Color.white;

	public bool collectHitData = false;
	public bool collectDeathData = true;
	public bool collectBounceData = false;
	public PhotonPlayer [] killer;
	public PhotonPlayer[] killed;
	public int recordcount;
	private bool initialized;


	void Awake()
	{
		initialized = false;
		recordcount = 0;
		killer = new PhotonPlayer[3];
		killed = new PhotonPlayer[3];
		DontDestroyOnLoad(this.gameObject);
	}
	void Update(){

		if(PhotonNetwork.room == null){
			Debug.Log("nani");
		}
		else{
			if(initialized == false){
				string killpair = "NULL";
				PhotonHashtable killinfo = new PhotonHashtable (){{"killinfo1",killpair}};
				PhotonNetwork.room.SetCustomProperties (killinfo);
				PhotonHashtable killinfo2 = new PhotonHashtable (){{"killinfo2",killpair}};
				PhotonNetwork.room.SetCustomProperties (killinfo2);
				PhotonHashtable killinfo3 = new PhotonHashtable (){{"killinfo3",killpair}};
				PhotonNetwork.room.SetCustomProperties (killinfo3);
				int killannouncementcount = 1;
				PhotonHashtable killinfo4 = new PhotonHashtable (){{"count",killannouncementcount}};
				PhotonNetwork.room.SetCustomProperties (killinfo4);
				initialized = true;
			}
		}
		for (int i=0; i<3; i++) {
			if(killer[i]!= null)
			Debug.Log("killed "+killed[i].name);
			else{
				Debug.Log("null");
			}
		}
	}
}
