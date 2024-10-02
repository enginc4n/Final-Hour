using System.Collections;
using System.Globalization;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using DG.Tweening;
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

    public Image dashImage;

    public TextMeshProUGUI dashText;
    
    public TextMeshProUGUI dashCooldownText;

    public TextMeshProUGUI fireCooldownText;

    public GameObject flyingObstacleWarning;

    [SerializeField]
    private GameObject increaseParticle;

    [SerializeField]
    private GameObject decreaseParticle;

    [SerializeField]
    private GameObject flyingText;

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
    
    public void UpdateScore(int score)
    {
      scoreText.text = score.ToString();
    }

    public void SetIcon(SpeedState speedState)
    {
      hourglassIcon.SetActive(speedState != SpeedState.Normal);

      switch (speedState)
      {
        case SpeedState.Fast:
          increaseParticle.SetActive(false);
          decreaseParticle.SetActive(true);
          break;
        case SpeedState.Slow:
          increaseParticle.SetActive(true);
          decreaseParticle.SetActive(false);
          break;
        case SpeedState.Normal:
          increaseParticle.SetActive(false);
          decreaseParticle.SetActive(false);
          break;
      }
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
    
    public void FlyText(float amount)
    {
      GameObject flyingTextGo = Instantiate(flyingText, timerText.transform, true);
      Transform flyingTransform = flyingTextGo.transform;
      flyingTransform.localScale = Vector3.one;
      RectTransform rectTransform = flyingTransform.GetComponent<RectTransform>();
      TextMeshProUGUI text = flyingTextGo.GetComponent<TextMeshProUGUI>();

      if (amount > 0)
      {
        rectTransform.anchoredPosition = new Vector2(150, 20);
        flyingTransform.DOBlendableLocalMoveBy(new Vector3(0, 50, 0), 2.5f);
        
        text.text = "+" + amount.ToString(CultureInfo.InvariantCulture);
        text.color = Color.cyan;
      }
      else
      {
        rectTransform.anchoredPosition = new Vector2(150, -20);
        flyingTransform.DOBlendableLocalMoveBy(new Vector3(0, -50, 0), 2.5f);
        
        text.text = amount.ToString(CultureInfo.InvariantCulture);
        text.color = Color.red;
      }

      text.DOFade(0, 3).OnComplete(() =>
        {
          flyingTransform.DOKill();
          Destroy(flyingTextGo);
        }
        );
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
