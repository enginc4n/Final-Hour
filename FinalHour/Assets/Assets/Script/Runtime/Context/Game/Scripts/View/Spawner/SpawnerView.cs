using System;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;

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
  }
}
