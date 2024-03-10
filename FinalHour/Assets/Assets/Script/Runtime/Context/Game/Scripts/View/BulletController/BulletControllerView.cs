using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.BulletController
{
  public class BulletControllerView : EventView
  {
    public Rigidbody2D bulletRigidBody;

    public void TranslateBullet(Vector2 bulletSpeed)
    {
      bulletRigidBody.velocity = bulletSpeed;
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
