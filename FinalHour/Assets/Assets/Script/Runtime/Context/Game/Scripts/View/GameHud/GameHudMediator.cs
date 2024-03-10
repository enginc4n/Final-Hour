using System.Collections;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
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
    
    private Coroutine _shadowLoop;

    public override void OnRegister()
    {
      view.dispatcher.AddListener(GameHudEvent.Settings, OnSettings);
      
      dispatcher.AddListener(PlayerEvent.Play, OnInitialize);
      dispatcher.AddListener(PlayerEvent.SlowDown, CountTime);
      dispatcher.AddListener(PlayerEvent.SpeedUp, CountTime);
      dispatcher.AddListener(PlayerEvent.ReturnNormalSpeed, CountTime);
      dispatcher.AddListener(PlayerEvent.EnemyStartedMoving, StartShadowLoop);
      dispatcher.AddListener(PlayerEvent.EnemyStoppedMoving, StopShadowLoop);
      dispatcher.AddListener(PlayerEvent.Died, OnDied);
    }

    public override void OnInitialize()
    {
      view.SetState(true);
      view.SetShadowOpacity(0);
      view.UpdateTimer(playerModel.remainingTime);
      view.UpdateScore(playerModel.score);
      CountTime();
    }
    
    private void CountTime()
    {
      view.SetIcon(playerModel.speedState);
      CancelInvoke();
      InvokeRepeating("UpdateRemainingTime", playerModel.timerCountSpeed, playerModel.timerCountSpeed);
    }

    private void UpdateRemainingTime()
    {
      if (playerModel.remainingTime > 0)
      {
        playerModel.ChangeRemainingTime(-1f);
        view.UpdateTimer(playerModel.remainingTime);

        playerModel.ChangeScore(1);
        view.UpdateScore(playerModel.score);
      }
      else
      {
        playerModel.Die();
      }
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

      if (currentDistance > spawnDistance)
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
      dispatcher.Dispatch(GameEvent.OpenSettings, transform);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(GameHudEvent.Settings, OnSettings);
      
      dispatcher.RemoveListener(PlayerEvent.Play, OnInitialize);
      dispatcher.RemoveListener(PlayerEvent.SlowDown, CountTime);
      dispatcher.RemoveListener(PlayerEvent.SpeedUp, CountTime);
      dispatcher.RemoveListener(PlayerEvent.ReturnNormalSpeed, CountTime);
      dispatcher.RemoveListener(PlayerEvent.EnemyStartedMoving, StartShadowLoop);
      dispatcher.RemoveListener(PlayerEvent.EnemyStoppedMoving, StopShadowLoop);
      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);
    }
  }
}
