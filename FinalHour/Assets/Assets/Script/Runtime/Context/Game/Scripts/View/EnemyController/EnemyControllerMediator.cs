using System;
using System.Collections;
using System.Data;
using System.IO;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
using DG.Tweening;
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

      dispatcher.AddListener(PlayerEvent.DashStarted, OnDashStarted);
      dispatcher.AddListener(PlayerEvent.DashFinished, OnDashFinished);
      dispatcher.AddListener(PlayerEvent.CollectDash, OnDashStarted);
      dispatcher.AddListener(PlayerEvent.CollectedDashComplete, OnDashFinished);
      dispatcher.AddListener(PlayerEvent.Died, OnDied);
      dispatcher.AddListener(PlayerEvent.SlowDown, OnSlowDown);
      dispatcher.AddListener(PlayerEvent.SpeedUp, OnSpeedUp);
      dispatcher.AddListener(PlayerEvent.ReturnNormalSpeed, OnReturnNormalSpeed);
      dispatcher.AddListener(PlayerEvent.CrashObstacle, OnCrashObstacle);
     
      dispatcher.AddListener(GameEvent.Pause, OnPause);
      dispatcher.AddListener(GameEvent.Continue, OnContinue);
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
    
    private void OnDashStarted()
    {
      float lastSpeed = view.modifiedSpeed;

      view.speed -= GameMechanicSettings.EnemySpeed + (GameMechanicSettings.EnemySpeed * GameMechanicSettings.DashSpeed);
      
      CheckMovementStart(lastSpeed);
    }
    
    private void OnDashFinished()
    {
      if (!playerModel.isAlive)
      {
        return;
      }
      
      float lastSpeed = view.modifiedSpeed;
      
      view.speed += GameMechanicSettings.EnemySpeed + (GameMechanicSettings.EnemySpeed * GameMechanicSettings.DashSpeed);
      
      CheckMovementStart(lastSpeed);
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

      if (view.speed == -9999f || !playerModel.isAlive)
      {
        view.speed = 0f;
        _lastState = SpeedState.Normal;
        return;
      }
      
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
      if (view.modifiedSpeed > 0)
      {
        dispatcher.Dispatch(PlayerEvent.EnemyCloser);
      }
      
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
      view.alreadyDead = true;
      view.speed = 0f;
      view.crashCount = 0;
      _lastState = SpeedState.Normal;

      if (view.enemyBoxCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
      {
        view.KillAnimation();
      }
      else
      {
        StartCoroutine(Catch());
      }
    }

    private IEnumerator Catch()
    {
      while (!view.enemyBoxCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
      {
        transform.Translate(Vector2.right * (5 * Time.deltaTime));

        // Wait for the next frame
        yield return null;
      }

      view.KillAnimation();
    }

    private void OnCrashObstacle()
    {
      if (!playerModel.isAlive)
      {
        return;
      }
      
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
    
    private void OnPause()
    {
      view.enemyAnimator.enabled = false;
    }
    
    private void OnContinue()
    {
      view.wasPaused = true;
      view.enemyAnimator.enabled = true;
    }

    public override void OnRemove()
    {
      transform.GetComponent<RectTransform>().DOKill();
      view.spriteRenderer.DOKill();
      
      view.dispatcher.RemoveListener(EnemyControllerEvent.CaughtPlayer, OnCaughtPlayer);
      view.dispatcher.RemoveListener(EnemyControllerEvent.EnemyMoved, StartPositionLoop);

      dispatcher.RemoveListener(PlayerEvent.DashStarted, OnDashStarted);
      dispatcher.RemoveListener(PlayerEvent.DashFinished, OnDashFinished);
      dispatcher.RemoveListener(PlayerEvent.CollectDash, OnDashStarted);
      dispatcher.RemoveListener(PlayerEvent.CollectedDashComplete, OnDashFinished);
      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);
      dispatcher.RemoveListener(PlayerEvent.SlowDown, OnSlowDown);
      dispatcher.RemoveListener(PlayerEvent.SpeedUp, OnSpeedUp);
      dispatcher.RemoveListener(PlayerEvent.ReturnNormalSpeed, OnReturnNormalSpeed);
      dispatcher.RemoveListener(PlayerEvent.CrashObstacle, OnCrashObstacle);
      
      dispatcher.RemoveListener(GameEvent.Pause, OnPause);
      dispatcher.RemoveListener(GameEvent.Continue, OnContinue);
    }
  }
}
