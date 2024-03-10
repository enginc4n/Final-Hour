using Assets.Script.Runtime.Context.Game.Scripts.Model;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.Background
{
  public class BackgroundMediator : EventMediator
  {
    [Inject]
    public BackgroundView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }

    private void Update()
    {
      view.ParallaxEffect(playerModel.movementSpeed);
    }
    
  }
}
