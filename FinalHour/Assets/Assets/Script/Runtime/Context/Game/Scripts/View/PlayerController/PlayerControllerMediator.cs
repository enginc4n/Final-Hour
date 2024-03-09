using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using strange.extensions.mediation.impl;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.PlayerController
{
    public enum PlayerControllerEvents
    {
        FireBullet
    }

    public class PlayerControllerMediator : EventMediator
    {
        [Inject]
        public PlayerControllerView view { get; set; }

        public override void OnRegister()
        {
            view.dispatcher.AddListener(PlayerControllerEvents.FireBullet, OnFireBullet);
        }

        private void OnFireBullet()
        {
            dispatcher.Dispatch(GameEvents.FireBullet, view.gameObject.transform.position);
        }

        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(PlayerControllerEvents.FireBullet, OnFireBullet);
        }
    }
}