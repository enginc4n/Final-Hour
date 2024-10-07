using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using strange.extensions.mediation.impl;
using UnityEngine.SceneManagement;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.DeadPanel
{
  public enum DeadPanelEvent
  {
    PlayAgain,
    Menu
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
      view.dispatcher.AddListener(DeadPanelEvent.Menu, OnMenu);

      dispatcher.AddListener(PlayerEvent.Died, OnDied);
    }

    public override void OnInitialize()
    {
      view.SetState(false);
    }

    private void OnDied()
    {
      view.score = playerModel.score;
      view.SetState(true, playerModel.remainingTime <= 0);
    }

    private void OnPlayAgain()
    {
      dispatcher.Dispatch(GameEvent.Start);
    }
    
    private void OnMenu()
    {
      SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(DeadPanelEvent.PlayAgain, OnPlayAgain);
      view.dispatcher.RemoveListener(DeadPanelEvent.Menu, OnMenu);
      
      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);
    }
  }
}