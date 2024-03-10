using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.Spawner
{
  public class SpawnerMediator : EventMediator
  {
    [Inject]
    public SpawnerView view { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(GameEvent.Died, OnDied);
    }

    private void OnDied()
    {
      view.StopSpawn();
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(GameEvent.Died, OnDied);
    }
  }
}
