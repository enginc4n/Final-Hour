using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.ControlsPanel
{
  public class ControlsPanelView : EventView
  {
    public void OnClose()
    {
      dispatcher.Dispatch(ControlsPanelEvent.Close);
    }
  }
}