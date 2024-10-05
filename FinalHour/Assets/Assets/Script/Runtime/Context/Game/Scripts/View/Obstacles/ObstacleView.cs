using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.View.Obstacles;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using strange.extensions.mediation.impl;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.Script.Runtime.Context.Game.Scripts.View
{
  public class ObstacleView : EventView
  {
    public RectTransform rectTransform;
    
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

    public Transform imageTransform;

    public Coroutine activeRoutine;

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

      imageTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
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

      if (other.gameObject.layer == LayerMask.NameToLayer("ObstacleBarrier"))
      {
        Destroy(gameObject);
      }
    }

    public T InstantiateObject<T>(T objectToInstantiate) where T : Object
    {
      return Instantiate(objectToInstantiate, transform.position, Quaternion.identity, transform.parent);
    } 
    
    public void ArcMove( )
    {
      float maxHeight = GameMechanicSettings.JumpHeight - 100;
      float minHeight = rectTransform.anchoredPosition.y;

      rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, Random.Range(minHeight,maxHeight));


      activeRoutine = StartCoroutine(Random.Range(0, 1) > 0 ? ArcFromUp(minHeight, maxHeight) : ArcFromDown(minHeight, maxHeight));
    }

    private IEnumerator ArcFromUp(float minHeight, float maxHeight)
    {
      while (true)
      {
        TweenerCore<Vector2, Vector2, VectorOptions> tween = rectTransform.DOAnchorPosY(maxHeight, 250).SetSpeedBased();
        
        yield return tween.WaitForCompletion();
        
        tween = rectTransform.DOAnchorPosY(minHeight, 250).SetSpeedBased();
        
        yield return tween.WaitForCompletion();
      }
    }
    
    private IEnumerator ArcFromDown(float minHeight, float maxHeight)
    {
      while (true)
      {
        TweenerCore<Vector2, Vector2, VectorOptions>  tween = rectTransform.DOAnchorPosY(minHeight, 250).SetSpeedBased().SetEase(Ease.Linear);
        
        yield return tween.WaitForCompletion();
        
        tween = rectTransform.DOAnchorPosY(maxHeight, 250).SetSpeedBased().SetEase(Ease.Linear);
        
        yield return tween.WaitForCompletion();
      }
    }

  }
}
