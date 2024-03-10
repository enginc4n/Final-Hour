using strange.extensions.command.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets.Script.Runtime.Context.Menu.Scripts.Command
{
  public class OpenSettingsCommand : EventCommand
  {
    public override void Execute()
    {
      InstantiateGameObject();
    }

    void InstantiateGameObject()
    {
      AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("SettingsPanel");

      handle.Completed += (op) =>
      {
        if (op.Status != AsyncOperationStatus.Succeeded)
        {
          Debug.LogError("Failed to load addressable asset: " + op.DebugName);
        }
      };
    }
  }
}