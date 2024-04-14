using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.SoundSettingsPanel
{
  public enum SoundSettingsEvents
  {
    ToogleSFX,
    ToogleMusic,
    MusicVolume,
    SFXVolume,
    Close,
    Exit
  }

  public class SoundSettingsMediator : EventMediator
  {
    [Inject]
    public SoundSettingsView view { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(SoundSettingsEvents.ToogleSFX, OnToogleSFX);
      view.dispatcher.AddListener(SoundSettingsEvents.ToogleMusic, OnToogleMusic);
      view.dispatcher.AddListener(SoundSettingsEvents.Close, OnClose);
      view.dispatcher.AddListener(SoundSettingsEvents.Exit, OnExit);
    }

    private void OnClose()
    {
      dispatcher.Dispatch(GameEvent.SoundSettingsPanel);
    }

    private void OnExit()
    {
      dispatcher.Dispatch(GameEvent.Exit);
    }

    private void OnToogleSFX()
    {
      throw new System.NotImplementedException();
    }

    private void OnToogleMusic()
    {
      throw new System.NotImplementedException();
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(SoundSettingsEvents.ToogleSFX, OnToogleSFX);
      view.dispatcher.RemoveListener(SoundSettingsEvents.ToogleMusic, OnToogleMusic);
      view.dispatcher.RemoveListener(SoundSettingsEvents.Close, OnClose);
      view.dispatcher.RemoveListener(SoundSettingsEvents.Exit, OnExit);
    }
  }
}
