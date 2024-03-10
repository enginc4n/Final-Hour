using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using strange.extensions.mediation.impl;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.Spawner
{
  public class SpawnerView : EventView
  {
    [Serializable]
    public class WeightedObject
    {
      public GameObject obj;
      public float weight;
      public float minHeight;
      public float maxHeight;
    }

    public List<WeightedObject> weightedObjects;
    private float spawnInterval;

    private List<GameObject> spawnPool = new();
    private float totalWeight;
    private Coroutine spawnCoroutine;

    private void Start()
    {
      spawnInterval = GameControlSettings.spawnTime;

      foreach (WeightedObject weightedObject in weightedObjects)
      {
        totalWeight += weightedObject.weight;
      }

      foreach (WeightedObject weightedObject in weightedObjects)
      {
        int numToSpawn = Mathf.RoundToInt(weightedObject.weight / totalWeight * 100);
        for (int i = 0; i < numToSpawn; i++)
        {
          spawnPool.Add(weightedObject.obj);
        }
      }

      spawnCoroutine = StartCoroutine(SpawnObjects());
    }

    public void StopSpawn()
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
        WeightedObject weightedObject = FindWeightedObjectFromSpawnPool(objToSpawn);

        float randomHeight = Random.Range(weightedObject.minHeight, weightedObject.maxHeight);
        Vector3 spawnPosition = new(transform.position.x, randomHeight, transform.position.z);

        Instantiate(objToSpawn, spawnPosition, Quaternion.identity, transform);

        yield return new WaitForSeconds(spawnInterval);
      }
    }

    private WeightedObject FindWeightedObjectFromSpawnPool(GameObject objToFind)
    {
      return weightedObjects.FirstOrDefault(w => w.obj == objToFind);
    }
  }
}
