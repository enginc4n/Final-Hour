using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.EnemyController
{
  public class EnemyControllerView : EventView
  {
    public Rigidbody2D enemyRigidBody;

    public void MoveEnemy(float enemySpeed)
    {
      enemyRigidBody.velocity = new Vector2(enemySpeed, 0f);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
      {
        dispatcher.Dispatch(EnemyControllerEvent.CaughtPlayer);
      }
    }
  }
}
