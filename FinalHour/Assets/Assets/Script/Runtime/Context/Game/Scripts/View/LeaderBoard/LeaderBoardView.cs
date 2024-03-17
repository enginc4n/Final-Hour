using System;
using LootLocker.Requests;
using Scripts.Runtime.Modules.Core.PromiseTool;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.LeaderBoard
{
  public class LeaderBoardView : EventView
  {
    public void FetchHighScore()
    {
      dispatcher.Dispatch(LeaderBoardEvent.FetchHighScore);
    }

    public IPromise FetchTopHighScoresRoutine(string leaderBoardKey)
    {
      Promise promise = new();
      LootLockerSDKManager.GetScoreList(leaderBoardKey, 10, 0, (response) =>
      {
        if (response.success)
        {
          string tempPlayerNames = "Names\n";
          string tempPlayerScores = "Scores\n";
          LootLockerLeaderboardMember[] members = response.items;

          foreach (LootLockerLeaderboardMember member in members)
          {
            tempPlayerNames += member.rank + ". ";
            if (member.player.name != string.Empty)
            {
              tempPlayerNames += member.player.name;
            }
            else
            {
              tempPlayerNames += member.player.id;
            }

            tempPlayerScores += member.score + "\n";
          }

//playerNames text i tempPlayerNames e eşit olacak
//playerScores text i tempPlayerScores a eşit olacak
          promise.Resolve();
        }
        else
        {
          Exception ex = new("Failed to fetch leaderboard");
          promise.Reject(ex);
        }
      });
      return promise;
    }
  }
}
