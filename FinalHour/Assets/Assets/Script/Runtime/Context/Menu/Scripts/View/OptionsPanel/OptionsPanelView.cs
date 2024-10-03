using Assets.Script.Runtime.Context.Menu.Scripts.View.OptionsPanel;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.SettingsPanel
{
  public class OptionsPanelView : EventView
  {
    public void OnClose()
    {
      dispatcher.Dispatch(OptionsPanelEvent.Close);
    }
    
    public void OnInstructions()
    {
      dispatcher.Dispatch(OptionsPanelEvent.Instructions);
    }
    
    public void OnSettings()
    {
      dispatcher.Dispatch(OptionsPanelEvent.Settings);
    }
    
    public void OnControls()
    {
      dispatcher.Dispatch(OptionsPanelEvent.Controls);
    }
    
    public void OnCredits()
    {
      dispatcher.Dispatch(OptionsPanelEvent.Credits);
    }

    public void OnExit()
    {
      dispatcher.Dispatch(OptionsPanelEvent.Exit);
    }
  }
}