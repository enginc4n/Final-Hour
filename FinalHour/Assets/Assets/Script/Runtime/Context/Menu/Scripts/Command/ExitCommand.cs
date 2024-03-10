using strange.extensions.command.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Menu.Scripts.Command
{
  public class ExitCommand : EventCommand
  {
    public override void Execute()
    {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }
  }
}
