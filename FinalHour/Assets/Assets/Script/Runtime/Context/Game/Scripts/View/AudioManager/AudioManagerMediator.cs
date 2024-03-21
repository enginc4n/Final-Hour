using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.AudioManager
{
  public class AudioManagerMediator : EventMediator
  {
    [Inject]
    public AudioManagerView view { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(PlayerEvent.Died, OnPlayerDied);
      dispatcher.AddListener(PlayerEvent.SlowDown, OnSlowDown);
      dispatcher.AddListener(PlayerEvent.SpeedUp, OnSpeedUp);
      dispatcher.AddListener(PlayerEvent.Collect, OnCollect);
      dispatcher.AddListener(PlayerEvent.CrashObstacle, OnCrashObstacle);
      dispatcher.AddListener(PlayerEvent.FireBullet, OnFireBullet);
      dispatcher.AddListener(PlayerEvent.Jump, OnJump);
      dispatcher.AddListener(PlayerEvent.EnemyStartedMoving, OnEnemyStartedMoving);
      dispatcher.AddListener(GameEvent.Start, OnStart);
      dispatcher.AddListener(GameEvent.Menu, OnMenu);
    }

    private void OnPlayerDied()
    {
    }

    private void OnSlowDown()
    {
    }

    private void OnSpeedUp()
    {
    }

    private void OnCollect()
    {
    }

    private void OnCrashObstacle()
    {
    }

    private void OnFireBullet()
    {
    }

    private void OnJump()
    {
    }

    private void OnEnemyStartedMoving()
    {
    }

    private void OnStart()
    {
    }

    private void OnMenu()
    {
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(PlayerEvent.Died, OnPlayerDied);
      dispatcher.RemoveListener(PlayerEvent.SlowDown, OnSlowDown);
      dispatcher.RemoveListener(PlayerEvent.SpeedUp, OnSpeedUp);
      dispatcher.RemoveListener(PlayerEvent.Collect, OnCollect);
      dispatcher.RemoveListener(PlayerEvent.CrashObstacle, OnCrashObstacle);
      dispatcher.RemoveListener(PlayerEvent.FireBullet, OnFireBullet);
      dispatcher.RemoveListener(PlayerEvent.Jump, OnJump);
      dispatcher.AddListener(PlayerEvent.EnemyStartedMoving, OnEnemyStartedMoving);
      dispatcher.AddListener(GameEvent.Start, OnStart);
      dispatcher.AddListener(GameEvent.Menu, OnMenu);
    }
  }
}
