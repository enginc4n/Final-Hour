using System.Collections;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
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

    [Inject]
    public ISpeedModel speedModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(PlayerControllerEvents.FireBulletAction, OnFireBulletAction);
      view.dispatcher.AddListener(PlayerControllerEvents.Dash, OnDashAction);
      view.dispatcher.AddListener(PlayerControllerEvents.SlowDownTime, OnSlowDownTimeAction);
      view.dispatcher.AddListener(PlayerControllerEvents.SpeedUpTime, OnSpeedUpTimeAction);
      view.dispatcher.AddListener(PlayerControllerEvents.ReturnNormalSpeed, OnReturnNormalSpeed);
      view.dispatcher.AddListener(PlayerControllerEvents.Jump, OnJumpAction);
      view.dispatcher.AddListener(PlayerControllerEvents.Crouch, OnCrouchAction);

      dispatcher.AddListener(PlayerEvent.Died, OnDied);
      dispatcher.AddListener(GameEvent.Pause, OnPause);
      dispatcher.AddListener(GameEvent.Continue, OnContinue);
    }

    public override void OnInitialize()
    {
      playerModel.position = view.playerBodyCollider.bounds.center.x - view.playerBodyCollider.bounds.extents.x;

      speedModel.ReturnNormalSpeed();
      view.SetActionMapState(true);

      StartCoroutine(SpeedUpGame());
    }

    private IEnumerator SpeedUpGame()
    {
      while (playerModel.currentGameSpeed < GameControlSettings.MaxGameSpeed && playerModel.isAlive)
      {
        yield return new WaitForSecondsRealtime(GameControlSettings.GameSpeedUpRate);
        playerModel.currentGameSpeed += GameControlSettings.GameSpeedUpAmount;
        dispatcher.Dispatch(PlayerEvent.GameSpeedUp);
      }
    }

    private void OnCrouchAction()
    {
      view.SetColliders();
    }

    private void OnDied()
    {
      speedModel.ReturnNormalSpeed();

      view.SetActionMapState(false);
    }

    private void OnPause()
    {
      view.DisableInputs();
    }

    private void OnContinue()
    {
      if (playerModel.isAlive)
      {
        view.EnableInputs();
      }
    }

    private void OnJumpAction()
    {
      if (!playerModel.isAlive)
      {
        return;
      }

      view.playerRigidbody2D.DOMoveY(GameControlSettings.JumpHeight, GameControlSettings.JumpSpeed).SetSpeedBased();
      dispatcher.Dispatch(SoundEvent.Jump);
    }

    private void OnReturnNormalSpeed()
    {
      speedModel.ReturnNormalSpeed();
    }

    private void OnDashAction()
    {
      if (playerModel.isDashing)
      {
        return;
      }

      view.isDashReady = false;
      playerModel.ChangeRemainingTime(-GameControlSettings.DashCost);
      StartCoroutine(DashTimer());
    }

    private IEnumerator DashTimer()
    {
      playerModel.isDashing = true;
      view.ChangeColor(new Color(0.3f, 0.8f, 1f, 0.75f));
      playerModel.currentGameSpeed += GameControlSettings.DashSpeed;
      yield return new WaitForSeconds(GameControlSettings.DashDuration);
      playerModel.currentGameSpeed -= GameControlSettings.DashSpeed;
      playerModel.isDashing = false;
      view.ChangeColor(Color.white);
      dispatcher.Dispatch(PlayerEvent.Dash);
      StartCoroutine(view.DashCooldown());
    }

    private void OnFireBulletAction()
    {
      view.isFireReady = false;
      
      playerModel.ChangeRemainingTime(-GameControlSettings.FireCost);
      dispatcher.Dispatch(PlayerEvent.FireBullet, view.gameObject.transform);
      dispatcher.Dispatch(SoundEvent.Fire);
      StartCoroutine(view.FireCooldown());
    }

    private void OnSlowDownTimeAction()
    {
      speedModel.SlowDownTime();
      dispatcher.Dispatch(SoundEvent.SlowDownSpeed);
    }

    private void OnSpeedUpTimeAction()
    {
      speedModel.SpeedUpTime();
      dispatcher.Dispatch(SoundEvent.SpeedUpTime);
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

      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);
      dispatcher.RemoveListener(GameEvent.Pause, OnPause);
      dispatcher.RemoveListener(GameEvent.Continue, OnContinue);
    }
  }
}
