using System;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using DG.Tweening;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.Obstacles
{
  public enum ObstacleEvents
  {
    ObstacleIsBroken,
    CrashWithPlayer
  }

  public class ObstacleMediator : EventMediator
  {
    [Inject]
    public ObstacleView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }
    
    [Inject]
    public ISpeedModel speedModel { get; set; }

    private bool _keepMoving = false;

    public override void OnRegister()
    {
      view.dispatcher.AddListener(ObstacleEvents.CrashWithPlayer, OnCrashWithPlayer);
      view.dispatcher.AddListener(ObstacleEvents.ObstacleIsBroken, OnObstacleIsBroken);
      
      dispatcher.AddListener(PlayerEvent.Died, OnDied);
    }

    private void OnObstacleIsBroken()
    {
      view.InstantiateObject(view.breakParticle);
      if (view.isDropTime)
      {
        GameObject timeAdder = view.InstantiateObject(view.timeAdder);
        timeAdder.GetComponent<ObstacleView>().ownSpeedFactor = 1f;
      }
      
      Destroy(view.gameObject);
    }

    private void OnCrashWithPlayer()
    {
      if (!playerModel.isAlive)
      {
        return;
      }
      
      if (view.isCollectible)
      {
        CollectibleProcess();
      }
      else
      {
        if (!playerModel.isDashing)
        {
          CrashObstacleProcess();
        }
      }
    }

    private void CollectibleProcess()
    {
      if (CompareTag(ObstacleTag.Time))
      {
        playerModel.remainingTime += GameMechanicSettings.CollectibleTimeAmount;
        view.InstantiateObject(view.collectParticle);
        dispatcher.Dispatch(PlayerEvent.Collect);
        Destroy(view.gameObject);
      } else if (CompareTag(ObstacleTag.Dasher))
      {
        view.InstantiateObject(view.collectParticle);
        dispatcher.Dispatch(PlayerEvent.CollectDash);
        Destroy(view.gameObject);
      }
    }

    private void CrashObstacleProcess()
    {
      view.InstantiateObject(view.crushParticle);
      dispatcher.Dispatch(PlayerEvent.CrashObstacle);
      Destroy(view.gameObject);
    }

    private void FixedUpdate()
    {
      if (!playerModel.isAlive && !_keepMoving)
      {
        return;
      }

      transform.Translate(new Vector2(-playerModel.currentGameSpeed*view.ownSpeedFactor, 0), Space.Self);

      if ( PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) == view.tutorialIndex && view.tutorialIndex == 2 && view.rectTransform.anchoredPosition.x <= 1500)
      {
        speedModel.Pause();
        dispatcher.Dispatch(GameEvent.TutorialStepStart);
      }
      
      if ( PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) == view.tutorialIndex && view.rectTransform.anchoredPosition.x <= 800)
      {
        speedModel.Pause();
        dispatcher.Dispatch(GameEvent.TutorialStepStart);
      }
    }

    private void OnDied()
    {
      if (view.obstacleType is ObstacleType.Flying or ObstacleType.Collectible)
      {
        _keepMoving = true;
      }
    }
    
    public override void OnRemove()
    {
      if (view.activeRoutine != null)
      {
        StopCoroutine(view.activeRoutine);
      }

      view.rectTransform.DOKill();
      
      view.dispatcher.RemoveListener(ObstacleEvents.CrashWithPlayer, OnCrashWithPlayer);
      view.dispatcher.RemoveListener(ObstacleEvents.ObstacleIsBroken, OnObstacleIsBroken);
      
      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);
    }
  }
}
