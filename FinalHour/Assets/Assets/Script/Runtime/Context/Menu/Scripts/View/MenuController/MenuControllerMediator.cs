using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.MenuController
{
  public enum MenuControllerEvent
  {
    Press,
    Settings,
    SoundSettings
  }

  public class MenuControllerMediator : EventMediator
  {
    [Inject]
    public MenuControllerView view { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(MenuControllerEvent.Press, OnPress);
      view.dispatcher.AddListener(MenuControllerEvent.Settings, OnSettings);
      view.dispatcher.AddListener(MenuControllerEvent.SoundSettings, OnSoundSettings);
      
      dispatcher.AddListener(GameEvent.Continue, OnContinue);
    }

    private void OnSoundSettings()
    {
      dispatcher.Dispatch(GameEvent.OptionsPanel, transform);
    }

    public override void OnInitialize()
    {
      dispatcher.Dispatch(GameEvent.Menu);
    }

    private void OnPress()
    {
      dispatcher.Dispatch(GameEvent.Start);
    }

    public void OnSettings()
    {
      view.shadow.SetActive(true);
      dispatcher.Dispatch(GameEvent.OptionsPanel, transform);
    }
    
    public void OnContinue()
    { 
      view.shadow.SetActive(false);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(MenuControllerEvent.Press, OnPress);
      view.dispatcher.RemoveListener(MenuControllerEvent.Settings, OnSettings);
      view.dispatcher.RemoveListener(MenuControllerEvent.SoundSettings, OnSoundSettings);
      
      dispatcher.RemoveListener(GameEvent.Continue, OnContinue);
    }
  }
}
