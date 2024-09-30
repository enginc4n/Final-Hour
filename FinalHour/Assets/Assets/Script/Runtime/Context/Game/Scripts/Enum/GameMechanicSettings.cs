﻿
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.Enum
{
  public static class GameMechanicSettings
  { 
    public const float StartingTime = 60f;
    public static readonly Vector3 PlayerSpawnPosition = new Vector3(275, 330, 0f);
    
    //GameSpeed
    public const float StartingGameSpeed = 0.2f;
    public const float MaxGameSpeed = 1f;
    public const float GameSpeedUpRate= 1f;
    public const float GameSpeedUpAmount = 0.002f;
    
    //Spawns
    public const float SpawnInterval = 1f; //higher number puts more time between spawns 
    public const float CollectibleTimeAmount = 5f;
    public const float CrashPunishment = 1f;
    
    //Jump
    public const float JumpSpeed = 10f;
    public const float JumpHeight = 1.6f;

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
    public const float SlowTimeGain = 2f; // x amount second gain at every second

    //Enemy
    public const float EnemySpeed = 150f; //+x per second
    public const float EnemyTimeToCatchFromStart = 3f;
    public const float EnemyCatchTimeFromMax = 5f;
  }
}
