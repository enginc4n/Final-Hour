using Assets.Script.Runtime.Context.Game.Scripts.Config;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
using Scripts.Runtime.Modules.Core.PromiseTool;
using strange.extensions.command.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Assets.Script.Runtime.Context.Menu.Scripts.Command
{
  public class OpenSettingsCommand : EventCommand
  {
    [Inject]
    public ISettingsModel settingsModel { get; set; }
    
    [Inject]
    public ISpeedModel speedModel { get; set; }
    public override void Execute()
    {
      if (!settingsModel.isOpen)
      {
        speedModel.Pause();
        
        Transform transform = (Transform)evt.data;
        GameObject panel = ObjectPool.instance.GetPooledSettings();
        panel.SetActive(true);
        panel.transform.SetParent(transform);
        panel.transform.localScale = Vector3.one;
        
        settingsModel.OpenSettings(panel);
      }
      else
      {
        settingsModel.CloseSettings();
        speedModel.Continue();
      }
    }
  }
}