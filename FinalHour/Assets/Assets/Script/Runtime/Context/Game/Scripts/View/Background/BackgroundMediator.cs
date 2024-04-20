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
    
    [Inject]
    public ISpeedModel speedModel { get; set; }
    
    public override void OnInitialize()
    {
      StartCoroutine(LoopBackground());
      speedModel.gameDistance = view.image.transform.GetComponent<RectTransform>().sizeDelta.x;
    }

    private IEnumerator LoopBackground()
    {
      while (playerModel.isAlive)
      {
        view.ParallaxEffect(playerModel.currentGameSpeed);
        yield return null;
      }
    }
  }
}
