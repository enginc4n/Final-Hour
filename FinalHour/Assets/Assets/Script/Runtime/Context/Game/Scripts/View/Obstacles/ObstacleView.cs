using System;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.View.Obstacles;
using DG.Tweening;
using strange.extensions.mediation.impl;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.Script.Runtime.Context.Game.Scripts.View
{
  public class ObstacleView : EventView
  {
    [Header("Obstacle Mood")]
    public bool isCollectible;
    public bool isBreakable;
    public bool isDropTime;
    public ObstacleType obstacleType;

    public float ownSpeedFactor = 1f;

    [Header("Settings")]
    public float rotationSpeed;

    [Header("Refrences")]
    public GameObject timeAdder;

    public ParticleSystem collectParticle;

    public ParticleSystem crushParticle;

    public ParticleSystem breakParticle;

    public Sequence sequence;

    private void Update()
    {
      RotateObject();
    }

    private void RotateObject()
    {
      if (!isCollectible)
      {
        return;
      }

      transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
      {
        if (isBreakable)
        {
          dispatcher.Dispatch(ObstacleEvents.ObstacleIsBroken);
          return;
        }
      }

      if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
      {
        dispatcher.Dispatch(ObstacleEvents.CrashWithPlayer);
        return;
      }

      if (other.gameObject.layer == LayerMask.NameToLayer("Barrier"))
      {
        Destroy(gameObject);
      }
    }

    public void InstantiateObject<T>(T objectToInstantiate) where T : Object
    { 
      Instantiate(objectToInstantiate, transform.position, Quaternion.identity, transform.parent);
    } 
    
    public void ArcMove()
    {
      Vector3 position = transform.position;
      transform.position = new Vector2(position.x, Random.Range(-3f, position.y + 1));

      sequence = DOTween.Sequence();

      if (Random.Range(0, 1) > 0)
      {
        sequence.Append(transform.DOMoveY(-3, GetSpeed(transform.position.y,-3)).SetEase(Ease.Linear));
        sequence.Append(transform.DOMoveY( position.y + 1,GetSpeed(-3, position.y + 1)).SetEase(Ease.Linear));
        sequence.Append(transform.DOMoveY(position.y, GetSpeed(position.y + 1, position.y)).SetEase(Ease.Linear));
      }
      else
      {
        sequence.Append(transform.DOMoveY( position.y + 1,GetSpeed(transform.position.y,position.y + 1)).SetEase(Ease.Linear));
        sequence.Append(transform.DOMoveY(-3, GetSpeed(position.y + 1,-3)).SetEase(Ease.Linear));
        sequence.Append(transform.DOMoveY(position.y, GetSpeed(-3,position.y)).SetEase(Ease.Linear));
      }

      sequence.SetLoops(-1, LoopType.Yoyo);
      sequence.Play();
    }

    private float GetSpeed(float start, float target)
    {
      float distance = Math.Abs(start - target);

      return distance / 5f;
    }
  }
}
