using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.GameHud
{
  public class GameHudMediator : EventMediator
  {
    [Inject]
    public GameHudView view { get; set; }
    
    
    }
  }
