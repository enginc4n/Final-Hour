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

    private void Update()
    {
      view.ParallaxEffect(playerModel.currentPlayerSpeed * 0.5f);
      DecrementTime();
    }

    private void DecrementTime()
    {
      playerModel.remainingTime -= view.decrementTimeAmount;
    }
  }
}
