using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Assets.Script.Runtime.Context.Game.Scripts.Model
{
  public class PlayerModel : IPlayerModel
  {
    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }

    public bool isAlive { get; set; }
    public int score { get; set; }
    public float slowDownTimeSpeed { get; set; }
    public float speedUpTimeSpeed { get; set; }
    public bool isDashing { get; set; }
    public float remainingTime { get; set; }
    public float currentSpeed { get; set; }
    public float defaultSpeed { get; set; }
    public float timerCountSpeed => 1 / currentSpeed;
    public float bulletSpeed => currentSpeed * 15f;
    public float movementSpeed => currentSpeed * 0.5f;
    public float jumpSpeed => currentSpeed * 10f;
    public float enemySpeed { get; set; }

    [PostConstruct]
    public void OnPostConstruct()
    {
      slowDownTimeSpeed = 0.5f;
      speedUpTimeSpeed = 2f;

      defaultSpeed = 1f;
      currentSpeed = defaultSpeed;
      remainingTime = 60f;
      enemySpeed = 2;
      isAlive = true;
    }

    public void SlowDownTime()
    {
      if (!isAlive)
      {
        return;
      }

      currentSpeed = slowDownTimeSpeed;
      dispatcher.Dispatch(GameEvent.SlowDown);
    }

    public void SpeedUpTime()
    {
      if (!isAlive)
      {
        return;
      }

      currentSpeed = speedUpTimeSpeed;
      dispatcher.Dispatch(GameEvent.SpeedUp);
    }

    public void ReturnNormalSpeed()
    {
      if (!isAlive)
      {
        return;
      }

      currentSpeed = defaultSpeed;
      dispatcher.Dispatch(GameEvent.ReturnNormalSpeed);
    }

    public void ChangeRemainingTime(float value)
    {
      if (!isAlive)
      {
        return;
      }

      remainingTime += value;
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
      currentSpeed = 0f;
      isAlive = false;
      dispatcher.Dispatch(GameEvent.Died);
    }
  }
}
