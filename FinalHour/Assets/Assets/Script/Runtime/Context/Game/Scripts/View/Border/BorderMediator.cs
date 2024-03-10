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
      dispatcher.AddListener(PlayerEvent.SlowDown, UpdateBorder);
      dispatcher.AddListener(PlayerEvent.SpeedUp, UpdateBorder);
      dispatcher.AddListener(PlayerEvent.ReturnNormalSpeed, UpdateBorder);
      dispatcher.AddListener(PlayerEvent.Died, OnDied);
      dispatcher.AddListener(PlayerEvent.Play, OnInitialize);
    }

    public override void OnInitialize()
    {
      view.SetState(true);
      UpdateBorder();
    }

    private void UpdateBorder()
    {
      view.SetBorder(playerModel.speedState);
    }

    private void OnDied()
    {
      view.SetState(false);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(PlayerEvent.SlowDown, UpdateBorder);
      dispatcher.RemoveListener(PlayerEvent.SpeedUp, UpdateBorder);
      dispatcher.RemoveListener(PlayerEvent.ReturnNormalSpeed, UpdateBorder);
      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);
      dispatcher.RemoveListener(PlayerEvent.Play, OnInitialize);
    }
  }
}