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

      dispatcher.AddListener(GameEvent.SlowDown, OnUpdateSpeed);
      dispatcher.AddListener(GameEvent.SpeedUp, OnUpdateSpeed);
      dispatcher.AddListener(GameEvent.ReturnNormalSpeed, OnUpdateSpeed);
      dispatcher.AddListener(GameEvent.Died, OnDied);

      OnUpdateSpeed();
    }

    private void OnObstacleIsBroken()
    {
      view.InstantiateObject(view.crushParticle);
      if (view.isDropTime)
      {
        view.InstantiateObject(view.timeAdder);
      }
    }

    private void OnCrashWithPlayer()
    {
      if (view.isCollectible)
      {
        CollectibleProcess();
      }
      else
      {
        CrashObstacleProcess();
      }
    }

    private void CollectibleProcess()
    {
      playerModel.remainingTime += GameControlSettings.addTimeAmount;
      Destroy(view.gameObject);
      view.InstantiateObject(view.collectParticle);
    }

    private void CrashObstacleProcess()
    {
      playerModel.remainingTime -= GameControlSettings.removeTimeAmount;
      Destroy(view.gameObject);
      view.InstantiateObject(view.crushParticle);
    }

    private void OnUpdateSpeed()
    {
      Vector2 obstacleSpeed = new(-playerModel.movementSpeed * GameControlSettings.obstacleSpeed, 0f);
      view.TranslateObstacle(obstacleSpeed);
    }

    private void OnDied()
    {
      DestroyImmediate(gameObject);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(ObstacleEvents.CrashWithPlayer, OnCrashWithPlayer);
      view.dispatcher.RemoveListener(ObstacleEvents.ObstacleIsBroken, OnObstacleIsBroken);

      dispatcher.RemoveListener(GameEvent.SlowDown, OnUpdateSpeed);
      dispatcher.RemoveListener(GameEvent.SpeedUp, OnUpdateSpeed);
      dispatcher.RemoveListener(GameEvent.ReturnNormalSpeed, OnUpdateSpeed);
      dispatcher.RemoveListener(GameEvent.Died, OnDied);
    }
  }
}
