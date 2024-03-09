namespace Assets.Script.Runtime.Context.Game.Scripts.Model
{
  public class PlayerModel : IPlayerModel
  {
    public bool isDashing { get; set; }
    public float remainingTime { get; set; }
    public float currentPlayerSpeed { get; set; }
    public float defaultSpeed { get; set; }

    [PostConstruct]
    public void OnPostConstruct()
    {
      defaultSpeed = 1f;
      currentPlayerSpeed = defaultSpeed;
      remainingTime = 100f;
    }
  }
}
