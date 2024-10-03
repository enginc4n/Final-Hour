using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.View.SettingsPanel;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.SoundSettingsPanel
{
  public enum SettingsEvents
  {
    ToggleSfx,
    ToggleMusic,
    MusicVolume,
    SfxVolume,
    Close
  }

  public class SettingsMediator : EventMediator
  {
    [Inject]
    public SettingsView view { get; set; }
    
    [Inject]
    public IAudioModel audioModel { get; set; }
    
    [Inject]
    public IUIModel uiModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(SettingsEvents.ToggleSfx, OnToggleSFX);
      view.dispatcher.AddListener(SettingsEvents.ToggleMusic, OnToggleMusic);
      view.dispatcher.AddListener(SettingsEvents.MusicVolume, OnMusicVolume);
      view.dispatcher.AddListener(SettingsEvents.SfxVolume, OnSfxVolume);
      view.dispatcher.AddListener(SettingsEvents.Close, OnClose);
    }

    public override void OnInitialize()
    {
      view.musicToggle.onValueChanged.RemoveAllListeners();
      view.sfxToggle.onValueChanged.RemoveAllListeners();
      view.musicSlider.onValueChanged.RemoveAllListeners();
      view.sfxSlider.onValueChanged.RemoveAllListeners();

      view.musicToggle.isOn = (PlayerPrefs.GetInt(SettingKeys.MusicOn) > 0);
      view.musicSlider.interactable = (PlayerPrefs.GetInt(SettingKeys.MusicOn) > 0);
      view.musicSlider.value = PlayerPrefs.GetFloat(SettingKeys.MusicVolume);
      
      view.sfxToggle.isOn = (PlayerPrefs.GetInt(SettingKeys.SfxOn) > 0);
      view.sfxSlider.interactable = (PlayerPrefs.GetInt(SettingKeys.SfxOn) > 0);
      view.sfxSlider.value = PlayerPrefs.GetFloat(SettingKeys.SfxVolume);

      view.musicToggle.onValueChanged.AddListener(view.ToggleMusic);
      view.sfxToggle.onValueChanged.AddListener(view.ToggleSfx);
      view.musicSlider.onValueChanged.AddListener(view.MusicVolume);
      view.sfxSlider.onValueChanged.AddListener(view.SfxVolume);
    }

    private void OnClose()
    {
      uiModel.ClosePanel(PanelKeys.SettingsPanel);
    }

    private void OnToggleSFX()
    {
      audioModel.ToggleSfx();
      OnInitialize();
    }

    private void OnToggleMusic()
    {
      audioModel.ToggleMusic();
      OnInitialize();
    }
    
    private void OnMusicVolume(IEvent payload)
    {
      float volume = (float)payload.data;
      audioModel.SetMusicVolume(volume);
    }

    private void OnSfxVolume(IEvent payload)
    {
      float volume = (float)payload.data;
      audioModel.SetSfxVolume(volume);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(SettingsEvents.ToggleSfx, OnToggleSFX);
      view.dispatcher.RemoveListener(SettingsEvents.ToggleMusic, OnToggleMusic);
      view.dispatcher.RemoveListener(SettingsEvents.MusicVolume, OnMusicVolume);
      view.dispatcher.RemoveListener(SettingsEvents.SfxVolume, OnSfxVolume);
      view.dispatcher.RemoveListener(SettingsEvents.Close, OnClose);
    }
  }
}
