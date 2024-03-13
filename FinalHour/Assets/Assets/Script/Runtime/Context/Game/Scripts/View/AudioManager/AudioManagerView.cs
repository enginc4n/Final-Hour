using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.AudioManager
{
  public class AudioManagerView : EventView
  {
    [Header("Sounds")]
    public AudioClip gameTheme;

    public AudioClip menuTheme;
    public AudioClip jumpClip;
    public AudioClip deathSoundClip;
    public AudioClip slowDownSpeedClip;
    public AudioClip speedUpTimeClip;
    public AudioClip collectClip;
    public AudioClip destroyClip;
    public AudioClip fireClip;
    public AudioClip enemyCloserClip;

    [Header("Refrences")]
    public AudioSource audioSource;

    public void PlayAudioClip(AudioClip audioClip, float volume = 1f)
    {
      audioSource.PlayOneShot(audioClip, volume);
    }

    public void LoopAudioClip(AudioClip audioClip)
    {
      audioSource.clip = audioClip;
      audioSource.loop = true;
      audioSource.Play();
    }

    public bool IsPlaying()
    {
      return audioSource.isPlaying;
    }
  }
}
