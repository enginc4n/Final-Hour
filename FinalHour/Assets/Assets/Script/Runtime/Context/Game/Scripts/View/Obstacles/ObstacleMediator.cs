using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View
{
  public class ObstacleMediator : EventMediator
  {
    [Inject]
    public ObstacleView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(GameEvent.SlowDown, OnUpdateSpeed);
      dispatcher.AddListener(GameEvent.SpeedUp, OnUpdateSpeed);
      dispatcher.AddListener(GameEvent.ReturnNormalSpeed, OnUpdateSpeed);

      OnUpdateSpeed();
    }

    private void OnUpdateSpeed()
    {
      Vector2 obstacleSpeed = new(-playerModel.movementSpeed * GameControlSettings.obstacleSpeed, 0f);
      view.TranslateObstacle(obstacleSpeed);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(GameEvent.SlowDown, OnUpdateSpeed);
      dispatcher.RemoveListener(GameEvent.SpeedUp, OnUpdateSpeed);
      dispatcher.RemoveListener(GameEvent.ReturnNormalSpeed, OnUpdateSpeed);
    }
  }
}
