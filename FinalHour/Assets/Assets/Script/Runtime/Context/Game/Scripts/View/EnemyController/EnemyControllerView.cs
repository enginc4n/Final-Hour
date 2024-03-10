using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.EnemyController
{
  public class EnemyControllerView : EventView
  {
    public Rigidbody2D enemyRigidBody;
    public BoxCollider2D enemyBoxCollider;
    public Animator enemyAnimator;

    public void MoveEnemy(float enemySpeed)
    {
      enemyRigidBody.velocity = new Vector2(enemySpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
      {
        //dispatcher.Dispatch(EnemyControllerEvent.HitLimit);
        return;
      }

      dispatcher.Dispatch(EnemyControllerEvent.CaughtPlayer);
      enemyAnimator.SetTrigger("Catch");
    }
  }
}
