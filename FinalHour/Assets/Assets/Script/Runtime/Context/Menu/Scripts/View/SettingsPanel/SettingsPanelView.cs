using Assets.Script.Runtime.Context.Menu.Scripts.View.MenuController;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.SettingsPanel
{
  public class SettingsPanelView : EventView
  {
    public void OnClose()
    {
      dispatcher.Dispatch(SettingsPanelEvent.Close);
    }

    public void OnExit()
    {
      dispatcher.Dispatch(SettingsPanelEvent.Exit);
    }
  }
}