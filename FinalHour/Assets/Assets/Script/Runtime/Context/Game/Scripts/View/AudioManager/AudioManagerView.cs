using System;
using System.Collections;
using System.Collections.Generic;
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
    public AudioSource timeSpeedSource;
    public AudioSource deathSoundSource;
    private List<Sound> _playingSounds = new List<Sound>();
    
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
      Sound sound = Array.Find(sfxSounds, sfx => sfx.name == name);

      if (sound == null)
      {
        Debug.LogError("Sound not found with the name: " + name);
        return;
      }
      
      sfxSource.clip = sound.clip;
      sfxSource.PlayOneShot(sound.clip);
    }
    
    public void PlayTimeSpeedSFX(string name)
    {
      Sound sound = Array.Find(sfxSounds, sfx => sfx.name == name);

      if (sound == null)
      {
        Debug.LogError("Sound not found with the name: " + name);
        return;
      }

      if (_playingSounds.Contains(sound))
      {
        return;
      }

      if (timeSpeedSource.isPlaying)
      {
        timeSpeedSource.Stop();
      }
      
      timeSpeedSource.clip = sound.clip;
      timeSpeedSource.PlayOneShot(sound.clip);
      _playingSounds.Add(sound);
      StartCoroutine(RemoveWhenFinished(sound));
    }

    public void PlayDeathSound(string name)
    {
      Sound sound = Array.Find(sfxSounds, sfx => sfx.name == name);

      if (sound == null)
      {
        Debug.LogError("Sound not found with the name: " + name);
        return;
      }
      
      if (_playingSounds.Contains(sound))
      {
        return;
      }
      
      deathSoundSource.clip = sound.clip;
      deathSoundSource.PlayOneShot(sound.clip);
      _playingSounds.Add(sound);
      StartCoroutine(RemoveWhenFinished(sound));

    }
    
    private IEnumerator RemoveWhenFinished(Sound sound)
    {
      yield return new WaitForSeconds(sound.clip.length);
      _playingSounds.Remove(sound);
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
