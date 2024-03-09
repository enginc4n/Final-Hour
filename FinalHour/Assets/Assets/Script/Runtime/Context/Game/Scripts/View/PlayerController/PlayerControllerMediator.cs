using System.Collections;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.PlayerController
{
  public enum PlayerControllerEvents
  {
    FireBulletAction,
    Dash,
    SlowDownTime,
    SpeedUpTime,
    ReturnNormalSpeed,
    Jump
  }

  public class PlayerControllerMediator : EventMediator
  {
    [Inject]
    public PlayerControllerView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(PlayerControllerEvents.FireBulletAction, OnFireBulletAction);
      view.dispatcher.AddListener(PlayerControllerEvents.Dash, OnDashAction);
      view.dispatcher.AddListener(PlayerControllerEvents.SlowDownTime, OnSlowTimeAction);
      view.dispatcher.AddListener(PlayerControllerEvents.SpeedUpTime, OnSpeedUpTimeAction);
      view.dispatcher.AddListener(PlayerControllerEvents.ReturnNormalSpeed, OnReturnNormalSpeed);
      view.dispatcher.AddListener(PlayerControllerEvents.Jump, OnJumpAction);
    }

    private void OnJumpAction()
    {
      float adjustedJumpSpeed = view.jumpSpeed * playerModel.currentPlayerSpeed;
      view.playerRigidboyd2d.velocity += new Vector2(0f, adjustedJumpSpeed);
    }

    private void OnReturnNormalSpeed()
    {
      playerModel.currentPlayerSpeed = playerModel.defaultSpeed;
      dispatcher.Dispatch(GameEvents.ReturnNormalSpeed);
    }

    private void OnDashAction()
    {
      if (!playerModel.isDashing)
      {
        StartCoroutine(DashTimer());
      }
    }

    private IEnumerator DashTimer()
    {
      playerModel.isDashing = true;
      view.ChangeColor(Color.blue);
      yield return new WaitForSeconds(view.dashDuration / playerModel.currentPlayerSpeed);
      playerModel.isDashing = false;
      view.ChangeColor(Color.white);
    }

    private void OnFireBulletAction()
    {
      dispatcher.Dispatch(GameEvents.FireBullet, view.gameObject.transform);
    }

    private void OnSlowTimeAction()
    {
      playerModel.currentPlayerSpeed = view.slowTimeSpeed;
      dispatcher.Dispatch(GameEvents.SlownDown);
    }

    private void OnSpeedUpTimeAction()
    {
      playerModel.currentPlayerSpeed = view.speedUpTimeSpeed;
      dispatcher.Dispatch(GameEvents.SpeedUp);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(PlayerControllerEvents.FireBulletAction, OnFireBulletAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.Dash, OnDashAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.SlowDownTime, OnSlowTimeAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.SpeedUpTime, OnSpeedUpTimeAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.ReturnNormalSpeed, OnReturnNormalSpeed);
      view.dispatcher.RemoveListener(PlayerControllerEvents.Jump, OnJumpAction);
    }
  }
}
