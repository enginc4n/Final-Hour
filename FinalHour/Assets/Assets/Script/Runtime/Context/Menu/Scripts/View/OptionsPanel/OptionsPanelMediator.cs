using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.View.SettingsPanel;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.OptionsPanel
{
  public enum OptionsPanelEvent
  {
    Close,
    Exit,
    Instructions,
    Settings,
    Credits
  }
  public class OptionsPanelMediator : EventMediator
  {
    [Inject]
    public OptionsPanelView view { get; set; }
    
    [Inject]
    public IUIModel uiModel { get; set; }
    
    public override void OnRegister()
    { 
      view.dispatcher.AddListener(OptionsPanelEvent.Close, OnClose);
      view.dispatcher.AddListener(OptionsPanelEvent.Exit, OnExit);
      view.dispatcher.AddListener(OptionsPanelEvent.Instructions, OnInstructions);
      view.dispatcher.AddListener(OptionsPanelEvent.Settings, OnSettings);
      view.dispatcher.AddListener(OptionsPanelEvent.Credits, OnCredits);
    }
    
    private void OnClose()
    { 
      dispatcher.Dispatch(GameEvent.OptionsPanel);
    }
    
    private void OnExit()
    { 
      dispatcher.Dispatch(GameEvent.Exit);
    }
    
    private void OnInstructions()
    { 
      uiModel.OpenPanel(PanelKeys.InstructionsPanel, transform.parent);
    }
    
    private void OnSettings()
    { 
      uiModel.OpenPanel(PanelKeys.SettingsPanel, transform.parent);
    }
    
    private void OnCredits()
    { 
      uiModel.OpenPanel(PanelKeys.CreditsPanel, transform.parent);
    }
    
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(OptionsPanelEvent.Close, OnClose);
      view.dispatcher.RemoveListener(OptionsPanelEvent.Exit, OnExit);
      view.dispatcher.RemoveListener(OptionsPanelEvent.Instructions, OnInstructions);
      view.dispatcher.RemoveListener(OptionsPanelEvent.Settings, OnSettings);
      view.dispatcher.RemoveListener(OptionsPanelEvent.Credits, OnCredits);
    }
  }
}