﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.Model
{
  public class AudioModel : IAudioModel
  {
    public AudioSource musicSource { get; set; }
    public AudioSource sfxSource { get; set; }
    public AudioSource timeSpeedSource { get; set; }

    public void SetPitchVolumeRelative(float volume, float pitch)
    {
      musicSource.volume = GetScaledValue(volume, 0.2f, 1f);
      musicSource.pitch = GetScaledValue(pitch, 0.5f, 1f);
      
      sfxSource.volume = GetScaledValue(volume, 0, 1f);
      sfxSource.pitch = GetScaledValue(pitch, 0.95f, 1f);
      
      timeSpeedSource.volume-= GetScaledValue(volume, 0.5f, 1f);
      timeSpeedSource.pitch = GetScaledValue(pitch, 0.5f, 1f);
    }
    
    public void SetPitchVolume(float volume, float pitch)
    {
      musicSource.volume = volume;
      musicSource.pitch = pitch;
      
      sfxSource.volume = volume;
      sfxSource.pitch = pitch;
      
      timeSpeedSource.volume = volume;
      timeSpeedSource.pitch = pitch;
    }

    private float GetScaledValue(float value, float min, float max)
    {
      float range = max - min;

      return min + (range * value);
    }
  }
}