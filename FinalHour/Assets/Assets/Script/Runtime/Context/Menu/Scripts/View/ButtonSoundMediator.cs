using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using strange.extensions.mediation.impl;
using UnityEngine.EventSystems;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View
{
  public enum ButtonSoundEvent
  {
    Hover,
    Click
  }
  
  public class ButtonSoundMediator : EventMediator
  {
    [Inject]
    public ButtonSoundView view { get; set; }
    
    public override void OnRegister()
    {
      view.dispatcher.AddListener(ButtonSoundEvent.Hover, OnHover);
      view.dispatcher.AddListener(ButtonSoundEvent.Click, OnClick);
    }
    
    public void OnHover( )
    {
      dispatcher.Dispatch(GameEvent.Hover);
    }
    
    public void OnClick()
    {
      dispatcher.Dispatch(GameEvent.Click);
    }
    
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(ButtonSoundEvent.Hover, OnHover);
      view.dispatcher.RemoveListener(ButtonSoundEvent.Click, OnClick);
    }
  }
}