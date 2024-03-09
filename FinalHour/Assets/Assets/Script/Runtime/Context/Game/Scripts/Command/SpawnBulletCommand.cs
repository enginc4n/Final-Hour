﻿using Assets.Script.Runtime.Context.Game.Scripts.Enum;
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
            Transform transform = (Transform)evt.data;
            SpawnObject(transform);
        }

        private IPromise SpawnObject(Transform transform)
        {
            Promise promise = new();

            AsyncOperationHandle asyncOperationHandle = Addressables.InstantiateAsync(
                AddressableKeys.Bullet,
                transform.position,
                Quaternion.identity,
                transform
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
