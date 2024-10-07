using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.DeadPanel
{
  public class DeadPanelView : EventView
  {
    [SerializeField]
    private TextMeshProUGUI scoreText;
    
    [SerializeField]
    private TextMeshProUGUI deathMessage;

    [HideInInspector]
    public int score;
    public void SetState(bool isActive, bool timeDeath = true)
    {
      gameObject.SetActive(isActive);

      if (isActive)
      {
        scoreText.text = score.ToString();
        
        if (timeDeath)
        {
          deathMessage.text = "You've run out of time!";
        }
        else
        {
          deathMessage.text = "Death has caught up to you...";
        }
      }
    }

    public void OnPlayAgain()
    {
      dispatcher.Dispatch(DeadPanelEvent.PlayAgain);
    }


    public void OnMenu()
    {
      dispatcher.Dispatch(DeadPanelEvent.Menu);
    }
  }
}