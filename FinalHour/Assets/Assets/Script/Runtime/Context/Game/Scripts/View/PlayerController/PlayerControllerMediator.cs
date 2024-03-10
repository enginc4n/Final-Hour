using System.Collections;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using DG.Tweening;
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

    private Tweener _jumpTween;

    public override void OnRegister()
    {
      view.dispatcher.AddListener(PlayerControllerEvents.FireBulletAction, OnFireBulletAction);
      view.dispatcher.AddListener(PlayerControllerEvents.Dash, OnDashAction);
      view.dispatcher.AddListener(PlayerControllerEvents.SlowDownTime, OnSlowDownTimeAction);
      view.dispatcher.AddListener(PlayerControllerEvents.SpeedUpTime, OnSpeedUpTimeAction);
      view.dispatcher.AddListener(PlayerControllerEvents.ReturnNormalSpeed, OnReturnNormalSpeed);
      view.dispatcher.AddListener(PlayerControllerEvents.Jump, OnJumpAction);
      view.dispatcher.AddListener(PlayerControllerEvents.Crouch, OnCrouchAction);

      dispatcher.AddListener(PlayerEvent.Died, OnDeathProcess);
      dispatcher.AddListener(PlayerEvent.Play, OnInitialize);
    }

    public override void OnInitialize()
    {
      playerModel.position = view.playerBodyCollider.bounds.center.x - view.playerBodyCollider.bounds.extents.x;
      view.SetActionMapState(true);
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
      view.SetActionMapState(false);
    }

    private void OnJumpAction()
    {
      if (!playerModel.isAlive)
      {
        return;
      }

      view.playerRigidbody2D.DOMoveY(1.5f, 1);
      dispatcher.Dispatch(SoundEvents.Jump);
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
      dispatcher.Dispatch(PlayerEvent.FireBullet, view.gameObject.transform);
      dispatcher.Dispatch(SoundEvents.Fire);
    }

    private void OnSlowDownTimeAction()
    {
      playerModel.SlowDownTime();
      dispatcher.Dispatch(SoundEvents.SlowDownSpeed);
    }

    private void OnSpeedUpTimeAction()
    {
      playerModel.SpeedUpTime();
      dispatcher.Dispatch(SoundEvents.SpeedUpTime);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(PlayerControllerEvents.FireBulletAction, OnFireBulletAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.Dash, OnDashAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.SlowDownTime, OnSlowDownTimeAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.SpeedUpTime, OnSpeedUpTimeAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.ReturnNormalSpeed, OnReturnNormalSpeed);
      view.dispatcher.RemoveListener(PlayerControllerEvents.Jump, OnJumpAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.Crouch, OnCrouchAction);

      dispatcher.RemoveListener(PlayerEvent.Died, OnDeathProcess);
      dispatcher.RemoveListener(PlayerEvent.Play, OnInitialize);
    }
  }
}
