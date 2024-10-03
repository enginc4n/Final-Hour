using Assets.Script.Runtime.Context.Menu.Scripts.View.MenuController;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.InstructionsPanel
{
  public class InstructionsPanelView : EventView
  {
    public void OnClose()
    {
      dispatcher.Dispatch(InstructionsPanelEvent.Close);
    }

    public void OnControls()
    {
      dispatcher.Dispatch(InstructionsPanelEvent.Controls);
    }
  }
}