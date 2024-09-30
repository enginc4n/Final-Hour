using System.Collections;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.Background
{
  public class BackgroundMediator : EventMediator
  {
    [Inject]
    public BackgroundView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }
    
    public void FixedUpdate()
    {
      if (!playerModel.isAlive) return;
      
      transform.Translate(new Vector2(-playerModel.currentGameSpeed, 0), Space.World);

      RectTransform rectTransform = transform.GetComponent<RectTransform>();
      
      if (rectTransform.anchoredPosition.x <= -rectTransform.sizeDelta.x)
      {
        rectTransform.anchoredPosition = Vector2.zero;
      }
    }
  }
}
