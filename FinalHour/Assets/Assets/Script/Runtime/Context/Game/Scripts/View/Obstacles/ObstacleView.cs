﻿using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View
{
  public class ObstacleView : EventView
  {
    [Header("Obstacle Mood")]
    public bool isCollectible;

    public bool isBrokable;
    public bool isDashable;
    public bool isDropTime;

    [Header("Settings")]
    public float rotationSpeed;

    [Header("Refrences")]
    public Rigidbody2D obstacleRigidbody2D;

    public GameObject timeAdder;

    public ParticleSystem collectParticle;

    public ParticleSystem crushParticle;

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
        if (isBrokable)
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

    public void TranslateObstacle(Vector2 speed)
    {
      obstacleRigidbody2D.velocity = speed;
    }

    public void InstantiateObject<T>(T objectToInstantiate) where T : Object
    { 
      Instantiate(objectToInstantiate, transform.position, Quaternion.identity, transform.parent);
    } 
  }
}
