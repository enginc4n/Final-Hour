using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.AudioManager
{
  public class AudioManagerView : EventView
  {
    [FormerlySerializedAs("jumpSound")]
    [Header("Sounds")]
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

    public void PlayAudioClip(AudioClip audioClip)
    {
      audioSource.PlayOneShot(audioClip);
    }

    public bool IsPlaying()
    {
      return audioSource.isPlaying;
    }
  }
}
