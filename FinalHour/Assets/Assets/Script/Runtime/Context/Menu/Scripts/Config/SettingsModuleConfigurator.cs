using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Command;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.View.SettingsPanel;
using Assets.Script.Runtime.Context.Menu.Scripts.View.SoundSettingsPanel;
using strange.extensions.context.impl;

namespace Assets.Script.Runtime.Context.Menu.Scripts.Config
{
  public class SettingsModuleConfigurator
  {
    public static void All(MVCSContext context)
    {
      context.injectionBinder.Bind<ISettingsModel>().To<SettingsModel>().ToSingleton();
      context.injectionBinder.Bind<IAudioModel>().To<AudioModel>().ToSingleton();

      context.mediationBinder.Bind<SettingsPanelView>().To<SettingsPanelMediator>();
      context.mediationBinder.Bind<SoundSettingsView>().To<SoundSettingsMediator>();

      context.commandBinder.Bind(GameEvent.SettingsPanel).To<OpenSettingsCommand>();
      context.commandBinder.Bind(GameEvent.SoundSettingsPanel).To<OpenSoundSettingsCommand>();
      context.commandBinder.Bind(GameEvent.Exit).To<ExitCommand>();
      context.commandBinder.Bind(GameEvent.Start).To<StartCommand>();
    }
  }
}
