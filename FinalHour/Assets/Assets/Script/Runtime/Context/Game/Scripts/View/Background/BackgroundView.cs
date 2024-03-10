using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.Background
{
  public class BackgroundView : EventView
  {
    public RawImage image;

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
