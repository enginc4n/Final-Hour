
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.Enum
{
  public class GameControlSettings
  { 
    public const float StartingTime = 60f;
    
    //GameSpeed
    public const float StartingGameSpeed = 0.5f;
    public const float MaxGameSpeed = 5f;
    public const float GameSpeedUpRate= 5f;
    public const float GameSpeedUpAmount = 0.05f;
    
    //Spawns
    public const float SpawnRate = 2f;
    public const float CollectibleTimeAmount = 15f;
    
    //Jump
    public const float JumpSpeed = 7f;
    public const float JumpHeight = 1.5f;

    //Dash
    public const float DashCost = 10f;
    public const float DashDuration = 1f;
    public const float DashCooldown = 5f;
    public const float DashSpeed = 0.5f;
    
    //Fire
    public const float FireCost = 5f;   
    public const float FireCooldown = 0.5f;
    public const float BulletSpeed = 15f;


    //TimeControlSettings
    public const float DefaultGameSpeed = 1f;
    public const float SlowGameSpeed = 0.5f;
    public const float FastGameSpeed = 1.5f;

    //Enemy
    public const float EnemySpeed = 2f;
    public static readonly Vector3 EnemySpawnPosition = new Vector3(-18, 1, 0);
  }
}
