using UnityEngine;

namespace Assets.Script.Runtime.Context.Menu.Scripts.Model
{
  public class SettingsModel : ISettingsModel
  {
    public bool isOpen { get; set;  }

    private GameObject openPanel;

    public void OpenSettings(GameObject panel)
    {
      isOpen = true;
      openPanel = panel;
    }

    public void CloseSettings()
    {
      Object.Destroy(openPanel);
      isOpen = false;
      openPanel = null;
    }
  }
}