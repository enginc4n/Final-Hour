using System.Collections;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using strange.extensions.mediation.impl;
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

    private Coroutine _warningRoutine;

    private bool _hasSlowedDown;

    public override void OnRegister()
    {
      view.dispatcher.AddListener(GameHudEvent.Settings, OnSettings);

      dispatcher.AddListener(PlayerEvent.EnemyStartedMoving, StartShadowLoop);
      dispatcher.AddListener(PlayerEvent.EnemyStoppedMoving, StopShadowLoop);
      dispatcher.AddListener(PlayerEvent.Died, OnDied);
      dispatcher.AddListener(PlayerEvent.SlowDown, OnSlowDown);
      dispatcher.AddListener(PlayerEvent.SpeedUp, OnChangeSpeed);
      dispatcher.AddListener(PlayerEvent.ReturnNormalSpeed, OnChangeSpeed);
      dispatcher.AddListener(PlayerEvent.DashFinished, OnDashFinished);
      dispatcher.AddListener(PlayerEvent.FireBullet, OnFire);
      dispatcher.AddListener(PlayerEvent.FlyingObstacleIncoming, OnFlyingObstacleIncoming);
    }

    public override void OnInitialize()
    {
      view.SetState(true);
      view.SetShadowOpacity(0);
      view.SetIcon(speedModel.speedState);
      view.UpdateScore(playerModel.score);
      CountTime();
    }
    
    private void FixedUpdate()
    {
      int minutes = Mathf.FloorToInt(playerModel.remainingTime / 60f);
      int seconds = Mathf.FloorToInt(playerModel.remainingTime % 60f);
        
      view.timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void CountTime()
    {
      _timeLoop = StartCoroutine(DecreaseRemainingTime());
      StartCoroutine(ScoreRoutine());
    }

    private IEnumerator DecreaseRemainingTime()
    {
      while (playerModel.remainingTime > 0 && playerModel.isAlive)
      {
        yield return new WaitForSeconds(1f);

        playerModel.ChangeRemainingTime(-1f);
      }

      playerModel.Die();
    }

    private IEnumerator IncreaseRemainingTime()
    {
      while (playerModel.remainingTime > 0 && playerModel.isAlive)
      {
        yield return new WaitForSecondsRealtime(1 / GameMechanicSettings.SlowTimeGain);

        playerModel.ChangeRemainingTime(+1f);
      }
    }
    
    private IEnumerator ScoreRoutine()
    {
      if (playerModel.remainingTime <= 0 || !playerModel.isAlive)
      {
        StopCoroutine(ScoreRoutine());
      }
      
      while (playerModel.remainingTime > 0 && playerModel.isAlive)
      {
        yield return new WaitForSeconds(1f);

        playerModel.ChangeScore(1);
        view.UpdateScore(playerModel.score);
      }
    }

    private void OnDashFinished()
    {
      view.StartDashTimer();
    }

    private void OnFire()
    {
      view.StartFireTimer();
    }
    
    private void OnChangeSpeed()
    {
      if (_hasSlowedDown)
      {
        StopCoroutine(_timeLoop);
        _hasSlowedDown = false;

        if (playerModel.isAlive)
        {
          _timeLoop = StartCoroutine(DecreaseRemainingTime());
        }
      }

      view.SetIcon(speedModel.speedState);
    }

    private void OnSlowDown()
    {
      _hasSlowedDown = true;

      view.SetIcon(speedModel.speedState);

      StopCoroutine(_timeLoop);
      _timeLoop = StartCoroutine(IncreaseRemainingTime());
    }

    private void StartShadowLoop()
    {
      _shadowLoop ??= StartCoroutine(ShadowLoop());
    }

    private void StopShadowLoop()
    {
      if (_shadowLoop == null)
      {
        return;
      }

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
      float spawnDistance = Mathf.Abs(playerModel.position - enemyModel.spawnPosition);
      float currentDistance = Mathf.Abs(playerModel.position - enemyModel.currentPosition);

      if (currentDistance >= spawnDistance)
      {
        view.SetShadowOpacity(0);
      }
      else
      {
        float a = 1 - currentDistance / spawnDistance;
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
    
    private void OnFlyingObstacleIncoming()
    {
      if (_warningRoutine != null)
      {
        StopCoroutine(_warningRoutine);
      }

      _warningRoutine = StartCoroutine(ObstacleWarningRoutine());
      StartCoroutine(ObstacleWarningRoutine());
    }

    private IEnumerator ObstacleWarningRoutine()
    {
      view.flyingObstacleWarning.SetActive(true);
      yield return new WaitForSeconds(GameMechanicSettings.FlyingObstacleWarningTime);
      view.flyingObstacleWarning.SetActive(false);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(GameHudEvent.Settings, OnSettings);

      dispatcher.RemoveListener(PlayerEvent.EnemyStartedMoving, StartShadowLoop);
      dispatcher.RemoveListener(PlayerEvent.EnemyStoppedMoving, StopShadowLoop);
      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);
      dispatcher.RemoveListener(PlayerEvent.SlowDown, OnChangeSpeed);
      dispatcher.RemoveListener(PlayerEvent.SpeedUp, OnChangeSpeed);
      dispatcher.RemoveListener(PlayerEvent.ReturnNormalSpeed, OnChangeSpeed);
      dispatcher.RemoveListener(PlayerEvent.DashFinished, OnDashFinished);
      dispatcher.RemoveListener(PlayerEvent.FireBullet, OnFire);
      dispatcher.RemoveListener(PlayerEvent.FlyingObstacleIncoming, OnFlyingObstacleIncoming);
    }
  }
}
