using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.GameHud
{
  public class GameHudView : EventView
  {
    public RawImage image;

    public TextMeshProUGUI timerText;

    public TextMeshProUGUI scoreText;

    [SerializeField]
    private GameObject speedUpBorder;

    [SerializeField]
    private GameObject slowDownBorder;
    
    [SerializeField]
    private GameObject hourglassIcon;
    

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

    public void ParallaxEffect(float currentSpeed)
    {
      if (image.uvRect.x >= 1)
      {
        image.uvRect = new Rect(new Vector2(0, image.uvRect.y), image.uvRect.size);
      }

      image.uvRect = new Rect(new Vector2(image.uvRect.position.x + currentSpeed * Time.deltaTime, image.uvRect.position.y), image.uvRect.size);
    }

    public void SetBorder(SpeedState speedState)
    {
      speedUpBorder.SetActive(speedState == SpeedState.Fast);
      slowDownBorder.SetActive(speedState == SpeedState.Slow);
      hourglassIcon.SetActive(speedState != SpeedState.Normal);
    }
  }
}
