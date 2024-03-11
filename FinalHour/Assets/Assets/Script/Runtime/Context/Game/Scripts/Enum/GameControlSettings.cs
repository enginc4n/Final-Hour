

using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.Enum
{
  public class GameControlSettings
  { 
    //GameSpeed
    public const float StartingGameSpeed = 0.5f;
    public const float MaxGameSpeed = 5f;
    public const float GameSpeedUpRate= 5f;
    public const float GameSpeedUpAmount = 0.05f;
 
    public const float startingTime = 60f;
    public const float spawnRate = 2f;
    public const float addTimeAmount = 7f;
    
    //Jump
    public const float jumpSpeed = 7f;
    public const float jumpHeight = 1.5f;

    //Dash
    public const float dashCost = 2f;
    public const float dashDuration = 1f;
    public const float dashRate = 5f;
    public const float dashSpeed = 0.5f;
    
    //Fire
    public const float fireCost = 4f;   
    public const float fireRate = 5f;
    public const float bulletSpeed = 15f;


    //TimeControlSettings
    public const float DefaultGameSpeed = 1f;
    public const float SlowGameSpeed = 0.5f;
    public const float FastGameSpeed = 1.5f;

    //Enemy
    public const float enemySpeed = 2f;
    public static readonly Vector3 EnemySpawnPosition = new Vector3(-18, 1, 0);
  }
}
