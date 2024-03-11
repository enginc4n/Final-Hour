using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.SettingsPanel
{
  public enum SettingsPanelEvent
  {
    Close,
    Exit
  }
  public class SettingsPanelMediator : EventMediator
  {
    [Inject]
    public SettingsPanelView view { get; set; }
    

    public override void OnRegister()
    { 
      view.dispatcher.AddListener(SettingsPanelEvent.Close, OnClose);
      view.dispatcher.AddListener(SettingsPanelEvent.Exit, OnExit);
    }
    
    private void OnClose()
    { 
      dispatcher.Dispatch(GameEvent.SettingsPanel);
    }
    
    private void OnExit()
    { 
      dispatcher.Dispatch(GameEvent.Exit);
    }
    
    public override void OnRemove()
    {
      view.dispatcher.AddListener(SettingsPanelEvent.Close, OnClose);
      view.dispatcher.RemoveListener(SettingsPanelEvent.Exit, OnExit);
    }
  }
}