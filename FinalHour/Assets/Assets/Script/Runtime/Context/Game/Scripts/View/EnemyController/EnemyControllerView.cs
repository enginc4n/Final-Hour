using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using DG.Tweening;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.EnemyController
{
  public class EnemyControllerView : EventView
  {
    public Rigidbody2D enemyRigidBody;
    public BoxCollider2D enemyBoxCollider;
    public CircleCollider2D playerBodyCollider;
    public BoxCollider2D barrierBoxCollider;
    public Transform enemyBarrier;
    
    public Animator enemyAnimator;
    
    public void ResetPosition()
    {
      float playerPosition = playerBodyCollider.bounds.center.x - playerBodyCollider.bounds.extents.x;
      transform.position = new Vector2(playerPosition - (GameControlSettings.EnemyCatchFromStart*GameControlSettings.ObstaclePunish) - enemyBoxCollider.bounds.extents.x, transform.position.y);
      
      float enemyPosition = transform.position.x - enemyBoxCollider.bounds.extents.x;
      enemyBarrier.position = new Vector2(enemyPosition - ((GameControlSettings.EnemyCatchFromMax-GameControlSettings.EnemyCatchFromStart)*GameControlSettings.ObstaclePunish) - barrierBoxCollider.bounds.extents.x, transform.position.y);
    }
    
    public void MoveEnemy(float enemySpeed)
    {
      enemyRigidBody.velocity = new Vector2(enemySpeed / Time.timeScale, 0f);
    }

    public void MoveEnemyCrash()
    {
      transform.DOMoveX(GameControlSettings.ObstaclePunish, GameControlSettings.ObstaclePunish / GameControlSettings.EnemySpeed).SetRelative();
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
