using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.AudioManager
{
  public class AudioManagerMediator : EventMediator
  {
    [Inject]
    public AudioManagerView view { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(SoundEvents.Jump, OnJump);
      dispatcher.AddListener(SoundEvents.Fire, OnFire);
      dispatcher.AddListener(SoundEvents.DeathSound, OnDeath);
      dispatcher.AddListener(SoundEvents.Collect, OnCollect);
      dispatcher.AddListener(SoundEvents.Destroy, OnDestroyObject);
      dispatcher.AddListener(SoundEvents.SlowDownSpeed, OnSlowDownTime);
      dispatcher.AddListener(SoundEvents.SpeedUpTime, OnSpeedUpTime);
      dispatcher.AddListener(SoundEvents.EnemyCloser, OnEnemyCloser);
    }

    private void OnEnemyCloser()
    {
      view.PlayAudioClip(view.enemyCloserClip);
    }

    private void OnSpeedUpTime()
    {
      view.PlayAudioClip(view.speedUpTimeClip);
    }

    private void OnSlowDownTime()
    {
      view.PlayAudioClip(view.slowDownSpeedClip);
    }

    private void OnDestroyObject()
    {
      view.PlayAudioClip(view.destroyClip);
    }

    private void OnCollect()
    {
      view.PlayAudioClip(view.collectClip);
    }

    private void OnDeath()
    {
      view.PlayAudioClip(view.deathSoundClip);
    }

    private void OnFire()
    {
      view.PlayAudioClip(view.fireClip);
    }

    private void OnJump()
    {
      view.PlayAudioClip(view.jumpClip);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(SoundEvents.Jump, OnJump);
      dispatcher.RemoveListener(SoundEvents.Fire, OnFire);
      dispatcher.RemoveListener(SoundEvents.DeathSound, OnDeath);
      dispatcher.RemoveListener(SoundEvents.Collect, OnCollect);
      dispatcher.RemoveListener(SoundEvents.Destroy, OnDestroyObject);
      dispatcher.RemoveListener(SoundEvents.SlowDownSpeed, OnSlowDownTime);
      dispatcher.RemoveListener(SoundEvents.SpeedUpTime, OnSpeedUpTime);
      dispatcher.AddListener(SoundEvents.EnemyCloser, OnEnemyCloser);
    }
  }
}
