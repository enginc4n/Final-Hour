#if UNITY_EDITOR
using strange.extensions.context.impl;

namespace Assets.Script.Runtime.Context.Game.Scripts.Config
{
    public class GameBootstrap : ContextView
    {
        
        private void Awake()
        {
            //Instantiate the context, passing it this instance.
            context = new GameContext(this);
        }
    }
}

#endif