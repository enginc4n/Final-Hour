using strange.extensions.mediation.impl;
using UnityEngine.UI;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.SoundSettingsPanel
{
  public class SoundSettingsView : EventView
  {
    public Slider musicSlider;
    public Slider sfxSlider;

    public void ToogleSFX()
    {
      dispatcher.Dispatch(SoundSettingsEvents.ToogleSFX);
    }

    public void ToogleMusic()
    {
      dispatcher.Dispatch(SoundSettingsEvents.ToogleMusic);
    }

    public void MusicVolume()
    {
      dispatcher.Dispatch(SoundSettingsEvents.MusicVolume, musicSlider.value);
    }

    public void SFXVolume()
    {
      dispatcher.Dispatch(SoundSettingsEvents.SFXVolume, sfxSlider.value);
    }

    public void OnClose()
    {
      dispatcher.Dispatch(SoundSettingsEvents.Close);
    }

    public void OnExit()
    {
      dispatcher.Dispatch(SoundSettingsEvents.Exit);
    }
  }
}
