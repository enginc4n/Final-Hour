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
    public float currentGameSpeed { get; set; }
    public float trueGameSpeed { get; set; }
    public int score { get; set; }
    public float position { get; set; }
    public bool isDashing { get; set; }
    public bool isCrouching { get; set; }
    public float remainingTime { get; set; }
    
    public bool tutorialActive { get; set; }

    [PostConstruct]
    public void OnPostConstruct()
    {
      currentGameSpeed = GameMechanicSettings.StartingGameSpeed;
      trueGameSpeed = GameMechanicSettings.StartingGameSpeed;
      score = 0;
      remainingTime = GameMechanicSettings.StartingTime;

      isAlive = true;
    }

    public void ChangeScore(int value)
    {
      if (!isAlive)
      {
        return;
      }

      score += value;
    }
    
    public void ChangeRemainingTime(float value)
    {
      remainingTime += value;

      if (remainingTime <= 0)
      {
        remainingTime = 0f;
        Die();
      }
    }
    
    public void ChangeTrueGameSpeed(float value)
    {
      currentGameSpeed += value;
      trueGameSpeed += value;
    }
    
    public void ChangeGameSpeed(float value)
    {
      currentGameSpeed += value;
    }
    
    public void Die()
    {
      isAlive = false;
      currentGameSpeed = GameMechanicSettings.StartingGameSpeed;
      trueGameSpeed = GameMechanicSettings.StartingGameSpeed;
      dispatcher.Dispatch(PlayerEvent.Died);
    }
  }
}
