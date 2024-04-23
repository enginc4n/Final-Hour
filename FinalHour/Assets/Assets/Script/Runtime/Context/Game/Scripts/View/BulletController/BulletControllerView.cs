using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.BulletController
{
  public class BulletControllerView : EventView
  {
    public Rigidbody2D bulletRigidBody;

    protected override void OnEnable()
    {
      bulletRigidBody.velocity = new Vector2(GameControlSettings.BulletSpeed, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.gameObject.layer == LayerMask.NameToLayer("Barrier") || other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
      {
        gameObject.SetActive(false);
      }
    }
  }
}
