using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.MenuController
{
  public enum MenuControllerEvent
  {
    Press,
    Settings
  }
  
  public class MenuControllerMediator : EventMediator
  {
    [Inject]
    public MenuControllerView view { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(MenuControllerEvent.Press, OnPress);
      view.dispatcher.AddListener(MenuControllerEvent.Settings, OnSettings);
    }
    
    private void OnPress()
    {
      dispatcher.Dispatch(GameEvent.Start);
    }

    public void OnSettings()
    {
      dispatcher.Dispatch(GameEvent.OpenSettings,transform);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(MenuControllerEvent.Press, OnPress);
      view.dispatcher.RemoveListener(MenuControllerEvent.Settings, OnSettings);
    }
  }
}