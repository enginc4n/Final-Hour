using System;
using System.Collections.Generic;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.Background
{
  public class BackgroundView : EventView
  {
    public GameObject treeObject;
    public List<Sprite> treeImages;
    public Transform skyTransform;
    public Transform groundTransform;
    public Transform treeContainerTransform;
  }
}
