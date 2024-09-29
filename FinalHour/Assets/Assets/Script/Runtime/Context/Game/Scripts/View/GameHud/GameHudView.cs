using System.Collections;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.GameHud
{
  public class GameHudView : EventView
  {
    public TextMeshProUGUI timerText;

    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI dashCooldownText;

    public TextMeshProUGUI fireCooldownText;

    [SerializeField]
    private GameObject hourglassIcon;

    [SerializeField]
    private Graphic shadowImage;

    [SerializeField]
    private Image fireCooldownImage;

    [SerializeField]
    private Image dashCooldownImage;
    
    private const float CooldownTickRate = 0.01f;

    private float _currentDashTime;

    private float _currentFireTime;

    public void UpdateDashTimer(float remainingTime)
    {
      int minutes = Mathf.FloorToInt(remainingTime / 60f);
      int seconds = Mathf.FloorToInt(remainingTime % 60f);

      timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateScore(int score)
    {
      scoreText.text = score.ToString();
    }

    public void SetIcon(SpeedState speedState)
    {
      hourglassIcon.SetActive(speedState != SpeedState.Normal);
    }

    public void SetShadowOpacity(float opacity)
    {
      Color color = shadowImage.color;
      shadowImage.color = new Color(color.r, color.b, color.g, opacity);
    }

    public void SetState(bool isActive)
    {
      gameObject.SetActive(isActive);
    }
    
    public void StartDashTimer()
    {
      _currentDashTime = GameMechanicSettings.DashCooldown;
      InvokeRepeating(nameof(UpdateDashTimer), 0f, CooldownTickRate); 
    }
    
    private void UpdateDashTimer()
    {
      _currentDashTime -= CooldownTickRate; 

      if (_currentDashTime <= 0)
      {
        _currentDashTime = 0;
        CancelInvoke(nameof(UpdateDashTimer));
      }

      UpdateTimer(dashCooldownImage, dashCooldownText, _currentDashTime, GameMechanicSettings.DashCooldown);
    }
    
    public void StartFireTimer()
    {
      _currentFireTime = GameMechanicSettings.FireCooldown;
      InvokeRepeating(nameof(UpdateFireTimer), 0f, CooldownTickRate); 
    }
    
    private void UpdateFireTimer()
    {
      _currentFireTime -= CooldownTickRate; 

      if (_currentFireTime <= 0)
      {
        _currentFireTime = 0;
        CancelInvoke(nameof(UpdateFireTimer));
      }
 
      UpdateTimer(fireCooldownImage, fireCooldownText, _currentFireTime, GameMechanicSettings.FireCooldown);
    }

    private void UpdateTimer(Image image, TMP_Text label, float currentCooldown, float totalCooldown)
    {
      if (currentCooldown > 0)
      {
        image.fillAmount = currentCooldown / totalCooldown;
        label.text = Mathf.Ceil(currentCooldown).ToString();
      }
      else
      {
        image.fillAmount = 0;
        label.text = string.Empty;
      }
    }

    public void OnSettings()
    {
      dispatcher.Dispatch(GameHudEvent.Settings, transform);
    }
  }
}
