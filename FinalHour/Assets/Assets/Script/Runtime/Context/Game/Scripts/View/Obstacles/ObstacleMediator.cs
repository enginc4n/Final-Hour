using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.Obstacles
{
  public enum ObstacleEvents
  {
    ObstacleIsBroken,
    CrashWithPlayer
  }

  public class ObstacleMediator : EventMediator
  {
    [Inject]
    public ObstacleView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(ObstacleEvents.CrashWithPlayer, OnCrashWithPlayer);
      view.dispatcher.AddListener(ObstacleEvents.ObstacleIsBroken, OnObstacleIsBroken);
      
      dispatcher.AddListener(PlayerEvent.Died, OnDied);
    }
    
    private void OnObstacleIsBroken()
    {
      view.InstantiateObject(view.breakParticle);
      if (view.isDropTime)
      {
        view.InstantiateObject(view.timeAdder);
      }
      
      Destroy(view.gameObject);
    }

    private void OnCrashWithPlayer()
    {
      if (view.isCollectible)
      {
        CollectibleProcess();
      }
      else
      {
        if (!playerModel.isDashing)
        {
          CrashObstacleProcess();
        }
      }
    }

    private void CollectibleProcess()
    {
      playerModel.remainingTime += GameMechanicSettings.CollectibleTimeAmount;
      view.InstantiateObject(view.collectParticle);
      dispatcher.Dispatch(PlayerEvent.Collect);
      Destroy(view.gameObject);
    }

    private void CrashObstacleProcess()
    {
      view.InstantiateObject(view.crushParticle);
      dispatcher.Dispatch(PlayerEvent.CrashObstacle);
      Destroy(view.gameObject);
    }

    private void FixedUpdate()
    {
      if (!playerModel.isAlive)
      {
        return;
      }

      transform.Translate(new Vector2(-playerModel.currentGameSpeed*view.ownSpeedFactor, 0), Space.World);
    }

    private void OnDied()
    {
      Destroy(gameObject);
    }
    
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(ObstacleEvents.CrashWithPlayer, OnCrashWithPlayer);
      view.dispatcher.RemoveListener(ObstacleEvents.ObstacleIsBroken, OnObstacleIsBroken);
      
      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);
    }
  }
}
