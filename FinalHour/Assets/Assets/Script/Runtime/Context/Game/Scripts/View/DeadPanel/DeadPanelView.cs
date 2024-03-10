using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.DeadPanel
{
  public class DeadPanelView : EventView
  {
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [HideInInspector]
    public int score;
    public void SetState(bool isActive)
    {
      gameObject.SetActive(isActive);

      if (isActive)
      {
        scoreText.text = score.ToString();
      }
    }

    public void OnPlayAgain()
    {
      
    }
  }
}