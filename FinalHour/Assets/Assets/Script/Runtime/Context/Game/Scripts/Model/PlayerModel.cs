using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Assets.Script.Runtime.Context.Game.Scripts.Model
{
  public class PlayerModel : IPlayerModel
  {
    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }

    public SpeedState speedState { get; set; }
    public bool isAlive { get; set; }
    public int score { get; set; }
    public float position { get; set; }
    public float slowDownTimeSpeed { get; set; }
    public float speedUpTimeSpeed { get; set; }
    public bool isDashing { get; set; }
    public float remainingTime { get; set; }
    public float currentSpeed { get; set; }
    public float defaultSpeed { get; set; }
    public float timerCountSpeed => 1 / currentSpeed;
    public float bulletSpeed => currentSpeed * GameControlSettings.bulletSpeed;
    public float movementSpeed => currentSpeed * GameControlSettings.movementSpeed;
    public float jumpSpeed => currentSpeed * GameControlSettings.jumpSpeed;
    public float enemySpeed { get; set; }

    [PostConstruct]
    public void OnPostConstruct()
    {
      score = 0;
      speedState = SpeedState.Normal;
      slowDownTimeSpeed = GameControlSettings.slowDownTimeSpeed;
      speedUpTimeSpeed = GameControlSettings.speedUpTimeSpeed;
      remainingTime = GameControlSettings.startingTime;
      enemySpeed = GameControlSettings.enemySpeed;
      defaultSpeed = GameControlSettings.defaultSpeed;
      currentSpeed = defaultSpeed;

      isAlive = true;
    }

    public void SlowDownTime()
    {
      if (!isAlive)
      {
        return;
      }

      speedState = SpeedState.Slow;
      currentSpeed = slowDownTimeSpeed;
      dispatcher.Dispatch(PlayerEvent.SlowDown);
    }

    public void SpeedUpTime()
    {
      if (!isAlive)
      {
        return;
      }

      speedState = SpeedState.Fast;
      currentSpeed = speedUpTimeSpeed;
      dispatcher.Dispatch(PlayerEvent.SpeedUp);
    }

    public void ReturnNormalSpeed()
    {
      if (!isAlive)
      {
        return;
      }

      speedState = SpeedState.Normal;
      currentSpeed = defaultSpeed;
      dispatcher.Dispatch(PlayerEvent.ReturnNormalSpeed);
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
      dispatcher.Dispatch(PlayerEvent.Died);
      dispatcher.Dispatch(SoundEvents.DeathSound);
    }

    public void Respawn()
    {
      OnPostConstruct();
      dispatcher.Dispatch(PlayerEvent.Play);
    }
  }
}
