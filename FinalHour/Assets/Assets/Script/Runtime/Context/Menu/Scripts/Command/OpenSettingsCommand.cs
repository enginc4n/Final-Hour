using System.Linq;
using Assets.Script.Runtime.Context.Game.Scripts.Config;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
using strange.extensions.command.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Menu.Scripts.Command
{
  public class OpenSettingsCommand : EventCommand
  {
    [Inject]
    public IUIModel uiModel { get; set; }
    
    [Inject]
    public ISpeedModel speedModel { get; set; }
    public override void Execute()
    {
      if (uiModel.openPanels.All(obj => obj.Value != PanelKeys.OptionsPanel)) 
      {
        Transform layer = (Transform)evt.data;
        speedModel.Pause();

        uiModel.OpenPanel(PanelKeys.OptionsPanel, layer);
      }
      else
      {
       uiModel.ClosePanel(PanelKeys.OptionsPanel);
       
       speedModel.Continue();
      }
    }
  }
}