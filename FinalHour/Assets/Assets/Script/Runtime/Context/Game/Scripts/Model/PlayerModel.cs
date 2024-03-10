using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Assets.Script.Runtime.Context.Game.Scripts.Model
{
  public class PlayerModel : IPlayerModel
  {
    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }
    
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
    }

    public void SlowDownTime()
    {
      currentSpeed = slowDownTimeSpeed;
      dispatcher.Dispatch(GameEvent.SlowDown);
    }
    
    public void SpeedUpTime()
    {
      currentSpeed = speedUpTimeSpeed;
      dispatcher.Dispatch(GameEvent.SpeedUp);
    }

    public void ReturnNormalSpeed()
    {
      currentSpeed = defaultSpeed;
      dispatcher.Dispatch(GameEvent.ReturnNormalSpeed);
    }

    public void Die()
    {
      currentSpeed = 0f;
      dispatcher.Dispatch(GameEvent.Died);
    }
  }
}
