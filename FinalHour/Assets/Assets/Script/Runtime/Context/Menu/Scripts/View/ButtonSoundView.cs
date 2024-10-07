using strange.extensions.mediation.impl;
using UnityEngine.EventSystems;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View
{
  public class ButtonSoundView : EventView, IPointerEnterHandler, IPointerClickHandler
  
  {
    public void OnPointerEnter(PointerEventData eventData)
    {
      dispatcher.Dispatch(ButtonSoundEvent.Hover);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
      dispatcher.Dispatch(ButtonSoundEvent.Click);
    }
  }
}