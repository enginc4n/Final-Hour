using System.Collections;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
using strange.extensions.mediation.impl;
using Unity.XR.OpenVR;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.GameHud
{
  public enum GameHudEvent
  {
    Settings
  }
  public class GameHudMediator : EventMediator
  {
    [Inject]
    public GameHudView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }
    
    [Inject]
    public IEnemyModel enemyModel { get; set; }
    
    [Inject]
    public ISpeedModel speedModel { get; set; }
    
    private Coroutine _shadowLoop;

    private Coroutine _timeLoop;

    private bool hasSlowedDown;

    public override void OnRegister()
    {
      view.dispatcher.AddListener(GameHudEvent.Settings, OnSettings);
      
      dispatcher.AddListener(PlayerEvent.EnemyStartedMoving, StartShadowLoop);
      dispatcher.AddListener(PlayerEvent.EnemyStoppedMoving, StopShadowLoop);
      dispatcher.AddListener(PlayerEvent.Died, OnDied);
      dispatcher.AddListener(PlayerEvent.UpdateRemainingTime, OnUpdateRemainingTime);
      dispatcher.AddListener(PlayerEvent.SlowDown, OnSlowDown);
      dispatcher.AddListener(PlayerEvent.SpeedUp, OnChangeSpeed);
      dispatcher.AddListener(PlayerEvent.ReturnNormalSpeed, OnChangeSpeed);
    }

    public override void OnInitialize()
    {
      view.SetState(true);
      view.SetShadowOpacity(0);
      view.SetIcon(speedModel.speedState);
      view.UpdateTimer(playerModel.remainingTime);
      view.UpdateScore(playerModel.score);
      CountTime();
      
      dispatcher.Dispatch(SoundEvent.StartGame);
    }
    
    private void CountTime()
    {
      _timeLoop = StartCoroutine(DecreaseRemainingTime());
    }

    private IEnumerator DecreaseRemainingTime()
    {
      while (playerModel.remainingTime > 0 && playerModel.isAlive)
      {
        yield return new WaitForSeconds(1f);

        playerModel.ChangeRemainingTime(-1f);
        
        playerModel.ChangeScore(1);
        view.UpdateScore(playerModel.score);
      }
      
      playerModel.Die();
    }
    
    private IEnumerator IncreaseRemainingTime()
    {
      while (playerModel.remainingTime > 0 && playerModel.isAlive)
      {
        yield return new WaitForSecondsRealtime(GameControlSettings.SlowGameSpeed);
        
        playerModel.ChangeRemainingTime(+1f);

        playerModel.ChangeScore(1);
        view.UpdateScore(playerModel.score);
      }
    }
    
    private void OnUpdateRemainingTime()
    {
      view.UpdateTimer(playerModel.remainingTime);
    }

    private void OnChangeSpeed()
    {
      if (hasSlowedDown)
      {
        StopCoroutine(_timeLoop);
        _timeLoop= StartCoroutine(DecreaseRemainingTime());
        hasSlowedDown = false;
      }
      
      view.SetIcon(speedModel.speedState);
    }
    
    private void OnSlowDown()
    {
      hasSlowedDown = true;
      
      view.SetIcon(speedModel.speedState);
      
      StopCoroutine(_timeLoop);
      _timeLoop= StartCoroutine(IncreaseRemainingTime());
    }

    private void StartShadowLoop()
    {
      _shadowLoop ??= StartCoroutine(ShadowLoop());
    }
    
    private void StopShadowLoop()
    {
      if (_shadowLoop == null) return;
      StopCoroutine(_shadowLoop);
      _shadowLoop = null;
    }


    private IEnumerator ShadowLoop()
    {
      while (true)
      {
        UpdateShadow();

        yield return null;
      }
    }

    private void UpdateShadow()
    {
      float spawnDistance = Mathf.Abs(playerModel.position - GameControlSettings.EnemySpawnPosition.x);
      float currentDistance = Mathf.Abs(playerModel.position - enemyModel.position);

      if (currentDistance >= spawnDistance)
      {
        view.SetShadowOpacity(0);
      }
      else
      {
        float a = 1 - ((currentDistance / spawnDistance));
        view.SetShadowOpacity(a);
      }
    }

    private void OnDied()
    {
      view.SetState(false);
    }
    
    private void OnSettings()
    {
      dispatcher.Dispatch(GameEvent.SettingsPanel, transform);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(GameHudEvent.Settings, OnSettings);
      
      dispatcher.RemoveListener(PlayerEvent.EnemyStartedMoving, StartShadowLoop);
      dispatcher.RemoveListener(PlayerEvent.EnemyStoppedMoving, StopShadowLoop);
      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);
      dispatcher.RemoveListener(PlayerEvent.UpdateRemainingTime, OnUpdateRemainingTime);
      dispatcher.RemoveListener(PlayerEvent.SlowDown, OnChangeSpeed);
      dispatcher.RemoveListener(PlayerEvent.SpeedUp, OnChangeSpeed);
      dispatcher.RemoveListener(PlayerEvent.ReturnNormalSpeed, OnChangeSpeed);
    }
  }
}
