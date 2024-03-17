using System;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using LootLocker;
using LootLocker.Requests;
using Scripts.Runtime.Modules.Core.PromiseTool;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.Model
{
  public class PlayerModel : IPlayerModel
  {
    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }

    public bool isAlive { get; set; }
    public float currentGameSpeed { get; set; }
    public int score { get; set; }
    public float position { get; set; }
    public bool isDashing { get; set; }
    public float remainingTime { get; set; }

    public string playerName { get; set; }
    public string playerId { get; set; }

    [PostConstruct]
    public void OnPostConstruct()
    {
      currentGameSpeed = GameControlSettings.StartingGameSpeed;
      score = 0;
      remainingTime = GameControlSettings.startingTime;

      isAlive = true;
    }

    public void ChangeRemainingTime(float value)
    {
      remainingTime += value;

      dispatcher.Dispatch(PlayerEvent.UpdateRemainingTime);
    }

    public void ChangeScore(int value)
    {
      if (!isAlive)
      {
        return;
      }

      score += value;
    }

    public void Die()
    {
      isAlive = false;
      dispatcher.Dispatch(PlayerEvent.Died);
      dispatcher.Dispatch(SoundEvent.DeathSound);
    }

    public IPromise LoginRoutine()
    {
      Promise promise = new();
      LootLockerSDKManager.StartGuestSession((response) =>
      {
        if (response.success)
        {
          playerId = response.player_id.ToString();
          PlayerPrefs.SetString(PlayerPrefSettings.PlayerId, playerId);
          PlayerPrefs.SetString(PlayerPrefSettings.PlayerName, playerName);
          promise.Resolve();
        }
        else
        {
          Exception ex = new("Could not start session");
          promise.Reject(ex);
        }
      });
      return promise;
    }

    public IPromise SetPlayerName()
    {
      Promise promise = new();
      LootLockerSDKManager.SetPlayerName(playerName, (response) =>
      {
        if (response.success)
        {
          promise.Resolve();
        }
        else
        {
          LootLockerErrorData lootLockerErrorData = response.errorData;
          Exception ex = new("Can not set player name");
          promise.Reject(ex);
        }
      });
      return promise;
    }
  }
}
