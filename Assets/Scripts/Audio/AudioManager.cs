﻿using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
	
	public AudioClip[] Tracks;
	
	// Son de Noel
	public AudioClip NoelTrack;
	private AudioClip lastPlayingTrack;
	
	private AudioSource audioSource;
	private bool isPlay;

	private float delayTime;
	
	
	// Use this for initialization
	void Start () 
	{
		audioSource = GetComponent<AudioSource> ();
		changeTrack();
		isPlay = true;
		audioSource.ignoreListenerPause = true;
		delayTime = 0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isPlay)
			return;
		
		// EASTER EGG NOEL
		if (Input.GetKeyUp(KeyCode.N))
		{
			if (System.DateTime.Now.Month == 12
			    && (System.DateTime.Now.Day >= 24 && System.DateTime.Now.Day <= 26))
			{
				if (audio.clip != NoelTrack)
				{
					stopTrack();
					audioSource.clip = NoelTrack;
					audioSource.Play();
				}
				else
				{
					audioSource.Stop();
					audioSource.clip = lastPlayingTrack;
					audioSource.Play();
				}
			}
		}
		
		if (delayTime >= audioSource.timeSamples)
			changeTrack();

		delayTime += Time.deltaTime;
	}
	
	public void changeTrack()
	{
		if (!isPlay)
			return;
		
		if(audioSource.isPlaying)
			audioSource.Stop();
		
		int randomTrack = Random.Range(0, Tracks.Length-1);
		lastPlayingTrack = Tracks[randomTrack];
		
		startTrack();
		delayTime = 0f;
	}
	
	public void stopTrack()
	{
		lastPlayingTrack = audioSource.clip;
		audioSource.Stop();
	}
	
	public void pauseTrack()
	{
		audioSource.Pause();
	}
	
	public void startTrack()
	{
		if (lastPlayingTrack != null)
			audioSource.clip = lastPlayingTrack;
		else
			changeTrack();
		audioSource.Play();
	}
	
	public void changePlayState()
	{
		isPlay = !isPlay;
	}
	
	public bool getPlayState()
	{
		return isPlay;
	}

	public void changeVolume (float vol)
	{
		audioSource.volume = vol;
	}

	public float getVolume ()
	{
		return audioSource.volume;
	}
}