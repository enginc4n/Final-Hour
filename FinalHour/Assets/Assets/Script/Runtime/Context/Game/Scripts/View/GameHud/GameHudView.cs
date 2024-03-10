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
    
    [SerializeField]
    private GameObject hourglassIcon;
    
    [SerializeField]
    private Graphic shadowImage;

    public void UpdateTimer(float remainingTime)
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
  }
}
