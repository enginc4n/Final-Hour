﻿using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using strange.extensions.mediation.impl;
using UnityEngine.SceneManagement;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.AudioManager
{
  public class AudioManagerMediator : EventMediator
  {
    [Inject]
    public AudioManagerView view { get; set; }
    
    [Inject]
    public IAudioModel audioModel { get; set; }
    
    public override void OnRegister()
    {
      dispatcher.AddListener(PlayerEvent.Died, OnPlayerDied);
      dispatcher.AddListener(PlayerEvent.SlowDown, OnSlowDown);
      dispatcher.AddListener(PlayerEvent.SpeedUp, OnSpeedUp);
      dispatcher.AddListener(PlayerEvent.Collect, OnCollect);
      dispatcher.AddListener(PlayerEvent.CrashObstacle, OnCrashObstacle);
      dispatcher.AddListener(PlayerEvent.FireBullet, OnFireBullet);
      dispatcher.AddListener(PlayerEvent.Jump, OnJump);
      dispatcher.AddListener(PlayerEvent.EnemyCloser, OnEnemyStartedMoving);
      dispatcher.AddListener(PlayerEvent.DashStarted, OnDash);
      dispatcher.AddListener(PlayerEvent.CollectDash, OnDash);
      dispatcher.AddListener(PlayerEvent.FireSound, OnFireBullet);
      dispatcher.AddListener(PlayerEvent.BirdSound, OnBird);
      
      dispatcher.AddListener(GameEvent.GameStarted, OnStartGame);
      dispatcher.AddListener(GameEvent.ClockTick, OnClockTick);
      dispatcher.AddListener(GameEvent.Menu, OnMenu);
      dispatcher.AddListener(GameEvent.Pause, OnPause);
      dispatcher.AddListener(GameEvent.Continue, OnContinue);
      dispatcher.AddListener(GameEvent.Hover, OnHover);
      dispatcher.AddListener(GameEvent.Click, OnClick);
    }
    
    public override void OnInitialize()
    {
      audioModel.musicSource = view.musicSource;
      audioModel.sfxSource = view.sfxSource;
      audioModel.timeSpeedSource = view.timeSpeedSource;
      audioModel.deathSoundSource = view.deathSoundSource;
      audioModel.uiSource = view.uiSource;

      audioModel.ResetPitchVolume();
    }
    
    private void OnPlayerDied()
    {
      audioModel.SetPitchVolume(0f, 0f);
      view.musicSource.Stop();
      
      view.PlayDeathSound(SoundKeys.Death);
      view.PlayDeathSound(SoundKeys.Dead);
    }

    private void OnSlowDown()
    {
      view.PlayTimeSpeedSFX(SoundKeys.SlowDownSpeed);
    }

    private void OnSpeedUp()
    {
      view.PlayTimeSpeedSFX(SoundKeys.SpeedUpSpeed);
    }

    private void OnCollect()
    {
      view.PlaySFX(SoundKeys.Collect);
    }

    private void OnCrashObstacle()
    {
      view.PlaySFX(SoundKeys.DestroyObject);
      view.PlaySFX(SoundKeys.Grunt);
    }

    private void OnFireBullet()
    {
      view.PlaySFX(SoundKeys.Fire);
    }

    private void OnJump()
    {
      view.PlaySFX(SoundKeys.Jump);
    }

    private void OnEnemyStartedMoving()
    {
      view.PlayDeathSound(SoundKeys.EnemyCloser);
    }

    
    private void OnStartGame()
    {
      view.PlayMusic(SoundKeys.GameTheme);
    }
    
    
    private void OnDash()
    {
      view.PlaySFX(SoundKeys.Dash);
    }
    
    private void OnBird()
    {
      view.PlaySFX(SoundKeys.Raven);
    }
    
    private void OnClockTick()
    {
      view.PlaySFX(SoundKeys.ClockTick);
    }
    
    private void OnMenu()
    {
      view.PlayMusic(SoundKeys.MenuTheme);
    }

    private void OnPause()
    {
      if (SceneManager.GetActiveScene().buildIndex == 1)
      {
        audioModel.Pause();
      }
    }

    private void OnContinue()
    {
      if (SceneManager.GetActiveScene().buildIndex == 1)
      {
        audioModel.Resume();
      }
    }
    
    private void OnHover()
    { 
      view.PlayUi(SoundKeys.ButtonHover);
    }
    
    private void OnClick()
    {
      view.PlayUi(SoundKeys.ButtonClick);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(PlayerEvent.Died, OnPlayerDied);
      dispatcher.RemoveListener(PlayerEvent.SlowDown, OnSlowDown);
      dispatcher.RemoveListener(PlayerEvent.SpeedUp, OnSpeedUp);
      dispatcher.RemoveListener(PlayerEvent.Collect, OnCollect);
      dispatcher.RemoveListener(PlayerEvent.CrashObstacle, OnCrashObstacle);
      dispatcher.RemoveListener(PlayerEvent.FireBullet, OnFireBullet);
      dispatcher.RemoveListener(PlayerEvent.Jump, OnJump);
      dispatcher.RemoveListener(PlayerEvent.EnemyCloser, OnEnemyStartedMoving);
      dispatcher.RemoveListener(PlayerEvent.DashStarted, OnDash);
      dispatcher.RemoveListener(PlayerEvent.CollectDash, OnDash);
      dispatcher.RemoveListener(PlayerEvent.FireSound, OnFireBullet);
      dispatcher.RemoveListener(PlayerEvent.BirdSound, OnBird);
      
      dispatcher.RemoveListener(GameEvent.ClockTick, OnClockTick);
      dispatcher.RemoveListener(GameEvent.GameStarted, OnStartGame);
      dispatcher.RemoveListener(GameEvent.Menu, OnMenu);
      dispatcher.RemoveListener(GameEvent.Pause, OnPause);
      dispatcher.RemoveListener(GameEvent.Continue, OnContinue);
      dispatcher.RemoveListener(GameEvent.Hover, OnHover);
      dispatcher.RemoveListener(GameEvent.Click, OnClick);
    }
  }
}
