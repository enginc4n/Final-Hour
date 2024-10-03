using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.InstructionsPanel
{
  public enum InstructionsPanelEvent
  { 
    Controls,
    Close
  }
  public class InstructionsPanelMediator : EventMediator
  {
    [Inject]
    public InstructionsPanelView view { get; set; }
   
    [Inject]
    public IUIModel uiModel { get; set; }

    public override void OnRegister()
    { 
      view.dispatcher.AddListener(InstructionsPanelEvent.Close, OnClose);
      view.dispatcher.AddListener(InstructionsPanelEvent.Controls, OnControls);
    }
    
    private void OnClose()
    { 
      uiModel.ClosePanel(PanelKeys.InstructionsPanel);
    }
    
    private void OnControls()
    { 
      dispatcher.Dispatch(GameEvent.ControlsPanel);
    }
    
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(InstructionsPanelEvent.Close, OnClose);
      view.dispatcher.RemoveListener(InstructionsPanelEvent.Controls, OnControls);
    }
  }
}