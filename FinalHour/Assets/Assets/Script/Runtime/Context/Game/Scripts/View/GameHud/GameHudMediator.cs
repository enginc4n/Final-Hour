using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.GameHud
{
  public class GameHudMediator : EventMediator
  {
    [Inject]
    public GameHudView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(GameEvent.SlowDown, CountTime);
      dispatcher.AddListener(GameEvent.SpeedUp, CountTime);
      dispatcher.AddListener(GameEvent.ReturnNormalSpeed, CountTime);
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

    public override void OnRemove()
    {
      dispatcher.RemoveListener(GameEvent.SlowDown, CountTime);
      dispatcher.RemoveListener(GameEvent.SpeedUp, CountTime);
      dispatcher.RemoveListener(GameEvent.ReturnNormalSpeed, CountTime);
    }
  }
}
