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
      StartCoroutine(Off());  

      speedModel.ReturnNormalSpeed();
      view.SetActionMapState(true);

      StartCoroutine(SpeedUpGame());
    }
    
    private IEnumerator Off()
    {
      view.ResetPosition();
      yield return new WaitForEndOfFrame();
      playerModel.position = GameMechanicSettings.PlayerSpawnPosition.x - (view.playerBodyCollider.bounds.extents.x * view.playerBodyCollider.transform.localScale.x);;
    }

    private IEnumerator SpeedUpGame()
    {
      while (playerModel.currentGameSpeed < GameMechanicSettings.MaxGameSpeed && playerModel.isAlive)
      {
        yield return new WaitForSecondsRealtime(GameMechanicSettings.GameSpeedUpRate);
        playerModel.ChangeGameSpeed(GameMechanicSettings.GameSpeedUpAmount);
      }

      if (playerModel.currentGameSpeed >= GameMechanicSettings.MaxGameSpeed || !playerModel.isAlive)
      {
        StopCoroutine(SpeedUpGame());
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
      
      float posY = transform.position.y;

      Tween jump = view.transform.DOMoveY(GameMechanicSettings.JumpHeight, GameMechanicSettings.JumpSpeed)
        .SetEase(Ease.OutSine)
        .SetSpeedBased()
        .OnComplete(() => view.transform.DOMoveY(posY, GameMechanicSettings.JumpSpeed)
          .SetSpeedBased()
          .SetEase(Ease.InQuad)
          .OnComplete(() => dispatcher.Dispatch(PlayerEvent.Jump)));

      jump.Play();
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
      playerModel.ChangeRemainingTime(-GameMechanicSettings.DashCost);
      StartCoroutine(DashTimer());
    }

    private IEnumerator DashTimer()
    {
      playerModel.isDashing = true;
      dispatcher.Dispatch(PlayerEvent.Dash);
      view.ChangeColor(new Color(0.3f, 0.8f, 1f, 0.75f));
      playerModel.ChangeGameSpeed(GameMechanicSettings.DashSpeed);
      yield return new WaitForSeconds(GameMechanicSettings.DashDuration);
      playerModel.ChangeGameSpeed(-GameMechanicSettings.DashSpeed);
      playerModel.isDashing = false;
      view.ChangeColor(Color.white);
      dispatcher.Dispatch(PlayerEvent.DashFinished);
      StartCoroutine(view.DashCooldown());
    }

    private void OnFireBulletAction()
    {
      view.isFireReady = false;

      playerModel.ChangeRemainingTime(-GameMechanicSettings.FireCost);
      dispatcher.Dispatch(PlayerEvent.FireBullet, view.gameObject.transform);
      StartCoroutine(view.FireCooldown());
    }

    private void OnSlowDownTimeAction()
    {
      speedModel.SlowDownTime();
    }

    private void OnSpeedUpTimeAction()
    {
      speedModel.SpeedUpTime();
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
