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
      dispatcher.AddListener(GameEvents.SlownDown, OnUpdateSpeed);
      dispatcher.AddListener(GameEvents.SpeedUp, OnUpdateSpeed);
      dispatcher.AddListener(GameEvents.ReturnNormalSpeed, OnUpdateSpeed);

      OnUpdateSpeed();
    }

    private void OnUpdateSpeed()
    {
      Vector2 bulletSpeed = new(playerModel.currentPlayerSpeed * view.bulletSpeed, 0f);
      view.TranslateBullet(bulletSpeed);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(GameEvents.SlownDown, OnUpdateSpeed);
      dispatcher.RemoveListener(GameEvents.SpeedUp, OnUpdateSpeed);
      dispatcher.RemoveListener(GameEvents.ReturnNormalSpeed, OnUpdateSpeed);
    }
  }
}
