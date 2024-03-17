using Scripts.Runtime.Modules.Core.PromiseTool;

namespace Assets.Script.Runtime.Context.Game.Scripts.Model
{
  public interface ILeaderBoardModel
  {
    public string LeaderBoardKey { get; }
    public IPromise SubmitScoreRoutine(int scoreToUpdload);
  }
}
