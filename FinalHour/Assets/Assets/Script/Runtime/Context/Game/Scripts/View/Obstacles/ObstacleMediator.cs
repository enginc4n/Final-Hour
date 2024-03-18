using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View
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
      
      dispatcher.AddListener(PlayerEvent.GameSpeedUp, OnGameSpeedUp);
      dispatcher.AddListener(PlayerEvent.Died, OnDied);
    }

    public override void OnInitialize()
    {
      view.TranslateObstacle(new Vector2(-playerModel.currentGameSpeed*20f, 0));
    }

    private void OnObstacleIsBroken()
    {
      view.InstantiateObject(view.crushParticle);
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
      playerModel.remainingTime += GameControlSettings.CollectibleTimeAmount;
      view.InstantiateObject(view.collectParticle);
      dispatcher.Dispatch(SoundEvent.Collect);
      Destroy(view.gameObject);
    }

    private void CrashObstacleProcess()
    {
      view.InstantiateObject(view.crushParticle);
      dispatcher.Dispatch(PlayerEvent.CrashObstacle);
      dispatcher.Dispatch(SoundEvent.Destroy);
      Destroy(view.gameObject);
    }

    private void OnGameSpeedUp()
    {
      view.TranslateObstacle(new Vector2(-playerModel.currentGameSpeed*20f, 0));
    }
    
    private void OnDied()
    {
      Destroy(gameObject);
    }
    
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(ObstacleEvents.CrashWithPlayer, OnCrashWithPlayer);
      view.dispatcher.RemoveListener(ObstacleEvents.ObstacleIsBroken, OnObstacleIsBroken);
      
      dispatcher.RemoveListener(PlayerEvent.GameSpeedUp, OnGameSpeedUp);
      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);
    }
  }
}
