using UnityEngine;
using System.Collections;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;


public class RoomInit : MonoBehaviour {

	public PhotonPlayer [] killer;
	public PhotonPlayer[] killed;
	public int recordcount;
	private bool initialized;
	// Use this for initialization
	void Start () {
		initialized = false;
	}
	
	// Update is called once per frame
	void Update () {
		
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
	}
}
