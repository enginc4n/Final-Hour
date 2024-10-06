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
    public GameObject finishTutorialRaycast;
    
    public TextMeshProUGUI timerText;

    public TextMeshProUGUI scoreText;

    public Image dashImage;

    public TextMeshProUGUI dashText;
    
    public TextMeshProUGUI dashCooldownTextPc;
    
    public TextMeshProUGUI dashCooldownTextMobile;

    public TextMeshProUGUI fireCooldownTextPc;
    
    public TextMeshProUGUI fireCooldownTextMobile;

    public GameObject flyingObstacleWarning;

    public GameObject raycastGo;

    public GameObject mobilePad;

    public GameObject pcHud;

    public Button fireButton;

    public Button dashButton;

    public Button jumpButton;

    public Button crouchButton;

    [SerializeField]
    private TextMeshProUGUI outOfSeconds;

    [SerializeField]
    private GameObject increaseParticle;

    [SerializeField]
    private GameObject decreaseParticle;

    [SerializeField]
    private GameObject flyingText;
    
    public Transform timerTransform;
    
    public GameObject timerTutorialArrow;

    public Transform shadowTransform;

    public GameObject deathTutorialArrow;

    [SerializeField]
    private GameObject hourglassIcon;

    [SerializeField]
    private Graphic shadowImage;

    [SerializeField]
    private Image fireCooldownImagePc;
    
    [SerializeField]
    private Image fireCooldownImageMobile;

    [SerializeField]
    private Image dashCooldownImagePc;

    [SerializeField]
    private Image dashCooldownImageMobile;

    [HideInInspector]
    public DeviceType deviceType;
    
    private const float CooldownTickRate = 0.01f;

    private float _currentDashTime;

    private float _currentFireTime;

    private Sequence outOfSecondsTween;

    [Header("Tutorial")]
    public TextMeshProUGUI pcTutorialText;

    public bool isTutorialActive;

    public GameObject tiltLeftImage;
    
    public GameObject tiltRightImage;
    
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
      GameObject flyingTextGo = Instantiate(flyingText, timerTransform, true);
      Transform flyingTransform = flyingTextGo.transform;
      flyingTransform.localScale = Vector3.one;
      RectTransform rectTransform = flyingTransform.GetComponent<RectTransform>();
      TextMeshProUGUI text = flyingTextGo.GetComponent<TextMeshProUGUI>();

      if (amount > 0)
      {
        rectTransform.anchoredPosition = new Vector2(0, 20);
        flyingTransform.DOBlendableLocalMoveBy(new Vector3(0, 50, 0), 2.5f);
        
        text.text = "+" + amount.ToString(CultureInfo.InvariantCulture);
        text.color = Color.cyan;
      }
      else
      {
        rectTransform.anchoredPosition = new Vector2(0, -20);
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
        
        if (deviceType == DeviceType.Handheld)
        {
          if (isTutorialActive)
          {
            return;
          }
          
          dashButton.interactable = true;
        }
      }

      if (deviceType == DeviceType.Handheld)
      {
        UpdateTimer(dashCooldownImageMobile, dashCooldownTextMobile, _currentDashTime, GameMechanicSettings.DashCooldown);
      }
      else
      {
        UpdateTimer(dashCooldownImagePc, dashCooldownTextPc, _currentDashTime, GameMechanicSettings.DashCooldown);
      };
    }
    
    public void StartFireTimer()
    {
      if (deviceType == DeviceType.Handheld)
      {
        fireButton.interactable = false;
      }
      
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

        if (deviceType == DeviceType.Handheld)
        {
          if (isTutorialActive)
          {
            return;
          }
          
          fireButton.interactable = true;
        }
      }

      if (deviceType == DeviceType.Handheld)
      {
        UpdateTimer(fireCooldownImageMobile, fireCooldownTextMobile, _currentFireTime, GameMechanicSettings.FireCooldown);
      }
      else
      {
        UpdateTimer(fireCooldownImagePc, fireCooldownTextPc, _currentFireTime, GameMechanicSettings.FireCooldown);
      }
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

    public void OutOfSeconds()
    {
      if (outOfSecondsTween != null)
      {
        if (outOfSecondsTween.IsPlaying())
        {
          outOfSecondsTween.Complete();
        }
      }

      Sequence sequence = DOTween.Sequence();
      sequence.Append(outOfSeconds.transform.DOBlendableLocalMoveBy(new Vector3(0, 50f, 0), 2));
      sequence.Join(outOfSeconds.DOFade(0f, 2).From(1f));
      sequence.OnComplete((() => { outOfSecondsTween.Rewind(); }));

      outOfSecondsTween = sequence;
      outOfSecondsTween.Play();
    }

    public void OnSettings()
    {
      dispatcher.Dispatch(GameHudEvent.Settings);
    }

    public void OnFinishTutorial()
    {
      dispatcher.Dispatch(GameHudEvent.FinishTutorial);
    }
  }
}
