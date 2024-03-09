using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.GameHud
{
  public class GameHudView : EventView
  {
    public RawImage image;

    public float defaultSpeed;

    public float score;

    public float addingScoreAmount;

    public float decrementTimeAmount;

    private float _currentSpeed;

    private void Update()
    {
      AddScore();
    }

    private void AddScore()
    {
      score += addingScoreAmount;
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
