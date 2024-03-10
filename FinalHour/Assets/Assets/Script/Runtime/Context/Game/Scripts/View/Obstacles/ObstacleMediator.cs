﻿using Assets.Script.Runtime.Context.Game.Scripts.Enum;
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

      dispatcher.AddListener(PlayerEvent.SlowDown, OnUpdateSpeed);
      dispatcher.AddListener(PlayerEvent.SpeedUp, OnUpdateSpeed);
      dispatcher.AddListener(PlayerEvent.ReturnNormalSpeed, OnUpdateSpeed);
      dispatcher.AddListener(PlayerEvent.Died, OnDied);

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
      Destroy(view.gameObject);
      view.InstantiateObject(view.crushParticle);
      dispatcher.Dispatch(PlayerEvent.CrashObstacle);
    }

    private void OnUpdateSpeed()
    {
      Vector2 obstacleSpeed = new(-playerModel.movementSpeed * GameControlSettings.obstacleSpeed, 0f);
      view.TranslateObstacle(obstacleSpeed);
    }

    private void OnDied()
    {
      Destroy(gameObject);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(ObstacleEvents.CrashWithPlayer, OnCrashWithPlayer);
      view.dispatcher.RemoveListener(ObstacleEvents.ObstacleIsBroken, OnObstacleIsBroken);

      dispatcher.RemoveListener(PlayerEvent.SlowDown, OnUpdateSpeed);
      dispatcher.RemoveListener(PlayerEvent.SpeedUp, OnUpdateSpeed);
      dispatcher.RemoveListener(PlayerEvent.ReturnNormalSpeed, OnUpdateSpeed);
      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);
    }
  }
}