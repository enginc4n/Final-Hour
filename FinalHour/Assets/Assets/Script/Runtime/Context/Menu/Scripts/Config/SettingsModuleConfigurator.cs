using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Command;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.View.CreditsPanel;
using Assets.Script.Runtime.Context.Menu.Scripts.View.InstructionsPanel;
using Assets.Script.Runtime.Context.Menu.Scripts.View.OptionsPanel;
using Assets.Script.Runtime.Context.Menu.Scripts.View.SettingsPanel;
using Assets.Script.Runtime.Context.Menu.Scripts.View.SoundSettingsPanel;
using strange.extensions.context.impl;

namespace Assets.Script.Runtime.Context.Menu.Scripts.Config
{
  public class OptionsModuleConfigurator
  {
    public static void All(MVCSContext context)
    {
      context.injectionBinder.Bind<IUIModel>().To<UIModel>().ToSingleton();
      context.injectionBinder.Bind<IAudioModel>().To<AudioModel>().ToSingleton();

      context.mediationBinder.Bind<OptionsPanelView>().To<OptionsPanelMediator>();
      context.mediationBinder.Bind<InstructionsPanelView>().To<InstructionsPanelMediator>();
      context.mediationBinder.Bind<CreditsPanelView>().To<CreditsPanelMediator>();
      context.mediationBinder.Bind<SettingsPanelView>().To<SettingsPanelMediator>();

      context.commandBinder.Bind(GameEvent.OptionsPanel).To<OpenOptionsCommand>();
      context.commandBinder.Bind(GameEvent.Exit).To<ExitCommand>();
      context.commandBinder.Bind(GameEvent.Start).To<StartCommand>();
    }
  }
}
