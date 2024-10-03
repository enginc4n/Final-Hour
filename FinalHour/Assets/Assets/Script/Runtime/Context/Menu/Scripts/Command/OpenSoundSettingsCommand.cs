using System.Linq;
using Assets.Script.Runtime.Context.Game.Scripts.Config;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
using strange.extensions.command.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Menu.Scripts.Command
{
  public class OpenGameSettingsCommand : EventCommand
  {
    [Inject]
    public IUIModel uiModel { get; set; }
    
    public override void Execute()
    {
      Transform layer = (Transform)evt.data;
      
      uiModel.OpenPanel(PanelKeys.SettingsPanel, layer);
    }
  }
}
