using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.DeadPanel
{
  public class DeadPanelView : EventView
  {
    [SerializeField]
    private AdManager adManager;
    
    [SerializeField]
    private TextMeshProUGUI scoreText;
    
    [SerializeField]
    private TextMeshProUGUI deathMessage;

    [HideInInspector]
    public int score;
    public void SetState(bool isActive, bool timeDeath = true)
    {
      gameObject.SetActive(isActive);

      if (!isActive) return;
      
      scoreText.text = score.ToString();
        
      deathMessage.text = timeDeath ? "You've run out of time!" : "Death has caught up to you...";
    }

    public void ShowAd()
    {
      adManager.LoadAd();
    }
    
    public void HideAd()
    {
      adManager.DestroyAd();
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