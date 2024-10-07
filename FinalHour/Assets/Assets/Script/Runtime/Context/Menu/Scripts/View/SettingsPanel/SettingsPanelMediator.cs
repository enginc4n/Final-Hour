using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.SettingsPanel
{
  public enum SettingsEvents
  {
    ToggleSfx,
    ToggleMusic,
    MusicVolume,
    SfxVolume,
    Close
  }

  public class SettingsPanelMediator : EventMediator
  {
    [Inject]
    public SettingsPanelView panelView { get; set; }
    
    [Inject]
    public IAudioModel audioModel { get; set; }
    
    [Inject]
    public IUIModel uiModel { get; set; }

    public override void OnRegister()
    {
      panelView.dispatcher.AddListener(SettingsEvents.ToggleSfx, OnToggleSFX);
      panelView.dispatcher.AddListener(SettingsEvents.ToggleMusic, OnToggleMusic);
      panelView.dispatcher.AddListener(SettingsEvents.MusicVolume, OnMusicVolume);
      panelView.dispatcher.AddListener(SettingsEvents.SfxVolume, OnSfxVolume);
      panelView.dispatcher.AddListener(SettingsEvents.Close, OnClose);
    }

    public override void OnInitialize()
    {
      panelView.musicToggle.onValueChanged.RemoveAllListeners();
      panelView.sfxToggle.onValueChanged.RemoveAllListeners();
      panelView.musicSlider.onValueChanged.RemoveAllListeners();
      panelView.sfxSlider.onValueChanged.RemoveAllListeners();

      panelView.musicToggle.isOn = (PlayerPrefs.GetInt(SettingKeys.MusicOn) > 0);
      panelView.musicSlider.interactable = (PlayerPrefs.GetInt(SettingKeys.MusicOn) > 0);
      panelView.musicSlider.value = PlayerPrefs.GetFloat(SettingKeys.MusicVolume);
      
      panelView.sfxToggle.isOn = (PlayerPrefs.GetInt(SettingKeys.SfxOn) > 0);
      panelView.sfxSlider.interactable = (PlayerPrefs.GetInt(SettingKeys.SfxOn) > 0);
      panelView.sfxSlider.value = PlayerPrefs.GetFloat(SettingKeys.SfxVolume);

      panelView.musicToggle.onValueChanged.AddListener(panelView.ToggleMusic);
      panelView.sfxToggle.onValueChanged.AddListener(panelView.ToggleSfx);
      panelView.musicSlider.onValueChanged.AddListener(panelView.MusicVolume);
      panelView.sfxSlider.onValueChanged.AddListener(panelView.SfxVolume);
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
      panelView.dispatcher.RemoveListener(SettingsEvents.ToggleSfx, OnToggleSFX);
      panelView.dispatcher.RemoveListener(SettingsEvents.ToggleMusic, OnToggleMusic);
      panelView.dispatcher.RemoveListener(SettingsEvents.MusicVolume, OnMusicVolume);
      panelView.dispatcher.RemoveListener(SettingsEvents.SfxVolume, OnSfxVolume);
      panelView.dispatcher.RemoveListener(SettingsEvents.Close, OnClose);
    }
  }
}
