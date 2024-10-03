using System.Collections.Generic;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.Model
{
  public class AudioModel : IAudioModel
  {
    public AudioSource musicSource { get; set; }
    public AudioSource sfxSource { get; set; }
    public AudioSource timeSpeedSource { get; set; }
    public AudioSource deathSoundSource { get; set; }

    private bool _scaledMusicVolume;
    private bool _scaledSfxVolume;
    

    public void SetMusicVolume(float volume)
    {
      if (_scaledMusicVolume)
      {
        float currentScale = musicSource.volume / PlayerPrefs.GetFloat(SettingKeys.MusicVolume);
        PlayerPrefs.SetFloat(SettingKeys.MusicVolume, volume);
        musicSource.volume = currentScale * volume;
      }
      else
      {
        PlayerPrefs.SetFloat(SettingKeys.MusicVolume, volume);
        musicSource.volume = volume;
      }
    }
    
    public void SetSfxVolume(float volume)
    {
      if (_scaledSfxVolume)
      {
        float currentScale = sfxSource.volume / PlayerPrefs.GetFloat(SettingKeys.SfxVolume);
        PlayerPrefs.SetFloat(SettingKeys.SfxVolume, volume);
        sfxSource.volume = currentScale * volume;
      }
      else
      {
        PlayerPrefs.SetFloat(SettingKeys.SfxVolume, volume);
        sfxSource.volume = volume;
      }

      deathSoundSource.volume = volume;
      timeSpeedSource.volume = volume;
    }
    
    public void SetPitchVolumeRelative(float volume, float pitch)
    {
      musicSource.volume = GetScaledValue(volume, PlayerPrefs.GetFloat(SettingKeys.MusicVolume)*0.2f, PlayerPrefs.GetFloat(SettingKeys.MusicVolume));
      musicSource.pitch = GetScaledValue(pitch, 0.5f, 1f);
      
      sfxSource.volume = GetScaledValue(volume, 0, PlayerPrefs.GetFloat(SettingKeys.SfxVolume));
      sfxSource.pitch = GetScaledValue(pitch, 0.95f, 1f);

      _scaledMusicVolume = true;
      _scaledSfxVolume = true;
    }
    
    public void SetPitchVolume(float volume, float pitch)
    {
      musicSource.volume = volume;
      musicSource.pitch = pitch;
      
      sfxSource.volume = volume;
      sfxSource.pitch = pitch;
      
      _scaledMusicVolume = true;
      _scaledSfxVolume = true;
    }
    
    public void ResetPitchVolume()
    { 
      if (!PlayerPrefs.HasKey(SettingKeys.MusicVolume))
      {
        PlayerPrefs.SetFloat(SettingKeys.MusicVolume, GameMechanicSettings.DefaultMusicVolume);
      }
      
      if (!PlayerPrefs.HasKey(SettingKeys.SfxVolume))
      {
        PlayerPrefs.SetFloat(SettingKeys.SfxVolume, GameMechanicSettings.DefaultSfxVolume);
      }
      
      if (!PlayerPrefs.HasKey(SettingKeys.MusicOn))
      {
        PlayerPrefs.SetInt(SettingKeys.MusicOn, 1);
      }
      
      if (!PlayerPrefs.HasKey(SettingKeys.SfxOn))
      {
        PlayerPrefs.SetInt(SettingKeys.SfxOn, 1);
      }
      
      musicSource.volume = PlayerPrefs.GetFloat(SettingKeys.MusicVolume);
      musicSource.pitch = 1;
      
      sfxSource.volume = PlayerPrefs.GetFloat(SettingKeys.SfxVolume);
      sfxSource.pitch = 1;
      
      _scaledMusicVolume = false;
      _scaledSfxVolume = false;
    }

    public void ToggleMusic()
    {
      if (PlayerPrefs.GetInt(SettingKeys.MusicOn) > 0)
      {
        musicSource.enabled = false;
        PlayerPrefs.SetInt(SettingKeys.MusicOn, 0);
      }
      else
      {
        musicSource.enabled = true;
        PlayerPrefs.SetInt(SettingKeys.MusicOn, 1);
      }
    }

    public void ToggleSfx()
    {
      if (PlayerPrefs.GetInt(SettingKeys.SfxOn) > 0)
      {
        timeSpeedSource.enabled = false;
        deathSoundSource.enabled = false;
        sfxSource.enabled = false;
        PlayerPrefs.SetInt(SettingKeys.SfxOn, 0);
      }
      else
      {
        timeSpeedSource.enabled = true;
        deathSoundSource.enabled = true;
        sfxSource.enabled = true;
        PlayerPrefs.SetInt(SettingKeys.SfxOn, 1);
      }
    }

    private float GetScaledValue(float value, float min, float max)
    {
      float range = max - min;

      return min + (range * value);
    }

    public void Pause()
    {
      musicSource.Pause();
      
      sfxSource.Pause();
      
      timeSpeedSource.Pause();
      
      deathSoundSource.Pause();
    }
    
    public void Resume()
    {
      musicSource.Play();
      
      sfxSource.Play();
      
      timeSpeedSource.Play();
      
      deathSoundSource.Play();
    }
  }
}