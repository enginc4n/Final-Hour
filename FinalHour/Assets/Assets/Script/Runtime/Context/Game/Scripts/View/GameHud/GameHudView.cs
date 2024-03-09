using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.GameHud
{
  public class GameHudView : EventView
  {
    public RawImage image ;

    public float defaultSpeed;
    
    private float _currentSpeed;

    protected override void Start()
    {
      ResetSpeed();
    }
    
    private void Update()
    {
      if (image.uvRect.x >= 1)
      {
        image.uvRect = new Rect(new Vector2(0, image.uvRect.y), image.uvRect.size);
      }
      
      image.uvRect = new Rect(new Vector2(image.uvRect.position.x + _currentSpeed * Time.deltaTime, image.uvRect.position.y), image.uvRect.size);
    }

    public void SpeedUp()
    {
      _currentSpeed *= 2;
    }
    
    public void SlowDown()
    {
      _currentSpeed /= 2;
    }

    public void ResetSpeed()
    {
      _currentSpeed = defaultSpeed;
    }
  }
}