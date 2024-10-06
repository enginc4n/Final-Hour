using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
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
    private bool nextStepReady = true;

    public override void OnRegister()
    {
      dispatcher.AddListener(PlayerEvent.Died, OnDied);
      dispatcher.AddListener(GameEvent.TutorialObstaclePassed, OnTutorialObstaclePassed);
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
        GameObject objToSpawn;
        ObstacleView obstacleView;
        
        if (PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) == 0)
        {
          objToSpawn = view.tutorialObjects[0];
          playerModel.tutorialActive = true;
          obstacleView = objToSpawn.GetComponent<ObstacleView>();
          obstacleView.tutorialIndex = 0;
        }
        else if (PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) == 1)
        {
          objToSpawn = view.tutorialObjects[1];
          playerModel.tutorialActive = true;
          obstacleView = objToSpawn.GetComponent<ObstacleView>();
          obstacleView.tutorialIndex = 1;
        } 
        else if (PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) == 2)
        {
          objToSpawn = view.tutorialObjects[2];
          playerModel.tutorialActive = true;
          obstacleView = objToSpawn.GetComponent<ObstacleView>();
          obstacleView.tutorialIndex = 2;
        }     
        else if (PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) == 3)
        {
          objToSpawn = view.tutorialObjects[3];
          playerModel.tutorialActive = true;
          obstacleView = objToSpawn.GetComponent<ObstacleView>();
          obstacleView.tutorialIndex = 3;
        } 
        else
        {
          int randomIndex = Random.Range(0, spawnPool.Count);
          objToSpawn = spawnPool[randomIndex];
          obstacleView = objToSpawn.GetComponent<ObstacleView>();
        }
        nextStepReady = false;
        
        SpawnerView.WeightedObject weightedObject = FindWeightedObjectFromSpawnPool(objToSpawn);
        
        ObstacleType obstacleType = obstacleView.obstacleType;
        if (obstacleType == ObstacleType.Flying)
        {
          dispatcher.Dispatch(PlayerEvent.FlyingObstacleIncoming);
        }
        
        yield return new WaitForSeconds(GameMechanicSettings.FlyingObstacleWarningTime);

        GameObject spawnedObject = Instantiate(objToSpawn, transform.position, Quaternion.identity, transform.parent);
        
        float randomHeight = Random.Range(weightedObject.minHeight, weightedObject.maxHeight);

        RectTransform spawnedRectTransform = spawnedObject.GetComponent<RectTransform>();
        spawnedObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(spawnedRectTransform.anchoredPosition.x, randomHeight);

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
          obstacleView.ArcMove();
        }
        
        float currentRate = GameMechanicSettings.SpawnInterval / (playerModel.currentGameSpeed / GameMechanicSettings.StartingGameSpeed);

        float min = currentRate* 0.75f; 
        float max = currentRate * 1.25f; 
        float waitTme = Random.Range(min, max);
        
        if (obstacleView.obstacleType == ObstacleType.Collectible)
        {
          yield return new WaitForSeconds((waitTme / 2) - GameMechanicSettings.FlyingObstacleWarningTime);
        }
        else if (playerModel.tutorialActive)
        {
          yield return new WaitUntil(() => nextStepReady);
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

    private void OnTutorialObstaclePassed()
    {
      nextStepReady = true;
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(PlayerEvent.Died, OnDied);
      dispatcher.RemoveListener(GameEvent.TutorialObstaclePassed, OnTutorialObstaclePassed);
    }
  }
}
