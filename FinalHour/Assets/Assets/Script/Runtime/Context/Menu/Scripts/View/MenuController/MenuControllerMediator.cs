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
    }

    private void OnSoundSettings()
    {
      dispatcher.Dispatch(GameEvent.SoundSettingsPanel, transform);
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
      dispatcher.Dispatch(GameEvent.SettingsPanel, transform);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(MenuControllerEvent.Press, OnPress);
      view.dispatcher.RemoveListener(MenuControllerEvent.Settings, OnSettings);
      view.dispatcher.RemoveListener(MenuControllerEvent.SoundSettings, OnSoundSettings);
    }
  }
}
