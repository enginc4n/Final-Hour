using System.Collections.Generic;
using Assets.Script.Runtime.Context.Menu.Scripts.View.SoundSettingsPanel;
using strange.extensions.mediation.impl;
using UnityEngine.UI;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.SettingsPanel
{
  public class SettingsPanelView : EventView
  {
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle musicToggle;
    public Toggle sfxToggle;

    public void ToggleSfx(bool isOn)
    {
      dispatcher.Dispatch(SettingsEvents.ToggleSfx);
    }

    public void ToggleMusic(bool isOn)
    {
      dispatcher.Dispatch(SettingsEvents.ToggleMusic);
    }

    public void MusicVolume(float value)
    {
      dispatcher.Dispatch(SettingsEvents.MusicVolume, musicSlider.value);
    }

    public void SfxVolume(float value)
    {
      dispatcher.Dispatch(SettingsEvents.SfxVolume, sfxSlider.value);
    }

    public void OnClose()
    {
      dispatcher.Dispatch(SettingsEvents.Close);
    }
  }
}
