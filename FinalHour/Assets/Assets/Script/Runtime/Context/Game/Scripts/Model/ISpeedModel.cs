using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Menu.Scripts.Model
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