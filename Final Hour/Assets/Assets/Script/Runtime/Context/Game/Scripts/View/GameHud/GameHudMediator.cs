using strange.extensions.mediation.impl;
using UnityEngine;

namespace Test.Scripts
{
  public class GameHudMediator : EventMediator
  {
    [Inject]
    public GameHudView view { get; set; }
    
    
    }
  }
