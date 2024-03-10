using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View
{
  public class ObstacleView : EventView
  {
    public Rigidbody2D obstacleRigidbody2D;

    public void TranslateObstacle(Vector2 speed)
    {
      obstacleRigidbody2D.velocity = speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.gameObject.layer == LayerMask.NameToLayer("Destroyer"))
      {
        Destroy(gameObject);
      }
    }
  }
}
