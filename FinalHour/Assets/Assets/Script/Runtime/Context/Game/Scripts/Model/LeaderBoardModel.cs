using System;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using LootLocker.Requests;
using Scripts.Runtime.Modules.Core.PromiseTool;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.Model
{
  public class LeaderBoardModel : ILeaderBoardModel
  {
    public string LeaderBoardKey { get; set; }

    [PostConstruct]
    public void OnPostConstruct()
    {
      LeaderBoardKey = "globalHighScore";
    }

    public IPromise SubmitScoreRoutine(int scoreToUpdload)
    {
      Promise promise = new();
      string playerId = PlayerPrefs.GetString(PlayerPrefSettings.PlayerId);
      LootLockerSDKManager.SubmitScore(playerId, scoreToUpdload, LeaderBoardKey, (response) =>
      {
        if (response.success)
        {
          promise.Resolve();
        }
        else
        {
          Exception ex = new("Failed to upload score");
          promise.Reject(ex);
        }
      });
      return promise;
    }
  }
}
