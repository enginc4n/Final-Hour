using System.Collections;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
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

    private Coroutine _positionLoop;

    public override void OnRegister()
    {
      view.dispatcher.AddListener(EnemyControllerEvent.CaughtPlayer, OnCaughtPlayer);
      view.dispatcher.AddListener(EnemyControllerEvent.HitLimit, OnReturnNormalSpeed);
      
      dispatcher.AddListener(GameEvent.SlowDown, OnSlowDown);
      dispatcher.AddListener(GameEvent.SpeedUp, OnSpeedUp);
      dispatcher.AddListener(GameEvent.ReturnNormalSpeed, OnReturnNormalSpeed);
    }
    
    public override void OnInitialize()
    {
      enemyModel.spawnPosition = view.enemyBoxCollider.bounds.center.x + view.enemyBoxCollider.bounds.extents.x;
    }

    private void UpdateModel()
    {
      enemyModel.position = view.enemyBoxCollider.bounds.center.x + view.enemyBoxCollider.bounds.extents.x;
    }

    private void OnCaughtPlayer()
    { 
      playerModel.Die();
      view.MoveEnemy(0f);
    }
    
    private void OnSlowDown()
    {
      view.MoveEnemy(playerModel.enemySpeed);
      StartPositionLoop();
      
      dispatcher.Dispatch(GameEvent.EnemyStartedMoving);
    }
    
    private void OnSpeedUp()
    {
      Debug.LogError("SpeedUp");
      if (view.enemyRigidBody.IsTouchingLayers(LayerMask.GetMask("Default"))) return;
      Debug.LogError("NotTouchingBorder");
      view.MoveEnemy(-playerModel.enemySpeed);
      StartPositionLoop();
      
      dispatcher.Dispatch(GameEvent.EnemyStartedMoving);
    }

    private void OnReturnNormalSpeed()
    {
      view.MoveEnemy(0);
      StopPositionLoop();
      
      dispatcher.Dispatch(GameEvent.EnemyStoppedMoving);
    }
    
    private void StartPositionLoop()
    {
      _positionLoop ??= StartCoroutine(PositionLoop());
    }
    
    private void StopPositionLoop()
    {
      if (_positionLoop == null) return;
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
    
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(EnemyControllerEvent.CaughtPlayer, OnCaughtPlayer);

      dispatcher.RemoveListener(GameEvent.SlowDown, OnSlowDown);
      dispatcher.RemoveListener(GameEvent.SpeedUp, OnSpeedUp);
      dispatcher.RemoveListener(GameEvent.ReturnNormalSpeed, OnReturnNormalSpeed);
    }
  }
}
