using System.Collections;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
using DG.Tweening;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.GameHud
{
  public enum GameHudEvent
  {
    Settings
  }

  public class GameHudMediator : EventMediator
  {
    [Inject]
    public GameHudView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }

    [Inject]
    public IEnemyModel enemyModel { get; set; }

    [Inject]
    public ISpeedModel speedModel { get; set; }
    
    [Inject]
    public IAudioModel audioModel { get; set; }
    
    [Inject]
    public IUIModel uiModel { get; set; }

    private Coroutine _shadowLoop;

    private Coroutine _timeLoop;

    private Coroutine _warningRoutine;

    private bool _hasSlowedDown;

    public override void OnRegister()
    {
      view.dispatcher.AddListener(GameHudEvent.Settings, OnSettings);

      dispatcher.AddListener(PlayerEvent.EnemyStartedMoving, StartShadowLoop);
      dispatcher.AddListener(PlayerEvent.EnemyStoppedMoving, StopShadowLoop);
      dispatcher.AddListener(PlayerEvent.Died, OnDied);
      dispatcher.AddListener(PlayerEvent.SlowDown, OnSlowDown);
      dispatcher.AddListener(PlayerEvent.SpeedUp, OnChangeSpeed);
      dispatcher.AddListener(PlayerEvent.ReturnNormalSpeed, OnChangeSpeed);
      dispatcher.AddListener(PlayerEvent.DashStarted, OnDashStarted);
      dispatcher.AddListener(PlayerEvent.DashFinished, OnDashFinished);
      dispatcher.AddListener(PlayerEvent.FireBullet, OnFire);
      dispatcher.AddListener(PlayerEvent.Collect, OnCollect);
      dispatcher.AddListener(PlayerEvent.FlyingObstacleIncoming, OnFlyingObstacleIncoming);
      dispatcher.AddListener(PlayerEvent.CollectDash, OnCollectDash);
      dispatcher.AddListener(PlayerEvent.CollectedDashComplete, OnCollectedDashComplete);
      dispatcher.AddListener(PlayerEvent.Jump, OnJump);
      dispatcher.AddListener(PlayerEvent.JumpFinished, OnJumpFinished);
      dispatcher.AddListener(PlayerEvent.Crouch, OnCrouch);
      dispatcher.AddListener(PlayerEvent.CrouchFinished, OnCrouchFinished);
      dispatcher.AddListener(PlayerEvent.NotEnoughSeconds, OnOutOfSeconds);

      dispatcher.AddListener(GameEvent.Pause, OnPause);
      dispatcher.AddListener(GameEvent.Continue, OnContinue);
      dispatcher.AddListener(GameEvent.Start, SetLayout);
    }

    public override void OnInitialize()
    {
      if (!PlayerPrefs.HasKey(SettingKeys.FirstTime))
      {
        speedModel.Pause();
        uiModel.OpenPanel(PanelKeys.InstructionsPanel, transform.parent);
        
        PlayerPrefs.SetInt(SettingKeys.FirstTime, 0);
      }
      
      view.deviceType = DeviceType.Handheld;
      view.SetState(true);
      view.SetShadowOpacity(0);
      view.SetIcon(speedModel.speedState);
      view.UpdateScore(playerModel.score);
      CountTime();

      SetLayout();
    }

    private void SetLayout()
    {
      if (view.deviceType == DeviceType.Handheld)
      {
        view.pcHud.SetActive(false);

        Canvas.ForceUpdateCanvases();
        view.mobilePad.SetActive(false);
        view.mobilePad.SetActive(true);
        Canvas.ForceUpdateCanvases();
      }
      else
      {
        view.pcHud.SetActive(true);
        view.mobilePad.SetActive(false);
      }
    }
    
    private void FixedUpdate()
    {
      int minutes = Mathf.FloorToInt(playerModel.remainingTime / 60f);
      int seconds = Mathf.FloorToInt(playerModel.remainingTime % 60f);
        
      view.timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
      
      view.timerText.color = playerModel.remainingTime is <= 10 and > 1 ? new Color(1f, 0.2290596f, 0.1650943f) : Color.white;
    }

    private void CountTime()
    {
      _timeLoop = StartCoroutine(DecreaseRemainingTime());
      StartCoroutine(ScoreRoutine());
    }

    private IEnumerator DecreaseRemainingTime()
    {
      while (playerModel.remainingTime > 0 && playerModel.isAlive)
      {
        yield return new WaitForSeconds(1f);

        playerModel.ChangeRemainingTime(-1f);
        
        if (playerModel.remainingTime is <= 10 and > 1)
        {
          view.timerText.transform.DOScale(1.2f, 0.25f).OnComplete((() =>
          {
            view.timerText.transform.DOScale(1f, 0.25f);
            dispatcher.Dispatch(GameEvent.ClockTick);
          }));
        }
      }
    }

    private IEnumerator IncreaseRemainingTime()
    {
      while (playerModel.remainingTime > 0 && playerModel.isAlive)
      {
        yield return new WaitForSecondsRealtime(1 / GameMechanicSettings.SlowTimeGain);

        playerModel.ChangeRemainingTime(+1f);
      }
    }
    
    private IEnumerator ScoreRoutine()
    {
      while (playerModel.remainingTime > 0 && playerModel.isAlive)
      {
        yield return new WaitForSeconds(1f);

        playerModel.ChangeScore(1);
        view.UpdateScore(playerModel.score);
      }
    }

    private void OnDashFinished()
    {
      view.dashImage.color = Color.white;
      view.dashText.color = Color.white;
      view.StartDashTimer();
    }
    
    private void OnDashStarted()
    {
      if (view.deviceType == DeviceType.Handheld)
      {
        view.dashButton.interactable = false;
      }
      
      view.FlyText(-GameMechanicSettings.DashCost);
      
      view.dashImage.color = new Color(1f, 1f, 1f, 0.5f);
      view.dashText.color = new Color(1f, 1f, 1f, 0.5f);
    }

    private void OnFire()
    {
      view.FlyText(-GameMechanicSettings.FireCost);
      
      view.StartFireTimer();
    }
    
    private void OnCollect()
    {
      view.FlyText(GameMechanicSettings.CollectibleTimeAmount);
    }
    
    private void OnChangeSpeed()
    {
      if (_hasSlowedDown)
      {
        StopCoroutine(_timeLoop);
        _hasSlowedDown = false;

        if (playerModel.isAlive)
        {
          _timeLoop = StartCoroutine(DecreaseRemainingTime());
        }
      }

      view.SetIcon(speedModel.speedState);
    }

    private void OnSlowDown()
    {
      _hasSlowedDown = true;

      view.SetIcon(speedModel.speedState);

      StopCoroutine(_timeLoop);
      _timeLoop = StartCoroutine(IncreaseRemainingTime());
    }

    private void StartShadowLoop()
    {
       _shadowLoop ??= StartCoroutine(ShadowLoop());
    }

    private void StopShadowLoop()
    {
      if (_shadowLoop == null)
      {
        return;
      }

      StopCoroutine(_shadowLoop);
      _shadowLoop = null;
    }

    private IEnumerator ShadowLoop()
    {
      while (true)
      {
        UpdateShadow();

        yield return null;
      }
    }

    private void UpdateShadow()
    {
      float threshold = 0.35f;
      float spawnDistance = Mathf.Abs(playerModel.position - enemyModel.spawnPosition);
      float currentDistance = Mathf.Abs(playerModel.position - enemyModel.currentPosition);

      if (currentDistance >= spawnDistance)
      {
        view.SetShadowOpacity(0); 
        audioModel.ResetPitchVolume();
      }
      else
      {
        float a = 1 - currentDistance / spawnDistance;
        view.SetShadowOpacity(a);
        if (a >=threshold)
        {
          audioModel.SetPitchVolumeRelative(1-a,1-a);
        }
        else
        {
          audioModel.ResetPitchVolume();
        }
      }
    }

    private void OnDied()
    {
      view.SetState(false);
      
      StopCoroutine(ScoreRoutine());
      StopShadowLoop();
      
      if (_timeLoop != null)
      {
        StopCoroutine(_timeLoop);
      }

      if (_warningRoutine != null)
      {
        StopCoroutine(_warningRoutine);
      }
    }

    private void OnSettings()
    {
      dispatcher.Dispatch(GameEvent.OptionsPanel, transform);
    }
    
    private void OnFlyingObstacleIncoming()
    {
      if (_warningRoutine != null)
      {
        StopCoroutine(_warningRoutine);
      }

      _warningRoutine = StartCoroutine(ObstacleWarningRoutine());
      StartCoroutine(ObstacleWarningRoutine());
    }

    private IEnumerator ObstacleWarningRoutine()
    {
      view.flyingObstacleWarning.SetActive(true);
      yield return new WaitForSeconds(GameMechanicSettings.FlyingObstacleWarningTime);
      view.flyingObstacleWarning.SetActive(false);
    }
    
    private void OnCollectDash()
    {
      view.dashImage.color = new Color(1f, 1f, 1f, 0.5f);
      view.dashText.color = new Color(1f, 1f, 1f, 0.5f);
    }
    
    private void OnCollectedDashComplete()
    {
      view.dashImage.color = Color.white;
      view.dashText.color = Color.white;
    }
    
    private void OnJump()
    {
      if (view.deviceType == DeviceType.Handheld)
      {
        view.jumpButton.interactable = false;
      }
    }
    
    private void OnJumpFinished()
    {
      if (view.deviceType == DeviceType.Handheld)
      {
        view.jumpButton.interactable = true;
      }
    }
    
    private void OnCrouch()
    {
      if (view.deviceType == DeviceType.Handheld)
      {
        view.crouchButton.interactable = false;
      }
    }
    
    private void OnCrouchFinished()
    {
      if (view.deviceType == DeviceType.Handheld)
      {
        view.crouchButton.interactable = true;
      }
    }
    
    private void OnOutOfSeconds()
    {
      view.OutOfSeconds();
    }
    
    private void OnPause()
    { 
      view.raycastGo.SetActive(true);
    }
    
    private void OnContinue()
    {
      view.raycastGo.SetActive(false);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(GameHudEvent.Settings, OnSettings);

      dispatcher.RemoveListener(PlayerEvent.EnemyStartedMoving, StartShadowLoop);
      dispatcher.RemoveListener(PlayerEvent.EnemyStoppedMoving, StopShadowLoop);
      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);
      dispatcher.RemoveListener(PlayerEvent.SlowDown, OnChangeSpeed);
      dispatcher.RemoveListener(PlayerEvent.SpeedUp, OnChangeSpeed);
      dispatcher.RemoveListener(PlayerEvent.ReturnNormalSpeed, OnChangeSpeed);
      dispatcher.RemoveListener(PlayerEvent.DashStarted, OnDashStarted);
      dispatcher.RemoveListener(PlayerEvent.DashFinished, OnDashFinished);
      dispatcher.RemoveListener(PlayerEvent.FireBullet, OnFire);
      dispatcher.RemoveListener(PlayerEvent.Collect, OnCollect);
      dispatcher.RemoveListener(PlayerEvent.FlyingObstacleIncoming, OnFlyingObstacleIncoming);
      dispatcher.RemoveListener(PlayerEvent.CollectDash, OnCollectDash);
      dispatcher.RemoveListener(PlayerEvent.CollectedDashComplete, OnCollectedDashComplete);
      dispatcher.RemoveListener(PlayerEvent.Jump, OnJump);
      dispatcher.RemoveListener(PlayerEvent.JumpFinished, OnJumpFinished);
      dispatcher.RemoveListener(PlayerEvent.Crouch, OnCrouch);
      dispatcher.RemoveListener(PlayerEvent.CrouchFinished, OnCrouchFinished);
      dispatcher.RemoveListener(PlayerEvent.NotEnoughSeconds, OnOutOfSeconds);

      dispatcher.RemoveListener(GameEvent.Pause, OnPause);
      dispatcher.RemoveListener(GameEvent.Continue, OnContinue);
      dispatcher.RemoveListener(GameEvent.Start, SetLayout);
    }
  }
}
