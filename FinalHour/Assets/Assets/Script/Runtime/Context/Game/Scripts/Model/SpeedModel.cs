using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script.Runtime.Context.Game.Scripts.Model
{
  public class SpeedModel : ISpeedModel
  {
    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }

    public float gameDistance { get; set; }

    public SpeedState speedState { get; set; }

    [PostConstruct]
    public void OnPostConstruct()
    {
      speedState = SpeedState.Normal;
    }

    public void SlowDownTime()
    {
      Time.timeScale = GameMechanicSettings.SlowGameSpeed;
      speedState = SpeedState.Slow;

      dispatcher.Dispatch(PlayerEvent.SlowDown);
    }

    public void SpeedUpTime()
    {
      Time.timeScale = GameMechanicSettings.FastGameSpeed;
      speedState = SpeedState.Fast;

      dispatcher.Dispatch(PlayerEvent.SpeedUp);
    }

    public void ReturnNormalSpeed()
    {
      Time.timeScale = GameMechanicSettings.DefaultGameSpeed;
      speedState = SpeedState.Normal;

      dispatcher.Dispatch(PlayerEvent.ReturnNormalSpeed);
    }

    public void Pause()
    {
      if (SceneManager.GetActiveScene().buildIndex == 1)
      {
        Time.timeScale = 0;
      }

      dispatcher.Dispatch(GameEvent.Pause);
    }

    public void Continue()
    {
      Time.timeScale = GameMechanicSettings.DefaultGameSpeed;
      speedState = SpeedState.Normal;

      dispatcher.Dispatch(GameEvent.Continue);
    }
  }
}
