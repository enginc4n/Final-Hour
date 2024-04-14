
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.Enum
{
  public class GameControlSettings
  { 
    public const float StartingTime = 60f;
    public static readonly Vector3 PlayerSpawnPosition = new Vector3(275, 330, 0f);
    
    //GameSpeed
    public const float StartingGameSpeed = 0.65f;
    public const float MaxGameSpeed = 1.25f;
    public const float GameSpeedUpRate= 5f;
    public const float GameSpeedUpAmount = 0.05f;
    
    //Spawns
    public const float SpawnRate = 2f;
    public const float CollectibleTimeAmount = 5f;
    public const float ObstacleSpeed = 20f;
    public const float ObstaclePunish = 1f;
    
    //Jump
    public const float JumpSpeed = 14f;
    public const float JumpHeight = 1.5f;

    //Dash
    public const float DashCost = 2f;
    public const float DashDuration = 1f;
    public const float DashCooldown = 5f;
    public const float DashSpeed = 1.5f;
    
    //Fire
    public const float FireCost = 2.5f;   
    public const float FireCooldown = 0.5f;
    public const float BulletSpeed = 15f;

    //TimeControlSettings
    public const float DefaultGameSpeed = 1f;
    public const float SlowGameSpeed = 0.5f;
    public const float FastGameSpeed = 1.5f;
    public const float SlowTimeGain = 2.5f;

    //Enemy
    public const float EnemySpeed = 2f;
    public const int EnemyCatchFromStart = 3;
    public const int EnemyCatchFromMax = 5;
  }
}
