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
      dispatcher.AddListener(SoundEvent.StartGame, OnStartGame);
      dispatcher.AddListener(SoundEvent.Menu, OnMenu);
      dispatcher.AddListener(SoundEvent.Jump, OnJump);
      dispatcher.AddListener(SoundEvent.Fire, OnFire);
      dispatcher.AddListener(SoundEvent.DeathSound, OnDeath);
      dispatcher.AddListener(SoundEvent.Collect, OnCollect);
      dispatcher.AddListener(SoundEvent.Destroy, OnDestroyObject);
      dispatcher.AddListener(SoundEvent.SlowDownSpeed, OnSlowDownTime);
      dispatcher.AddListener(SoundEvent.SpeedUpTime, OnSpeedUpTime);
      dispatcher.AddListener(SoundEvent.EnemyCloser, OnEnemyCloser);
    }
    
    private void OnStartGame()
    {
      view.LoopAudioClip(view.gameTheme);
    }
    
    private void OnMenu()
    {
      view.LoopAudioClip(view.menuTheme);
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
      dispatcher.RemoveListener(SoundEvent.StartGame, OnStartGame);
      dispatcher.RemoveListener(SoundEvent.Menu, OnMenu);
      dispatcher.RemoveListener(SoundEvent.Jump, OnJump);
      dispatcher.RemoveListener(SoundEvent.Fire, OnFire);
      dispatcher.RemoveListener(SoundEvent.DeathSound, OnDeath);
      dispatcher.RemoveListener(SoundEvent.Collect, OnCollect);
      dispatcher.RemoveListener(SoundEvent.Destroy, OnDestroyObject);
      dispatcher.RemoveListener(SoundEvent.SlowDownSpeed, OnSlowDownTime);
      dispatcher.RemoveListener(SoundEvent.SpeedUpTime, OnSpeedUpTime);
      dispatcher.AddListener(SoundEvent.EnemyCloser, OnEnemyCloser);
    }
  }
}
