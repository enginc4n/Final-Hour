using Assets.Script.Runtime.Context.Game.Scripts.Config;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Scripts.Runtime.Modules.Core.PromiseTool;
using strange.extensions.command.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets.Script.Runtime.Context.Game.Scripts.Command
{
  public class SpawnBulletCommand : EventCommand
  {
    [Inject]
    public IPlayerModel playerModel { get; set; }

    public override void Execute()
    {
      if (!playerModel.isAlive)
      {
        return;
      }

      Transform transform = (Transform)evt.data;
      GameObject bullet = ObjectPool.instance.GetPooledBullet();
      bullet.SetActive(true);
      bullet.transform.SetParent(transform.parent);
      bullet.transform.position = transform.position;
    }
  }
}
