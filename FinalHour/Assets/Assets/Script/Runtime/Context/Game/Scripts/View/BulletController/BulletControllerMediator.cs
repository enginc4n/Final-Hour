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
      dispatcher.AddListener(PlayerEvent.SlowDown, OnUpdateSpeed);
      dispatcher.AddListener(PlayerEvent.SpeedUp, OnUpdateSpeed);
      dispatcher.AddListener(PlayerEvent.ReturnNormalSpeed, OnUpdateSpeed);

      OnUpdateSpeed();
    }

    private void OnUpdateSpeed()
    {
      Vector2 bulletSpeed = new(playerModel.bulletSpeed, 0f);
      view.TranslateBullet(bulletSpeed);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(PlayerEvent.SlowDown, OnUpdateSpeed);
      dispatcher.RemoveListener(PlayerEvent.SpeedUp, OnUpdateSpeed);
      dispatcher.RemoveListener(PlayerEvent.ReturnNormalSpeed, OnUpdateSpeed);
    }
  }
}
