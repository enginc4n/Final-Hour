using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.ControlsPanel
{
  public class ControlsPanelView : EventView
  {
    public GameObject pcControls;

    public GameObject mobileControls;
    
    public void OnClose()
    {
      dispatcher.Dispatch(ControlsPanelEvent.Close);
    }
  }
}