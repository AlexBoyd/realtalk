﻿using UnityEngine;
using System.Collections;

namespace Fungus
{
	/**
	 * Singleton music manager component.
	 * Provides basic music and sound effect functionality.
	 * Music playback persists across scene loads.
	 */
	[RequireComponent(typeof(AudioSource))]
	public class MusicController : MonoBehaviour 
	{
		static MusicController instance;

		/**
		 * Returns the MusicController singleton instance.
		 * Will create a MusicController game object if none currently exists.
		 */
		static public MusicController GetInstance()
		{
			if (instance == null)
			{
				GameObject go = new GameObject("MusicController");
				DontDestroyOnLoad(go);
				instance = go.AddComponent<MusicController>();
			}

			return instance;
		}

		protected virtual void Start()
		{
			audio.playOnAwake = false;
			audio.loop = true;
		}

		/**
		 * Plays game music using an audio clip.
		 * One music clip may be played at a time.
		 * @param musicClip The music clip to play
		 */
		public void PlayMusic(AudioClip musicClip)
		{
			audio.clip = musicClip;
			audio.Play();
		}

		/**
		 * Stops playing game music.
		 */
		public virtual void StopMusic()
		{
			audio.Stop();
		}

		/**
		 * Fades the game music volume to required level over a period of time.
		 * @param volume The new music volume value [0..1]
		 * @param duration The length of time in seconds needed to complete the volume change.
		 */
		public virtual void SetMusicVolume(float volume, float duration)
		{
			iTween.AudioTo(gameObject, volume, 1f, duration);
		}

		/**
		 * Plays a sound effect once, at the specified volume.
		 * Multiple sound effects can be played at the same time.
		 * @param soundClip The sound effect clip to play
		 * @param volume The volume level of the sound effect
		 */
		public virtual void PlaySound(AudioClip soundClip, float volume)
		{
            audio.Stop ();
			audio.clip = soundClip;
            audio.volume = volume;
            audio.loop = false;
            audio.Play ();
		}
	}
}