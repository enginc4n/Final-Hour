using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.Model
{
  public interface IAudioModel
  {
    public AudioSource musicSource { get; set; }
    public AudioSource sfxSource { get; set; }
    public AudioSource timeSpeedSource { get; set; }

    public void SetPitchVolumeRelative(float volume, float pitch);
    
    public void SetPitchVolume(float volume, float pitch);
  }
}