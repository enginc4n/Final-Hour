#if UNITY_EDITOR
using Scripts;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using Test.Scripts;
using UnityEngine;


namespace Scenes
{
  public class GameContext : MVCSContext
  {
    public GameContext(MonoBehaviour view) : base(view)
    {
    }

    public GameContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
    {
    }

    protected override void mapBindings()
    {
      mediationBinder.Bind<GameHudView>().To<GameHudMediator>();
    }
  }
}
#endif