using System;
using System.Collections;
using System.IO;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.EnemyController
{
  public enum EnemyControllerEvent
  {
    CaughtPlayer,
    EnemyMoved
  }

  public class EnemyControllerMediator : EventMediator
  {
    [Inject]
    public EnemyControllerView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }

    [Inject]
    public IEnemyModel enemyModel { get; set; }
    

    private Coroutine _positionLoop;

    private SpeedState _lastState;
    
    private float enemyPositionFromRight => transform.GetComponent<RectTransform>().anchoredPosition.x + (view.enemyBoxCollider.bounds.extents.x * view.enemyBoxCollider.transform.localScale.x);

    public override void OnRegister()
    {
      view.dispatcher.AddListener(EnemyControllerEvent.CaughtPlayer, OnCaughtPlayer);
      view.dispatcher.AddListener(EnemyControllerEvent.EnemyMoved, StartPositionLoop);

      dispatcher.AddListener(PlayerEvent.Died, OnDied);
      dispatcher.AddListener(PlayerEvent.SlowDown, OnSlowDown);
      dispatcher.AddListener(PlayerEvent.SpeedUp, OnSpeedUp);
      dispatcher.AddListener(PlayerEvent.ReturnNormalSpeed, OnReturnNormalSpeed);
      dispatcher.AddListener(PlayerEvent.CrashObstacle, OnCrashObstacle);
    }
    
    public override void OnInitialize()
    {
      StartCoroutine(Off());  
    }

    private IEnumerator Off()
    {
      yield return new WaitForEndOfFrame();
      view.ResetPosition();
      yield return new WaitForEndOfFrame();
      enemyModel.spawnPosition = enemyPositionFromRight;
      enemyModel.currentPosition = enemyPositionFromRight;
    }

    private void UpdateModel()
    {
      enemyModel.currentPosition = enemyPositionFromRight;
    }

    private void OnCaughtPlayer()
    {
      playerModel.Die();
    }

    private void OnSlowDown()
    {
      float lastSpeed = view.modifiedSpeed;
        
      if (_lastState == SpeedState.Fast)
      {
        view.speed += GameMechanicSettings.EnemySpeed*2;
      }
      else
      {
        view.speed += GameMechanicSettings.EnemySpeed;
      }
      
      _lastState = SpeedState.Slow;
      CheckMovementStart(lastSpeed);
    }

    private void OnSpeedUp()
    {
      float lastSpeed = view.modifiedSpeed;

      if (_lastState == SpeedState.Slow)
      {
        view.speed -= GameMechanicSettings.EnemySpeed*2;
      }
      else
      {
        view.speed -= GameMechanicSettings.EnemySpeed;
      }
      
      _lastState = SpeedState.Fast;
      CheckMovementStart(lastSpeed);
    }

    private void OnReturnNormalSpeed()
    {
      float lastSpeed = view.modifiedSpeed;
      
      if (_lastState == SpeedState.Fast)
      {
        view.speed += GameMechanicSettings.EnemySpeed;
      } else if (_lastState == SpeedState.Slow)
      {
        view.speed -= GameMechanicSettings.EnemySpeed;
      }

      if (view.speed != 0) return;
      _lastState = SpeedState.Normal;
      CheckMovementStart(lastSpeed);
    }
    
    private void StartPositionLoop()
    {
      dispatcher.Dispatch(PlayerEvent.EnemyStartedMoving);
      _positionLoop ??= StartCoroutine(PositionLoop());
    }

    private void StopPositionLoop()
    {
      if (_positionLoop == null)
      {
        return;
      }

      StopCoroutine(_positionLoop);
      _positionLoop = null;
      dispatcher.Dispatch(PlayerEvent.EnemyStoppedMoving);
    }

    private IEnumerator PositionLoop()
    {
      while (view.modifiedSpeed != 0)
      {
        UpdateModel();

        yield return null;
      }

      if (view.modifiedSpeed == 0)
      {
        StopPositionLoop();
      }
    }

    private void OnDied()
    {
      view.speed = 0f;
      view.crashCount = 0;
      _lastState = SpeedState.Normal;
    }

    private void OnCrashObstacle()
    {
      float lastSpeed = view.modifiedSpeed;
      
      view.crashCount++;
      view.crashRemainingDistance += GameMechanicSettings.EnemySpeed * GameMechanicSettings.CrashPunishment; 
      
      CheckMovementStart(lastSpeed);
    }

    private void CheckMovementStart(float lastSpeed)
    {
      if (lastSpeed != 0)
      {
        return;
      }
      StartPositionLoop();
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(EnemyControllerEvent.CaughtPlayer, OnCaughtPlayer);
      view.dispatcher.RemoveListener(EnemyControllerEvent.EnemyMoved, StartPositionLoop);

      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);
      dispatcher.RemoveListener(PlayerEvent.SlowDown, OnSlowDown);
      dispatcher.RemoveListener(PlayerEvent.SpeedUp, OnSpeedUp);
      dispatcher.RemoveListener(PlayerEvent.ReturnNormalSpeed, OnReturnNormalSpeed);
      dispatcher.RemoveListener(PlayerEvent.CrashObstacle, OnCrashObstacle);
    }
  }
}
