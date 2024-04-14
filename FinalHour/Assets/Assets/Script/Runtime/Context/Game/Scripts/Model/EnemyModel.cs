using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.Model
{
  public class EnemyModel : IEnemyModel
  {
    public float spawnPosition { get; set; }
    public float currentPosition { get; set; }
  }
}