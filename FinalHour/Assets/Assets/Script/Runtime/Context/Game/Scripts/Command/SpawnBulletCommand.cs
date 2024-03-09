using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Scripts.Runtime.Modules.Core.PromiseTool;
using strange.extensions.command.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets.Script.Runtime.Context.Game.Scripts.Command
{
    public class SpawnBulletCommand : EventCommand
    {
        public override void Execute()
        {
            Vector3 position = (Vector3)evt.data;
            SpawnObject(position);
        }

        private IPromise SpawnObject(Vector3 position)
        {
            Promise promise = new();

            AsyncOperationHandle asyncOperationHandle = Addressables.InstantiateAsync(
                AddressableKeys.Bullet,
                position,
                Quaternion.identity
            );
            asyncOperationHandle.Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
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
