using System;
using System.Collections;
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
    public SpriteRenderer spriteRenderer;
    
    public Animator enemyAnimator;

    [HideInInspector]
    public float speed = -9999f;
    
    public float modifiedSpeed => speed + (GameMechanicSettings.EnemySpeed * crashCount);

    public int crashCount;
    
    public float crashRemainingDistance = 0f; 

    private float playerPositionFromLeft => GameMechanicSettings.PlayerSpawnPosition.x - (playerBodyCollider.bounds.extents.x * playerBodyCollider.transform.localScale.x);
    
    private const float DistanceFromStart = GameMechanicSettings.EnemySpeed * GameMechanicSettings.EnemyTimeToCatchFromStart; //100 because 1 velocity means +100 position in 1 second

    private const float DistanceFromBarrier = GameMechanicSettings.EnemySpeed * GameMechanicSettings.EnemyCatchTimeFromMax;

    public bool alreadyDead;

    public bool wasPaused;

    private void FixedUpdate()
    {
      if (speed == 0 && modifiedSpeed == 0)
      {
        return;
      }
      
      if (enemyRigidBody.IsTouchingLayers(LayerMask.GetMask("Barrier")) && speed <= 0 && crashCount == 0)
      {
        return;
      }
      
      MoveEnemy();
    }

    private void MoveEnemy()
    {
      float deltaTime;
      
      if (wasPaused)
      {
        deltaTime = Time.deltaTime;
        wasPaused = false;
      }
      else
      {
        deltaTime = Time.unscaledDeltaTime;
      }

      float modifiedMovement = modifiedSpeed * deltaTime;
      float newPosition = transform.GetComponent<RectTransform>().anchoredPosition.x + modifiedMovement;
      
      transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(newPosition, transform.GetComponent<RectTransform>().anchoredPosition.y);

      if (crashCount == 0)
      {
        return;
      }
      
      float unmodifiedMovement = speed * deltaTime;
      float crushEffect = Math.Abs(unmodifiedMovement - modifiedMovement); //movement caused by crashing obstacle
      crashRemainingDistance -= crushEffect;

      if (crashRemainingDistance <= (crashCount - 1) * GameMechanicSettings.EnemySpeed * GameMechanicSettings.CrashPunishment)
      {
        crashCount--;
        if (modifiedSpeed != 0 && speed + (GameMechanicSettings.EnemySpeed * (crashCount + 1)) == 0)
        {
          dispatcher.Dispatch(EnemyControllerEvent.EnemyMoved);
        }
      }
    }
    
    public void ResetPosition()
    {
      transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(playerPositionFromLeft- DistanceFromStart - (enemyBoxCollider.bounds.extents.x * enemyBoxCollider.transform.localScale.x) + 10, transform.GetComponent<RectTransform>().anchoredPosition.y);
      enemyBarrier.GetComponent<RectTransform>().anchoredPosition = new Vector2(playerPositionFromLeft - DistanceFromBarrier - 
                                                                                (enemyBoxCollider.bounds.extents.x*2*enemyBoxCollider.transform.localScale.x) - 
                                                                                (barrierBoxCollider.bounds.extents.x/barrierBoxCollider.transform.lossyScale.x) + 10, enemyBarrier.GetComponent<RectTransform>().anchoredPosition.y);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
      if (alreadyDead) return;
      dispatcher.Dispatch(EnemyControllerEvent.CaughtPlayer);
    }

    public void KillAnimation()
    {
      enemyAnimator.SetTrigger("Catch");

      transform.DOBlendableMoveBy(new Vector3(0, 10, 0), 2).SetDelay(1.75f);
      spriteRenderer.DOColor(new Color(0.35f, 0.35f, 0.35f), 1f).SetDelay(1.5f);
      spriteRenderer.DOColor(new Color(0f, 0f, 0f, 0f), 1f).SetDelay(1.75f);    
    }
  }
}
