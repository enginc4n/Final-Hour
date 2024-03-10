using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.DeadPanel
{
  public enum DeadPanelEvent
  {
    PlayAgain
  }
  public class DeadPanelMediator : EventMediator
  {
    [Inject]
    public DeadPanelView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(DeadPanelEvent.PlayAgain, OnPlayAgain);
      
      dispatcher.AddListener(PlayerEvent.Play, OnInitialize);
      dispatcher.AddListener(PlayerEvent.Died, OnDied);
    }

    public override void OnInitialize()
    {
      view.SetState(false);
    }

    private void OnDied()
    {
      view.score = playerModel.score;
      view.SetState(true);
    }

    private void OnPlayAgain()
    {
      playerModel.Respawn();
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(DeadPanelEvent.PlayAgain, OnPlayAgain);
      
      dispatcher.RemoveListener(PlayerEvent.Play, OnInitialize);
      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);
    }
  }
}