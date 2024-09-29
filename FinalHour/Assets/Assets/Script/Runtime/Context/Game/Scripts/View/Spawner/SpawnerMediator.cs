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

        Instantiate(objToSpawn, spawnPosition, Quaternion.identity, transform);

        yield return new WaitForSeconds(GameMechanicSettings.SpawnRate / playerModel.currentGameSpeed);
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
