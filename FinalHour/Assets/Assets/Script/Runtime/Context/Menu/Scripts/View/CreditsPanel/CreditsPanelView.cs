using Assets.Script.Runtime.Context.Menu.Scripts.View.InstructionsPanel;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.CreditsPanel
{
  public class CreditsPanelView : EventView
  {
    public void OnClose()
    {
      dispatcher.Dispatch(CreditsPanelEvent.Close);
    }
  }
}