using Assets.Script.Runtime.Context.Game.Scripts.Enum;

namespace Assets.Script.Runtime.Context.Game.Scripts.Model
{
  public interface IPlayerModel
  {
    bool isAlive { get; set; }
    float currentGameSpeed { get; set; }
    float position { get; set; }
    int score { get; set; }
    bool isDashing { get; set; }
    float remainingTime { get; set; }
    void Die();
    void ChangeScore(int value);
    void ChangeRemainingTime(float value);
    void ChangeGameSpeed(float value);
  }
}
