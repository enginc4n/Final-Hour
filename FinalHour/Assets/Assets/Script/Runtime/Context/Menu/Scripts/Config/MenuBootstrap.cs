using Assets.Script.Runtime.Context.Game.Scripts.Config;
using strange.extensions.context.impl;

namespace Assets.Script.Runtime.Context.Menu.Scripts.Config
{
    public class MenuBootstrap : ContextView
    {
        private void Awake()
        {
            //Instantiate the context, passing it this instance.
            context = new MenuContext(this);
        }
    }
}
