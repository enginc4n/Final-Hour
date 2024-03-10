using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.Border
{
  public class BorderView : EventView
  {
    [SerializeField]
    private GameObject speedUpBorder;

    [SerializeField]
    private GameObject slowDownBorder;
    
    public void SetBorder(SpeedState speedState)
    {
      speedUpBorder.SetActive(speedState == SpeedState.Fast);
      slowDownBorder.SetActive(speedState == SpeedState.Slow);
    }
    
    public void SetState(bool isActive)
    {
      gameObject.SetActive(isActive);
    }
  }
}
