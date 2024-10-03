using Assets.Script.Runtime.Context.Game.Scripts.Command;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Game.Scripts.View;
using Assets.Script.Runtime.Context.Game.Scripts.View.AudioManager;
using Assets.Script.Runtime.Context.Game.Scripts.View.Background;
using Assets.Script.Runtime.Context.Game.Scripts.View.Border;
using Assets.Script.Runtime.Context.Game.Scripts.View.BulletController;
using Assets.Script.Runtime.Context.Game.Scripts.View.DeadPanel;
using Assets.Script.Runtime.Context.Game.Scripts.View.EnemyController;
using Assets.Script.Runtime.Context.Game.Scripts.View.GameHud;
using Assets.Script.Runtime.Context.Game.Scripts.View.Obstacles;
using Assets.Script.Runtime.Context.Game.Scripts.View.PlayerController;
using Assets.Script.Runtime.Context.Game.Scripts.View.Spawner;
using Assets.Script.Runtime.Context.Menu.Scripts.Config;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.Config
{
  public class GameContext : MVCSContext
  {
    public GameContext(MonoBehaviour view)
      : base(view)
    {
    }

    public GameContext(MonoBehaviour view, ContextStartupFlags flags)
      : base(view, flags)
    {
    }

    protected override void mapBindings()
    {
      OptionsModuleConfigurator.All(this);

      injectionBinder.Bind<IPlayerModel>().To<PlayerModel>().ToSingleton();
      injectionBinder.Bind<ISpeedModel>().To<SpeedModel>().ToSingleton();
      injectionBinder.Bind<IEnemyModel>().To<EnemyModel>().ToSingleton();
      injectionBinder.Bind<ILeaderBoardModel>().To<LeaderBoardModel>().ToSingleton();

      mediationBinder.Bind<GameHudView>().To<GameHudMediator>();
      mediationBinder.Bind<PlayerControllerView>().To<PlayerControllerMediator>();
      mediationBinder.Bind<BulletControllerView>().To<BulletControllerMediator>();
      mediationBinder.Bind<EnemyControllerView>().To<EnemyControllerMediator>();
      mediationBinder.Bind<SpawnerView>().To<SpawnerMediator>();
      mediationBinder.Bind<ObstacleView>().To<ObstacleMediator>();
      mediationBinder.Bind<BackgroundView>().To<BackgroundMediator>();
      mediationBinder.Bind<BorderView>().To<BorderMediator>();
      mediationBinder.Bind<DeadPanelView>().To<DeadPanelMediator>();
      mediationBinder.Bind<AudioManagerView>().To<AudioManagerMediator>();

      commandBinder.Bind(PlayerEvent.FireBullet).To<SpawnBulletCommand>();
    }
  }
}
