using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Menu.Scripts.Model
{
  public class SettingsModel : ISettingsModel
  {
    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }
    
    public bool isOpen { get; set;  }

    private GameObject openPanel;

    public void OpenSettings(GameObject panel)
    {
      isOpen = true;
      openPanel = panel;
    }

    public void CloseSettings()
    {
      openPanel.SetActive(false);
      isOpen = false;
      openPanel = null;
    }
  }
}