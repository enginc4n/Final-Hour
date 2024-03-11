using Assets.Script.Runtime.Context.Game.Scripts.View.AudioManager;
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
  public class MenuContext : MVCSContext
  {
    public MenuContext(MonoBehaviour view)
      : base(view)
    {
    }

    public MenuContext(MonoBehaviour view, ContextStartupFlags flags)
      : base(view, flags)
    {
    }

    protected override void mapBindings()
    {
      SettingsModuleConfigurator.All(this);
      
      injectionBinder.Bind<ISpeedModel>().To<SpeedModel>().ToSingleton();
      
      mediationBinder.Bind<AudioManagerView>().To<AudioManagerMediator>();
      mediationBinder.Bind<MenuControllerView>().To<MenuControllerMediator>();
    }
  }
}
