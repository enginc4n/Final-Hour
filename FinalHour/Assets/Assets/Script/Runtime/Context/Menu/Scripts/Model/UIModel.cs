using System.Collections.Generic;
using System.Linq;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets.Script.Runtime.Context.Menu.Scripts.Model
{
  public class UIModel : IUIModel
  {
    public Dictionary<GameObject, string> openPanels { get; } = new();

    public void OpenPanel(string panelKey, Transform layer)
    {
      if (openPanels.All(obj => obj.Value != panelKey))
      {
        Addressables.InstantiateAsync(panelKey).Completed += handle => PanelOpened(handle, layer, panelKey);
      }
      else
      {
        ClosePanel(panelKey);
      }
    }
    
    private void PanelOpened(AsyncOperationHandle<GameObject> handle, Transform layer, string panelKey)
    {
      if (handle.Status != AsyncOperationStatus.Succeeded) return;
      GameObject panel = handle.Result;
      openPanels.Add(panel, panelKey);

      Transform panelTransform = panel.transform;
      panelTransform.SetParent(layer);
      panelTransform.localScale = Vector3.one;
      panelTransform.position = Vector3.zero;
    }
    
    public void ClosePanel(string panelKey)
    {
      if (openPanels.All(obj => obj.Value != panelKey)) return;
      {
        (GameObject key, _) = openPanels.FirstOrDefault(obj => obj.Value ==  panelKey);
        Object.Destroy(key);
        openPanels.Remove(key);
      }
    }
  }
}