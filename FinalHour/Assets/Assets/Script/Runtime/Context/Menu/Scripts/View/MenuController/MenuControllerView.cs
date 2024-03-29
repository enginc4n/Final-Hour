﻿using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Menu.Scripts.View.MenuController
{
  public class MenuControllerView : EventView
  {
    public void OnPress()
    {
      dispatcher.Dispatch(MenuControllerEvent.Press);
    }
    
    public void OnSettings()
    {
      dispatcher.Dispatch(MenuControllerEvent.Settings);
    }
  }
}