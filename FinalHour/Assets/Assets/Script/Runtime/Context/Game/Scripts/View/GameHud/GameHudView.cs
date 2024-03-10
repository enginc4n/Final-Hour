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

    public void UpdateTimer(float remainingTime)
    {
      timerText.text = remainingTime.ToString();
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
  }
}
