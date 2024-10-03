using System.Collections.Generic;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Menu.Scripts.Model
{
  public interface IUIModel
  {
    public Dictionary<GameObject, string> openPanels { get; }
    
    void OpenPanel(string panelKey, Transform layer);

    void ClosePanel(string panelKey);
  }
}