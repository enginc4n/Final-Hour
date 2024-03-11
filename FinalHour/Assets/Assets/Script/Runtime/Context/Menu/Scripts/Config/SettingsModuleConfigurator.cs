using Assets.Script.Runtime.Context.Menu.Scripts.Command;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.View.MenuController;
using Assets.Script.Runtime.Context.Menu.Scripts.View.SettingsPanel;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Menu.Scripts.Config
{
  public class SettingsModuleConfigurator
  {
    public static void All(MVCSContext context)
    {
      context.injectionBinder.Bind<ISettingsModel>().To<SettingsModel>().ToSingleton();
      
      context.mediationBinder.Bind<SettingsPanelView>().To<SettingsPanelMediator>();
      
      context.commandBinder.Bind(GameEvent.SettingsPanel).To<OpenSettingsCommand>();
      context.commandBinder.Bind(GameEvent.Exit).To<ExitCommand>();
      context.commandBinder.Bind(GameEvent.Start).To<StartCommand>();
    }
  }
}
