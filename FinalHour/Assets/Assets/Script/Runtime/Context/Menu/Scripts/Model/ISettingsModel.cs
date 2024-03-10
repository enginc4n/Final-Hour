using UnityEngine;

namespace Assets.Script.Runtime.Context.Menu.Scripts.Model
{
  public interface ISettingsModel
  {
    bool isOpen { get; }
    void OpenSettings(GameObject panel);

    void CloseSettings();
  }
}