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
    Jump,
    Crouch
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
      view.dispatcher.AddListener(PlayerControllerEvents.SlowDownTime, OnSlowDownTimeAction);
      view.dispatcher.AddListener(PlayerControllerEvents.SpeedUpTime, OnSpeedUpTimeAction);
      view.dispatcher.AddListener(PlayerControllerEvents.ReturnNormalSpeed, OnReturnNormalSpeed);
      view.dispatcher.AddListener(PlayerControllerEvents.Jump, OnJumpAction);
      view.dispatcher.AddListener(PlayerControllerEvents.Crouch, OnCrouchAction);

      dispatcher.AddListener(GameEvent.Died, OnDeathProcess);
    }

    private void OnCrouchAction()
    {
      if (!playerModel.isAlive)
      {
        return;
      }

      view.SetColliders();
    }

    private void OnDeathProcess()
    {
      view.DeathProcess();
    }

    private void OnJumpAction()
    {
      if (!playerModel.isAlive)
      {
        return;
      }

      float adjustedJumpSpeed = playerModel.jumpSpeed;
      view.playerRigidboyd2d.velocity += new Vector2(0f, adjustedJumpSpeed);
    }

    private void OnReturnNormalSpeed()
    {
      playerModel.ReturnNormalSpeed();
    }

    private void OnDashAction()
    {
      if (!playerModel.isAlive)
      {
        return;
      }

      if (playerModel.isDashing)
      {
        return;
      }

      StartCoroutine(DashTimer());
    }

    private IEnumerator DashTimer()
    {
      playerModel.isDashing = true;
      view.ChangeColor(Color.blue);
      yield return new WaitForSeconds(view.dashDuration / playerModel.currentSpeed);
      playerModel.isDashing = false;
      view.ChangeColor(Color.white);
    }

    private void OnFireBulletAction()
    {
      dispatcher.Dispatch(GameEvent.FireBullet, view.gameObject.transform);
    }

    private void OnSlowDownTimeAction()
    {
      playerModel.SlowDownTime();
    }

    private void OnSpeedUpTimeAction()
    {
      playerModel.SpeedUpTime();
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(PlayerControllerEvents.FireBulletAction, OnFireBulletAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.Dash, OnDashAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.SlowDownTime, OnSlowDownTimeAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.SpeedUpTime, OnSpeedUpTimeAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.ReturnNormalSpeed, OnReturnNormalSpeed);
      view.dispatcher.RemoveListener(PlayerControllerEvents.Jump, OnJumpAction);
      view.dispatcher.AddListener(PlayerControllerEvents.Crouch, OnCrouchAction);

      dispatcher.RemoveListener(GameEvent.Died, OnDeathProcess);
    }
  }
}
