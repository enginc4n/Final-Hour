using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Scripts.Runtime.Modules.Core.PromiseTool;
using strange.extensions.command.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets.Script.Runtime.Context.Game.Scripts.Command
{
  public class StartCommand : EventCommand
  {
    [Inject]
    public IPlayerModel playerModel { get; set; }

    public override void Execute()
    {
      playerModel.Respawn();
      
      dispatcher.Dispatch(PlayerEvent.Play);
    }
    
  }
}
