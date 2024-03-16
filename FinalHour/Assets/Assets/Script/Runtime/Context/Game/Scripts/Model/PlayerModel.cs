using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.Model
{
  public class PlayerModel : IPlayerModel
  {
    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }
    
    public bool isAlive { get; set; }
    public float currentGameSpeed { get; set; }
    public int score { get; set; }
    public float position { get; set; }
    public bool isDashing { get; set; }
    public float remainingTime { get; set; }

    [PostConstruct]
    public void OnPostConstruct()
    {
      currentGameSpeed = GameControlSettings.StartingGameSpeed;
      score = 0;
      remainingTime = GameControlSettings.StartingTime;

      isAlive = true;
    }
    
    public void ChangeRemainingTime(float value)
    {
      remainingTime += value;

      if (remainingTime <= 0)
      {
        remainingTime = 0f;
        Die();
      }

      dispatcher.Dispatch(PlayerEvent.UpdateRemainingTime);
    }

    public void ChangeScore(int value)
    {
      if (!isAlive)
      {
        return;
      }

      score += value;
    }

    public void Die()
    {
      isAlive = false;
      dispatcher.Dispatch(PlayerEvent.Died);
      dispatcher.Dispatch(SoundEvent.DeathSound);
    }
  }
}
