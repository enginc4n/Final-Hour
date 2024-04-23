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

    public void Update()
    {
      if (!isAlive) return;

      RectTransform rectTransform = transform.GetComponent<RectTransform>();
      Vector2 pos = transform.position;
      pos.x -= speed * Time.deltaTime;

      if (rectTransform.anchoredPosition.x <= -rectTransform.sizeDelta.x)
      {
        rectTransform.anchoredPosition = Vector2.zero;
        return;
      }
      
      transform.position = pos;
    }
  }
}
