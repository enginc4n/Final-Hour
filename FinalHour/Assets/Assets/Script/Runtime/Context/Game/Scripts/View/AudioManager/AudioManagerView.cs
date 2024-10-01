using System;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.AudioManager
{
  public class AudioManagerView : EventView
  {
    public Sound[] musicSounds;
    public Sound[] sfxSounds;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public void PlayMusic(string name)
    {
      Sound sound = Array.Find(musicSounds, music => music.name == name);

      if (sound == null)
      {
        Debug.LogError("Sound not found at the name: " + name);
        return;
      }

      musicSource.clip = sound.clip;
      musicSource.loop = true;
      musicSource.Play();
    }

    public void PlaySFX(string name)
    {
      Sound sound = Array.Find(sfxSounds, music => music.name == name);

      if (sound == null)
      {
        Debug.LogError("Sound not found at the name: " + name);
        return;
      }

      sfxSource.PlayOneShot(sound.clip);
    }

    public void ToggleMusic()
    {
      musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
      sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
      musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
      sfxSource.volume = volume;
    }
  }
}
