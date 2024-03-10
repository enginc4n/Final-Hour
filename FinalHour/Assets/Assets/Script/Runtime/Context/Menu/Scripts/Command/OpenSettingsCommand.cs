using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
using Scripts.Runtime.Modules.Core.PromiseTool;
using strange.extensions.command.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets.Script.Runtime.Context.Menu.Scripts.Command
{
  public class OpenSettingsCommand : EventCommand
  {
    [Inject]
    public ISettingsModel settingsModel { get; set; }
    public override void Execute()
    {
      if (!settingsModel.isOpen)
      {
        Transform transform = (Transform)evt.data;

        SpawnObject(transform);
      }
      else
      { 
        settingsModel.CloseSettings();
      }
    }

    private IPromise SpawnObject( Transform transform)
    {
      Promise promise = new();

      AsyncOperationHandle asyncOperationHandle = Addressables.InstantiateAsync(
        MenuAddressableKeys.SettingsPanel,
        transform
      );
      asyncOperationHandle.Completed += handle =>
      {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        { 
          settingsModel.OpenSettings((GameObject)handle.Result);
          promise.Resolve();
        }
        else
        {
          promise.Reject(handle.OperationException);
        }
      };
      return promise;
    }
  }
}