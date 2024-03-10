using Assets.Script.Runtime.Context.Game.Scripts.Command;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Game.Scripts.View.BulletController;
using Assets.Script.Runtime.Context.Game.Scripts.View.EnemyController;
using Assets.Script.Runtime.Context.Game.Scripts.View.GameHud;
using Assets.Script.Runtime.Context.Game.Scripts.View.PlayerController;
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
      injectionBinder.Bind<IPlayerModel>().To<PlayerModel>().ToSingleton();

      mediationBinder.Bind<GameHudView>().To<GameHudMediator>();
      mediationBinder.Bind<PlayerControllerView>().To<PlayerControllerMediator>();
      mediationBinder.Bind<BulletControllerView>().To<BulletControllerMediator>();
      mediationBinder.Bind<EnemyControllerView>().To<EnemyControllerMediator>();

      commandBinder.Bind(GameEvent.FireBullet).To<SpawnBulletCommand>();
    }
  }
}
