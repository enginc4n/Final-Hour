using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.EnemyController
{
  public enum EnemyControllerEvent
  {
    CaughtPlayer
  }
  public class EnemyControllerMediator : EventMediator
  {
    [Inject]
    public EnemyControllerView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(EnemyControllerEvent.CaughtPlayer, OnCaughtPlayer);
      
      dispatcher.AddListener(GameEvent.SlowDown, OnSlowDown);
      dispatcher.AddListener(GameEvent.SpeedUp, OnSpeedUp);
      dispatcher.AddListener(GameEvent.ReturnNormalSpeed, OnReturnNormalSpeed);
    }

    private void OnCaughtPlayer()
    { 
      playerModel.Die();
    }
    
    private void OnSlowDown()
    {
      view.MoveEnemy(playerModel.enemySpeed);
    }
    
    private void OnSpeedUp()
    {
      if (!view.enemyRigidBody.IsTouchingLayers(LayerMask.GetMask("Default")))
      {
        view.MoveEnemy(-playerModel.enemySpeed);
      }
    }

    private void OnReturnNormalSpeed()
    {
      view.MoveEnemy(0);
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
