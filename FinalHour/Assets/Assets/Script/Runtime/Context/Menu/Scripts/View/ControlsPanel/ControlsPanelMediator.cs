using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.View.CreditsPanel;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.ControlsPanel
{
  public enum ControlsPanelEvent
  {
    Close
  }
  public class ControlsPanelMediator : EventMediator
  {
    [Inject]
    public ControlsPanelView view { get; set; }
   
    [Inject]
    public IUIModel uiModel { get; set; }

    public override void OnRegister()
    { 
      view.dispatcher.AddListener(ControlsPanelEvent.Close, OnClose);
    }
    
    private void OnClose()
    { 
      uiModel.ClosePanel(PanelKeys.ControlsPanel);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(ControlsPanelEvent.Close, OnClose);
    }
  }
}