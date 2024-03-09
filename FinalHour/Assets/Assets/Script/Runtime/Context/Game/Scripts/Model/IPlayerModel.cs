namespace Assets.Script.Runtime.Context.Game.Scripts.Model
{
  public interface IPlayerModel
  {
    bool isDashing { get; set; }
    float remainingTime { get; set; }
    float currentPlayerSpeed { get; set; }
    float defaultSpeed { get; }
  }
}
