using UnityEngine;
using System.Collections;

public class BGMusic : MonoBehaviour {

	public int currSong, nextSong;
	public int nSongs;
	public AudioClip song0, song1, song2, song3;
	public AudioClip[] songs;
	public AudioSource bgmusic;
	public bool BGMusicOn;
	public float pauseTime;

	void playSong(){
		if (!BGMusicOn) {
			return;
		}
		pauseTime = 2.0f;
		while (nextSong == currSong)
		  nextSong = Random.Range(0, nSongs);
		currSong = nextSong;
		bgmusic.audio.clip = songs[currSong];
		bgmusic.Play();
	}

	// Use this for initialization
	void Start () {
	  nSongs = 4;
	  currSong = -1;
	  nextSong = -1;
      songs = new AudioClip[nSongs];
	  songs[0] = song0;
	  songs[1] = song1;
	  songs[2] = song2;
	  songs[3] = song3;
	  pauseTime = 0.0f;
	}

	// Update is called once per frame
	void Update () {
	  if (BGMusicOn && !bgmusic.isPlaying) {
		  Invoke ("playSong", pauseTime);
	  }
	  if (!BGMusicOn) {
		if (bgmusic.isPlaying)
		  bgmusic.Stop();
		pauseTime = 0.0f;
	  }
	}
}
