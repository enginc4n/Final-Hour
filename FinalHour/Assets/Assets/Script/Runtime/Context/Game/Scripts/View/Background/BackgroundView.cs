using System;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.Background
{
  public class BackgroundView : EventView
  {
    [HideInInspector]
    public float speed;
    
    [HideInInspector]
    public bool isAlive;
  }
}
