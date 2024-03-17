using Assets.Script.Runtime.Context.Game.Scripts.Model;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.LeaderBoard
{
  public enum LeaderBoardEvent
  {
    FetchHighScore
  }

  public class LeaderBoardMediator : EventMediator
  {
    [Inject]
    public LeaderBoardView view { get; set; }

    [Inject]
    public ILeaderBoardModel leaderBoardModel { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(LeaderBoardEvent.FetchHighScore, OnFetchTopHighScores);
    }

    private void OnFetchTopHighScores()
    {
      view.FetchTopHighScoresRoutine(leaderBoardModel.LeaderBoardKey);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(LeaderBoardEvent.FetchHighScore, OnFetchTopHighScores);
    }
  }
}
