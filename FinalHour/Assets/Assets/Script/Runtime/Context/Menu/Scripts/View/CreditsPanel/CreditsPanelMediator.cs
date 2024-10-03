using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.CreditsPanel
{
  public enum CreditsPanelEvent
  {
    Close
  }
  public class CreditsPanelMediator : EventMediator
  {
    [Inject]
    public CreditsPanelView view { get; set; }
   
    [Inject]
    public IUIModel uiModel { get; set; }

    public override void OnRegister()
    { 
      view.dispatcher.AddListener(CreditsPanelEvent.Close, OnClose);
    }
    
    private void OnClose()
    { 
      uiModel.ClosePanel(PanelKeys.CreditsPanel);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(CreditsPanelEvent.Close, OnClose);
    }
  }
}