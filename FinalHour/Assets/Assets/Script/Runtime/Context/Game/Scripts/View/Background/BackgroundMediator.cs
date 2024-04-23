using System.Collections;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.Background
{
  public class BackgroundMediator : EventMediator
  {
    [Inject]
    public BackgroundView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }
    
    [Inject]
    public ISpeedModel speedModel { get; set; }
    
    public override void OnInitialize()
    {
      UpdateSpeed();
      UpdateAliveStatus();

      dispatcher.AddListener(PlayerEvent.GameSpeedUpdated, UpdateSpeed);
      dispatcher.AddListener(PlayerEvent.Died, UpdateAliveStatus);
    }

    private void UpdateSpeed()
    {
      view.speed = playerModel.currentGameSpeed * GameControlSettings.ObstacleSpeed;
    }
    
    private void UpdateAliveStatus()
    {
      view.isAlive = playerModel.isAlive;
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(PlayerEvent.GameSpeedUpdated, UpdateSpeed);
      dispatcher.RemoveListener(PlayerEvent.Died, UpdateAliveStatus);
    }
  }
}
