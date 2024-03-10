using System.Collections;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.GameHud
{
  public class GameHudMediator : EventMediator
  {
    [Inject]
    public GameHudView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }
    
    [Inject]
    public IEnemyModel enemyModel { get; set; }
    
    private Coroutine _shadowLoop;

    public override void OnRegister()
    {
      dispatcher.AddListener(GameEvent.SlowDown, CountTime);
      dispatcher.AddListener(GameEvent.SpeedUp, CountTime);
      dispatcher.AddListener(GameEvent.ReturnNormalSpeed, CountTime);
      dispatcher.AddListener(GameEvent.EnemyStartedMoving, StartShadowLoop);
      dispatcher.AddListener(GameEvent.EnemyStoppedMoving, StopShadowLoop);
    }

    public override void OnInitialize()
    {
      view.UpdateTimer(playerModel.remainingTime);
      CountTime();
    }
    
    private void CountTime()
    {
      view.SetIcon(playerModel.speedState);
      CancelInvoke();
      InvokeRepeating("UpdateRemainingTime", playerModel.timerCountSpeed, playerModel.timerCountSpeed);
    }

    private void UpdateRemainingTime()
    {
      if (playerModel.remainingTime > 0)
      {
        playerModel.ChangeRemainingTime(-1f);
        view.UpdateTimer(playerModel.remainingTime);

        playerModel.ChangeScore(1);
        view.UpdateScore(playerModel.score);
      }
      else
      {
        playerModel.Die();
      }
    }
    
    private void StartShadowLoop()
    {
      _shadowLoop ??= StartCoroutine(ShadowLoop());
    }
    
    private void StopShadowLoop()
    {
      if (_shadowLoop == null) return;
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
      float spawnDistance = Mathf.Abs(playerModel.position - enemyModel.spawnPosition);
      float currentDistance = Mathf.Abs(playerModel.position - enemyModel.position);

      if (currentDistance > spawnDistance)
      {
        view.SetShadowOpacity(0);
      }
      else
      {
        float a = 1 - ((currentDistance / spawnDistance));
        view.SetShadowOpacity(a);
      }
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(GameEvent.SlowDown, CountTime);
      dispatcher.RemoveListener(GameEvent.SpeedUp, CountTime);
      dispatcher.RemoveListener(GameEvent.ReturnNormalSpeed, CountTime);
      dispatcher.RemoveListener(GameEvent.EnemyStartedMoving, StartShadowLoop);
      dispatcher.RemoveListener(GameEvent.EnemyStoppedMoving, StopShadowLoop);
    }
  }
}
