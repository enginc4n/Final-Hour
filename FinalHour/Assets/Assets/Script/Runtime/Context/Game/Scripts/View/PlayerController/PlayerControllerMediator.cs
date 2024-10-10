using System.Collections;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
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
    ReturnNormalSpeedTutorial,
    Jump,
    Crouch,
    CrouchFinished
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
      view.dispatcher.AddListener(PlayerControllerEvents.ReturnNormalSpeedTutorial, OnReturnNormalSpeedTutorial);
      view.dispatcher.AddListener(PlayerControllerEvents.Jump, OnJumpAction);
      view.dispatcher.AddListener(PlayerControllerEvents.Crouch, OnCrouchAction);
      view.dispatcher.AddListener(PlayerControllerEvents.CrouchFinished, OnCrouchFinished);

      dispatcher.AddListener(PlayerEvent.Died, OnDied);
      dispatcher.AddListener(GameEvent.Pause, OnPause);
      dispatcher.AddListener(GameEvent.Continue, OnContinue);
      dispatcher.AddListener(PlayerEvent.CrashObstacle, OnCrashObstacle);
      dispatcher.AddListener(PlayerEvent.CollectDash, OnCollectDash);
      dispatcher.AddListener(GameEvent.SpeedTutorial, OnSpeedTutorial);
    }

    public override void OnInitialize()
    {
      if (PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) < 4)
      {
        playerModel.tutorialActive = true;
      }
      else
      {
        playerModel.tutorialActive = false;
        PlayerPrefs.SetInt(SettingKeys.CompletedTutorialSteps, 9);
      }
      
      view.SetColliders(false);
      view.deadParticle.SetActive(false);
      dispatcher.Dispatch(GameEvent.GameStarted);    
      StartCoroutine(Off());
      speedModel.ReturnNormalSpeed();
      
      view.SetInputs();
      
      if (playerModel.tutorialActive)
      {
        view.DisableAllInputs();
      }
      else
      {
        view.EnableAllInputs();
      }

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
        while (playerModel.trueGameSpeed < GameMechanicSettings.MaxGameSpeed && playerModel.isAlive)
        {
          if (speedModel.isPaused)
          {
            yield return new WaitUntil(() => !speedModel.isPaused);
          }

          yield return new WaitForSecondsRealtime(GameMechanicSettings.GameSpeedUpTime);

          if (speedModel.isPaused)
          {
            yield return new WaitUntil(() => !speedModel.isPaused);
          }

          playerModel.ChangeTrueGameSpeed(GameMechanicSettings.GameSpeedUpAmount);

          view.playerAnimator.SetFloat("speed", playerModel.trueGameSpeed / GameMechanicSettings.StartingGameSpeed);
        }
      }

    private void OnDied()
    {
      view.deadParticle.SetActive(true);
      speedModel.ReturnNormalSpeed();

      view.SetActionMapState(false);
      StopCoroutine(SpeedUpGame());
    }

    private void OnPause()
    {
      if (!PlayerPrefs.HasKey(SettingKeys.FirstTime))
      {
        return;
      }

      if (!playerModel.tutorialActive)
      {
        view.DisableAllInputs();
      }
      else
      {
        view.DisableInputsTutorial();
      }
    }

    private void OnContinue()
    {
      if (!playerModel.isAlive)
      {
        return;
      }
      
      if (!playerModel.tutorialActive)
      {
        view.EnableAllInputs();
      }
      else if (playerModel.tutorialActive && (PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) == 5 || PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) == 7))
      {
        view.DisableInputsTutorial();
      }
      else
      {
        view.DisableAllInputs();
      }
    }

    private void OnJumpAction()
    {
      if (!playerModel.isAlive)
      {
        return;
      }

      CheckTutorialStep(0);
      
      dispatcher.Dispatch(PlayerEvent.Jump);
      float posY = view.rectTransform.anchoredPosition.y;

      Tween jump = view.rectTransform.DOAnchorPosY(GameMechanicSettings.JumpHeight, GameMechanicSettings.JumpSpeed)
        .SetEase(Ease.OutSine)
        .SetSpeedBased()
        .OnComplete(() =>
        {
          view.rectTransform.DOAnchorPosY(posY, GameMechanicSettings.JumpSpeed)
            .SetSpeedBased()
            .SetEase(Ease.InQuad).OnComplete((() =>
            {
              dispatcher.Dispatch(PlayerEvent.JumpFinished);
            }));
        });

      jump.Play();
    }
    
    private void OnCrouchAction()
    {
      if (playerModel.isCrouching)
      {
        return;
      }

      CheckTutorialStep(1);
      
      dispatcher.Dispatch(PlayerEvent.Crouch);

      playerModel.isCrouching = true;
      view.SetColliders(true);
      StartCoroutine(CrouchRoutine());
    }
    
    private IEnumerator CrouchRoutine()
    {
      yield return new WaitForSeconds(GameMechanicSettings.CrouchDuration);
      
      view.CrouchFinished();
      playerModel.isCrouching = false;
    }
    
    private void OnCrouchFinished()
    {
      dispatcher.Dispatch(PlayerEvent.CrouchFinished);
    }
    
    private void OnReturnNormalSpeed()
    {
      speedModel.ReturnNormalSpeed();

      if (!playerModel.tutorialActive || PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) != 6) return;
      
      speedModel.Pause();
      dispatcher.Dispatch(GameEvent.TutorialStepStart);
    }
    
    private void OnReturnNormalSpeedTutorial()
    {
      PlayerPrefs.SetInt(SettingKeys.CompletedTutorialSteps, PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) + 1);
        
      speedModel.ReturnNormalSpeed();
      
      speedModel.Pause();
      dispatcher.Dispatch(GameEvent.TutorialStepStart); 
    }
    
    private void OnSpeedTutorial()
    {
      PlayerPrefs.SetInt(SettingKeys.CompletedTutorialSteps, PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) + 1);
        
      speedModel.ReturnNormalSpeed();
      
      speedModel.Pause();
      dispatcher.Dispatch(GameEvent.TutorialStepStart); 
    }

    private void OnDashAction()
    {
      if (playerModel.isDashing)
      {
        return;
      }

      if (playerModel.remainingTime < GameMechanicSettings.DashCost + 1)
      {
        dispatcher.Dispatch(PlayerEvent.NotEnoughSeconds);
        return;
      }

      CheckTutorialStep(3);

      view.isDashReady = false;
      playerModel.ChangeRemainingTime(-GameMechanicSettings.DashCost);
      StartCoroutine(DashTimer());
    }

    private IEnumerator DashTimer()
    {
      playerModel.isDashing = true;
      dispatcher.Dispatch(PlayerEvent.DashStarted);
      view.ChangeColor(new Color(0.4352942f, 1f, 1f, 0.75f));
      view.dashParticle.SetActive(true);
      playerModel.ChangeGameSpeed(GameMechanicSettings.DashSpeed);
      yield return new WaitForSeconds(GameMechanicSettings.DashDuration);
      playerModel.ChangeGameSpeed(-GameMechanicSettings.DashSpeed);
      playerModel.isDashing = false;
      view.ChangeColor(Color.white);
      view.dashParticle.SetActive(false);
      dispatcher.Dispatch(PlayerEvent.DashFinished);
      StartCoroutine(view.DashCooldown());
    }

    private void OnFireBulletAction()
    {
      if (playerModel.remainingTime < GameMechanicSettings.FireCost + 1)
      {
        dispatcher.Dispatch(PlayerEvent.NotEnoughSeconds);
        return;
      }

      CheckTutorialStep(2);
      
      view.isFireReady = false;

      playerModel.ChangeRemainingTime(-GameMechanicSettings.FireCost);
      dispatcher.Dispatch(PlayerEvent.FireBullet, view.gameObject.transform);
      StartCoroutine(view.FireCooldown());
    }
    
    private void OnCrashObstacle()
    {
      view.playerAnimator.SetTrigger("Hurt");
    }

    private void OnSlowDownTimeAction()
    {
      CheckTutorialStep(4);
      
      speedModel.SlowDownTime();
    }

    private void OnSpeedUpTimeAction()
    { 
      CheckTutorialStep(6);
      
      speedModel.SpeedUpTime();
    }
    
    private void  OnCollectDash()
    {
      StartCoroutine(CollectDashRoutine());
    }

    private IEnumerator CollectDashRoutine()
    {
      if (playerModel.isDashing)
      {
        yield return new WaitUntil(() => !playerModel.isDashing);
      }

      view.isDashReady = false;
      StartCoroutine(CollectedDashTimer());
    }
    
    private IEnumerator CollectedDashTimer()
    {
      playerModel.isDashing = true;
      view.ChangeColor(new Color(0.4352942f, 1f, 1f, 0.75f));
      view.dashParticle.SetActive(true);
      playerModel.ChangeGameSpeed(GameMechanicSettings.DashSpeed);
      yield return new WaitForSeconds(GameMechanicSettings.DashDuration);
      playerModel.ChangeGameSpeed(-GameMechanicSettings.DashSpeed);
      playerModel.isDashing = false;
      view.ChangeColor(Color.white);
      view.dashParticle.SetActive(false);
      dispatcher.Dispatch(PlayerEvent.CollectedDashComplete);
      view.isDashReady = true;
    }

    private void CheckTutorialStep(int stepIndex)
    {
      if (!playerModel.tutorialActive || PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) != stepIndex) return;
      
      PlayerPrefs.SetInt(SettingKeys.CompletedTutorialSteps, stepIndex + 1);
      speedModel.Continue();
      dispatcher.Dispatch(GameEvent.TutorialStepComplete);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(PlayerControllerEvents.FireBulletAction, OnFireBulletAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.Dash, OnDashAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.SlowDownTime, OnSlowDownTimeAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.SpeedUpTime, OnSpeedUpTimeAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.ReturnNormalSpeed, OnReturnNormalSpeed);
      view.dispatcher.RemoveListener(PlayerControllerEvents.ReturnNormalSpeedTutorial, OnReturnNormalSpeedTutorial);
      view.dispatcher.RemoveListener(PlayerControllerEvents.Jump, OnJumpAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.Crouch, OnCrouchAction);
      view.dispatcher.RemoveListener(PlayerControllerEvents.CrouchFinished, OnCrouchFinished);

      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);
      dispatcher.RemoveListener(GameEvent.Pause, OnPause);
      dispatcher.RemoveListener(GameEvent.Continue, OnContinue);
      dispatcher.RemoveListener(PlayerEvent.CrashObstacle, OnCrashObstacle);
      dispatcher.RemoveListener(PlayerEvent.CollectDash, OnCollectDash);
      dispatcher.RemoveListener(GameEvent.SpeedTutorial, OnSpeedTutorial);
    }
  }
}
