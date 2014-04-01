using UnityEngine;
using System.Collections;


/* 	SOUND MANAGER
	========================================================================================================================
	DESCRIPTION:
	Over the past week or so, I have been working on a tool that helps users with use
	audio very easily in their games and also add functionality to the AudioSource
	object that will help to:
		1: 	Reduce lines of code in projects and make easier to read.
		2: 	Allow users to add complex effects that are not currently
			provided by Unity.
		3:	Easily play multiple sound effects over eachother at the same time.

	==========================================================================
	==========================================================================
	NOTE:
	- Volume is a float between 0.0f and 1.0f.

	==========================================================================
	==========================================================================
	DOCUMENTATION:
	- 1: A full documentation.
	- 2: A history of versions.


	==========================================================================
	==========================================================================
	- SECTION 1

	Start()							-		creates an AudioSource and enables it.
	
	FadeIn(src, t, vol) 			- 		loads 'src', and fades in to 'vol' in 't' seconds.
	
	FadeOut(t)						-		fades song out to stop in 't' seconds.
	
	FadeOutToSong(src, t, vol)		- 		fades out and in to the new clip in time 't'. 'vol' is optional and if left null will be the same as before.
	
	FadeOutToSong(src, t, t2, vol)	- 		fades out in 't' seconds and in to the new clip in time 't2'. 'vol' is optional and if left null will be the same as before.
	
	FadeTo(t, vol) 					- 		Fades the song to volume 'vol' to stop it in time t.
	
	Load(src)						- 		loads 'src', and plays it if begin is set.
	
	Pause()							- 		pauses the clip in its current spot.
	
	Pause(t)						- 		pauses for 't' seconds.
	
	Play() 							- 		begins the song if the clip has been set.
	
	Play(src) 						- 		loads the song from the 'src' and begins to play it.
	
	Play(vol)						-		plays song beginning in 't' seconds.
	
	Play(src, vol) 					- 		loads the song from the 'src' and begins to play it at volume 'vol'.
	
	Play(src, loop, poa)			-		creates an AudioSource, sets clip to 'src', sets loop and play on awake.
	
	
	Play(src, loop, poa, t, vol)	-		creates an AudioSource, sets clip to 'src', sets loop and play on awake, then fades in to 'vol' in 't' seconds.
	
	PlayDelayed(t)					- 		begins the song if the clip has been set at volume 'vol'.
	
	PlaySoundEffect(src, vol)		-		plays 'src' at volume 'vol'. Vol is optional and is 1.0 by default.
	
	SetLoop(loop)					-		sets loop to 'loop'.
	
	SetPlayOnAwake(poa)				-		sets play on awake to 'poa'.
	
	SetVolume(vol)					-		sets the volume to 'vol'.
	
	Stop() 							- 		stops the song.
	
	Swap()							-		stops current song, loads new song from 'src' and begins if 'begin' is set (set to false by default).


	==========================================================================
	==========================================================================
	- SECTION 2

 	VERSION 1.0	- added basics of song control.
 	Start()							-		creates an AudioSource and enables it.
 	Load(src)						- 		loads 'src', and plays it if begin is set.
	Play() 							- 		begins the song if the clip has been set
	Play(src) 						- 		loads the song from the 'src' and begins to play it.
	Play(vol)						-		plays song beginning in 't' seconds.
	PlayDelayed(vol) 				- 		begins the song if the clip has been set at volume 'vol'.
	Play(src, vol) 					- 		loads the song from the 'src' and begins to play it at volume 'vol'.
	Play(t)							-		plays song beginning in 't' seconds.
	Stop() 							- 		stops the song.
	Swap()							-		stops current song, loads new song from 'src' and begins if 'begin' is set (set to false by default).
	Pause()							-		pauses the clip in its current spot.

	==========================================================================
	VERSION 1.1 - added fading features.
	FadeIn(t, vol, src) 			- 		loads 'src', and fades in to 'vol' in 't' seconds. Src is optional if it has already been loaded.
	FadeOut(t)						-		fades song out to stop in 't' seconds
	FadeTo(t, vol) - Fades the song to volume 'vol' to stop it in time t.

	==========================================================================
	VERSION 1.2 - added a couple overloaded Play() and ability to set more options.
	SetLoop(loop)					- sets loop to 'loop'.
	SetPlayOnAwake(poa)				- sets play on awake to 'poa'.
	SetVolume(vol)					- sets the volume to 'vol'.
	Play(src, loop, poa)			- sets clip to 'src', sets loop and playOnAwake, then play
	Play(src, loop, poa, t, vol)	- sets clip to 'src', sets loop and playOnAwake, then fades in to 'vol' in 't' seconds.

	==========================================================================
	VERSION 1.3 -
	FadeOutToSong(src, t, vol)		- 		fades out and in to the new clip in time 't'. 'vol' is optional and if left null will be the same as before.
	FadeOutToSong(src, t, t2, vol)	- 		fades out in 't' seconds and in to the new clip in time 't2'. 'vol' is optional and if left null will be the same as before.
	Pause(t)						- 		pauses for 't' seconds.
	PlaySoundEffect(src, vol)		- 		plays the sound from 'src' at volume 'vol' on the first available soundeffect audio source. vol is optional and is 1.0f by default.


	========================================================================================================================
	========================================================================================================================
*/



public class SoundManager : MonoBehaviour {

	
	// ==========================================================================================
	// VERSION 1.0

	private AudioSource main;
	private AudioSource[] effectSources = new AudioSource[10];

	// Start()
	void Start ()
	{
		main = gameObject.AddComponent( typeof(AudioSource) ) as AudioSource;
		main.name = "main";
		main.enabled = true;

		for(int i = 0; i < effectSources.Length; i++)
		{
			effectSources[i] = gameObject.AddComponent( typeof(AudioSource) ) as AudioSource;
			effectSources[i].enabled = true;
			effectSources[i].loop = false;
		}
	}

	// Load(src)
	public void Load(string src)
	{ 
		main.enabled = false;
		main.clip = Resources.Load(src) as AudioClip;
		main.enabled = true;
	}

	// Play()
	public void Play() { main.Play(); }

	// Play(src)
	public void Play(string src)
	{
		this.Load(src);
		this.Play();
	}

	// Play(vol)
	public void Play(float vol)
	{
		main.volume = vol;
		this.Play();
	}

	// Play(src, vol)
	public void Play(string src, float vol)
	{
		this.Load(src);
		main.volume = vol;
		this.Play();
	}

	// PlayDelayed(t)
	public void PlayDelayed(float t) { main.PlayDelayed( t ); }

	// Stop()
	public void Stop() { main.Stop (); }

	// Swap(src, begin)
	public void Swap(string src, bool begin = false)
	{
		this.Stop();
		this.Load(src);
		if(begin) this.Play();
	}

	public void Pause(){ main.Pause(); }
	
	// ========================================================================================================================
	// ========================================================================================================================








	// ==========================================================================================
	// VERSION 1.1

	// FadeIn(t, vol, src) 
	public void FadeIn(float t, float vol, string src = null)
	{
		main.volume = 0;
		if(src != null) this.Play(src);
		else this.Play();
		StartCoroutine( this.Fader(t, vol) );
	}

	// FadeOut(t)
	public void FadeOut(float t){ StartCoroutine( this.Fader(t, 0) ); }

	// FadeTo(t, vol)
	public void FadeTo(float t, float vol) { StartCoroutine( this.Fader(t, vol) ); }

	// Helper coroutine that fades volume over a set time 't' to a set volume 'vol'.
	// comeFrom is used to finalize whatever function the call was made from.
	private IEnumerator Fader(float time, float vol)
	{
		float start = main.volume;
		float end = vol;

		float i = 0.0f;
		float rate = 1.0f/time;
		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			main.volume = Mathf.Lerp(start, end, i);
			yield return 0;
		}
		main.volume = vol;
		if(Mathf.Approximately(0.0f, vol)) this.Stop();
	}

	// ========================================================================================================================
	// ========================================================================================================================







	// ==========================================================================================
	// VERSION 1.2

	// SetLoop(loop)
	public void SetLoop(bool loop){ main.loop = loop; }

	// SetPlayOnAwake(poa)				
	public void SetPlayOnAwake(bool poa){ main.playOnAwake = poa; }

	// SetVolume(vol)
	public void SetVolume(float vol){ main.volume = vol; }

	// Play(src, loop, poa)
	public void Play( string src, bool loop, bool poa )
	{
		this.SetLoop(loop);
		this.SetPlayOnAwake(poa);
		this.Load(src);
		this.Play();
	}

	// Play(src, loop, poa, t, vol)
	public void Play( string src, bool loop, bool poa, float t, float vol )
	{
		this.SetLoop( loop );
		this.SetPlayOnAwake( poa );
		this.FadeIn(t, vol, src);
	}




	// ==========================================================================================
	// VERSION 1.3

	// Pause(t)
	public void Pause(float t) { StartCoroutine( this.PauseHelper(t) ); }

	// helper function for pause that helps it to wait for a certain period of time before starting again
	private IEnumerator PauseHelper(float t)
	{
		this.Pause();
		yield return new WaitForSeconds(t);
		this.Play();
	}

	// FadeToNewSong(src, t, vol)
	public void FadeToNewSong(string src, float fade, float vol = -1.0f){ StartCoroutine( this.FadeOutToSongHelper(src, vol, fade/2, fade/2) ); }

	// FadeToNewSong(src, t, t2, vol)
	public void FadeToNewSong(string src, float fadeOut, float fadeIn, float vol = -1.0f){ StartCoroutine( this.FadeOutToSongHelper(src, vol, fadeOut, fadeIn) ); }

	// helper coroutine to help the song fade out while another song begins to fade in.
	private IEnumerator FadeOutToSongHelper(string src, float vol, float fadeOut, float fadeIn)
	{
		float time1 = fadeOut, time2 = fadeIn;

		if(vol == -1.0f) vol = main.volume;
		this.FadeOut(time1);
		yield return new WaitForSeconds(time1);
		this.Swap(src);
		this.FadeIn(time2, vol);
		yield return new WaitForSeconds(time2);
	}

	// PlaySoundEffect(src, vol)
	public void PlaySoundEffect(string src, float vol = 1.0f)
	{
		for( int i = 0; i < effectSources.Length; i++ )
		{
			if( !effectSources[i].isPlaying )
			{
				effectSources[i].enabled = false;
				effectSources[i].clip = Resources.Load(src) as AudioClip;
				effectSources[i].enabled = true;
				effectSources[i].Play();
				return;
			}
		}
	}



	
	// ========================================================================================================================
	// ========================================================================================================================


}
