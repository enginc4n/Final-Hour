using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Game.Scripts.View.GameHud;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.Border
{
  public class BorderMediator : EventMediator
  {
    [Inject]
    public BorderView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(GameEvent.SlowDown, UpdateBorder);
      dispatcher.AddListener(GameEvent.SpeedUp, UpdateBorder);
      dispatcher.AddListener(GameEvent.ReturnNormalSpeed, UpdateBorder);
    }

    public override void OnInitialize()
    {
      UpdateBorder();
    }

    private void UpdateBorder()
    {
      view.SetBorder(playerModel.speedState);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(GameEvent.SlowDown, UpdateBorder);
      dispatcher.RemoveListener(GameEvent.SpeedUp, UpdateBorder);
      dispatcher.RemoveListener(GameEvent.ReturnNormalSpeed, UpdateBorder);
    }
  }
}
