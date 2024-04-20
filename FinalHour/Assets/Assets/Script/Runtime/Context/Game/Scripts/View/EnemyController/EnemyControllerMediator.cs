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
    HitLimit
  }

  public class EnemyControllerMediator : EventMediator
  {
    [Inject]
    public EnemyControllerView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }

    [Inject]
    public IEnemyModel enemyModel { get; set; }
    
    [Inject]
    public ISpeedModel speedModel { get; set; }

    private Coroutine _positionLoop;

    public override void OnRegister()
    {
      view.dispatcher.AddListener(EnemyControllerEvent.CaughtPlayer, OnCaughtPlayer);
      view.dispatcher.AddListener(EnemyControllerEvent.HitLimit, OnReturnNormalSpeed);
      
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
      enemyModel.spawnPosition = view.enemyBoxCollider.bounds.center.x + view.enemyBoxCollider.bounds.extents.x;
      enemyModel.currentPosition = view.enemyBoxCollider.bounds.center.x + view.enemyBoxCollider.bounds.extents.x;
    }

    private void UpdateModel()
    {
      enemyModel.currentPosition = view.enemyBoxCollider.bounds.center.x + view.enemyBoxCollider.bounds.extents.x;
    }

    private void OnCaughtPlayer()
    {
      playerModel.Die();
      view.MoveEnemy(0f);
    }

    private void OnSlowDown()
    {
      view.MoveEnemy(GameControlSettings.EnemySpeed);
      StartPositionLoop();

      dispatcher.Dispatch(PlayerEvent.EnemyStartedMoving);
    }

    private void OnSpeedUp()
    {
      if (view.enemyRigidBody.IsTouchingLayers(LayerMask.GetMask("Barrier")))
      {
        return;
      }

      view.MoveEnemy(-GameControlSettings.EnemySpeed);
      StartPositionLoop();

      dispatcher.Dispatch(PlayerEvent.EnemyStartedMoving);
    }

    private void OnReturnNormalSpeed()
    {
      view.MoveEnemy(0);
      StopPositionLoop();

      dispatcher.Dispatch(PlayerEvent.EnemyStoppedMoving);
    }

    private void StartPositionLoop()
    {
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
    }

    private IEnumerator PositionLoop()
    {
      while (true)
      {
        UpdateModel();

        yield return null;
      }
    }

    private void OnDied()
    {
      view.MoveEnemy(0);
    }

    private void OnCrashObstacle()
    {
      StartCoroutine(CrashRoutine());
    }

    private IEnumerator CrashRoutine()
    {
      dispatcher.Dispatch(PlayerEvent.EnemyStartedMoving);
      view.MoveEnemyCrash();
      StartPositionLoop();
      yield return new WaitForSeconds(0.5f);
      switch (speedModel.speedState)
      {
        case SpeedState.Fast:
          OnSpeedUp();
          break;
        case SpeedState.Normal:
          OnReturnNormalSpeed();
          break;
        case SpeedState.Slow:
          OnSlowDown();
          break;
      }
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(EnemyControllerEvent.CaughtPlayer, OnCaughtPlayer);
      view.dispatcher.RemoveListener(EnemyControllerEvent.HitLimit, OnReturnNormalSpeed);
      
      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);
      dispatcher.RemoveListener(PlayerEvent.SlowDown, OnSlowDown);
      dispatcher.RemoveListener(PlayerEvent.SpeedUp, OnSpeedUp);
      dispatcher.RemoveListener(PlayerEvent.ReturnNormalSpeed, OnReturnNormalSpeed);
      dispatcher.RemoveListener(PlayerEvent.CrashObstacle, OnCrashObstacle);
    }
  }
}
