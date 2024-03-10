using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.BulletController
{
  public class BulletControllerMediator : EventMediator
  {
    [Inject]
    public BulletControllerView view { get; set; }

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
      Vector2 bulletSpeed = new(playerModel.bulletSpeed, 0f);
      view.TranslateBullet(bulletSpeed);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(GameEvent.SlowDown, OnUpdateSpeed);
      dispatcher.RemoveListener(GameEvent.SpeedUp, OnUpdateSpeed);
      dispatcher.RemoveListener(GameEvent.ReturnNormalSpeed, OnUpdateSpeed);
    }
  }
}
