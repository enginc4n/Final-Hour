using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.Spawner
{
  public class SpawnerMediator : EventMediator
  {
    [Inject]
    public SpawnerView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }
    
    private List<GameObject> spawnPool = new();
    private float totalWeight;
    private Coroutine spawnCoroutine;

    public override void OnRegister()
    {
      dispatcher.AddListener(PlayerEvent.Died, OnDied);
    }

    private void OnDied()
    {
      StopSpawn();
    }

    private void Start()
    {
      foreach (SpawnerView.WeightedObject weightedObject in view.weightedObjects)
      {
        totalWeight += weightedObject.weight;
      }

      foreach (SpawnerView.WeightedObject weightedObject in view.weightedObjects)
      {
        int numToSpawn = Mathf.RoundToInt(weightedObject.weight / totalWeight * 100);
        for (int i = 0; i < numToSpawn; i++)
        {
          spawnPool.Add(weightedObject.obj);
        }
      }

      spawnCoroutine = StartCoroutine(SpawnObjects());
    }

    private void StopSpawn()
    {
      if (spawnCoroutine != null)
      {
        StopCoroutine(spawnCoroutine);
      }
    }

    private IEnumerator SpawnObjects()
    {
      while (true)
      {
        int randomIndex = Random.Range(0, spawnPool.Count);
        GameObject objToSpawn = spawnPool[randomIndex];
        SpawnerView.WeightedObject weightedObject = FindWeightedObjectFromSpawnPool(objToSpawn);

        float randomHeight = Random.Range(weightedObject.minHeight, weightedObject.maxHeight);
        Vector3 spawnPosition = new(transform.position.x, randomHeight, transform.position.z);

        ObstacleView obstacleView = objToSpawn.GetComponent<ObstacleView>();
        ObstacleType obstacleType = obstacleView.obstacleType;
        if (obstacleType == ObstacleType.Flying)
        {
          dispatcher.Dispatch(PlayerEvent.FlyingObstacleIncoming);
        }
        
        yield return new WaitForSeconds(GameMechanicSettings.FlyingObstacleWarningTime);

        GameObject spawnedObject = Instantiate(objToSpawn, spawnPosition, Quaternion.identity, transform);

        if (objToSpawn.CompareTag(ObstacleTag.Fire))
        {
          dispatcher.Dispatch(PlayerEvent.FireSound);
        }
        
        if (objToSpawn.CompareTag(ObstacleTag.Bird))
        {
          dispatcher.Dispatch(PlayerEvent.BirdSound);
        }

        if (obstacleType == ObstacleType.Collectible)
        {
          spawnedObject.GetComponent<ObstacleView>().ArcMove();
        }
        
        float currentRate = GameMechanicSettings.SpawnInterval / (playerModel.currentGameSpeed / GameMechanicSettings.StartingGameSpeed);

        float min = currentRate* 0.50f; 
        float max = currentRate * 1.50f; 
        float waitTme = Random.Range(min, max);
        
        if (obstacleView.obstacleType == ObstacleType.Collectible)
        {
          yield return new WaitForSeconds((waitTme / 2) - GameMechanicSettings.FlyingObstacleWarningTime);
        }
        else
        {
          yield return new WaitForSeconds(waitTme - GameMechanicSettings.FlyingObstacleWarningTime);
        }
      }
    }

    private SpawnerView.WeightedObject FindWeightedObjectFromSpawnPool(GameObject objToFind)
    {
      return view.weightedObjects.FirstOrDefault(w => w.obj == objToFind);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);
    }
  }
}
