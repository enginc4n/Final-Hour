using UnityEngine;


namespace Assets.Script.Runtime.Context.Game.Scripts.Model
{
  public interface IEnemyModel
  {
    float spawnPosition { get; set; }
    float currentPosition { get; set; }
  }
}