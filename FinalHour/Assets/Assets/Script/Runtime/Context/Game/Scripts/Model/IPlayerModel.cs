namespace Assets.Script.Runtime.Context.Game.Scripts.Model
{
  public interface IPlayerModel
  {
    int score { get; set; }
    float slowDownTimeSpeed { get; }
    float speedUpTimeSpeed { get; }
    bool isDashing { get; set; }
    float remainingTime { get; set; }
    float currentSpeed { get; set; }
    float defaultSpeed { get; }
    float timerCountSpeed { get; }
    float bulletSpeed { get; }
    float movementSpeed { get; }
    float jumpSpeed { get; }
    float enemySpeed { get; }
    void SlowDownTime();
    void SpeedUpTime();
    void ReturnNormalSpeed();
    void Die();
  }
}
