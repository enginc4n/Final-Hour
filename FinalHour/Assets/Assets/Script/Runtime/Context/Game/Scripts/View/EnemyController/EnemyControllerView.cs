using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.EnemyController
{
  public class EnemyControllerView : EventView
  {
    public Rigidbody2D enemyRigidBody;
    public BoxCollider2D enemyBoxCollider;
    public Animator enemyAnimator;

    public void ResetPosition()
    {
      transform.position = GameControlSettings.EnemySpawnPosition;
    }
    
    public void MoveEnemy(float enemySpeed)
    {
      enemyRigidBody.velocity = new Vector2(enemySpeed / Time.timeScale, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.gameObject.layer == LayerMask.NameToLayer("Barrier"))
      {
        dispatcher.Dispatch(EnemyControllerEvent.HitLimit);
      } else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
      {
        dispatcher.Dispatch(EnemyControllerEvent.CaughtPlayer);
        enemyAnimator.SetTrigger("Catch");
      }
    }
  }
}
