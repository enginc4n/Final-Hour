#if UNITY_EDITOR
using Scenes;
using strange.extensions.context.impl;

namespace Test.Scripts
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