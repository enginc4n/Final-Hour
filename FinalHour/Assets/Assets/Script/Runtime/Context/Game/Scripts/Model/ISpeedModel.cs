using Assets.Script.Runtime.Context.Game.Scripts.Enum;

namespace Assets.Script.Runtime.Context.Game.Scripts.Model
{
  public interface ISpeedModel
  {
    public SpeedState speedState { get; }
    void SlowDownTime();
    void SpeedUpTime();
    void ReturnNormalSpeed();
    void Pause();
    void Continue();
  }
}
