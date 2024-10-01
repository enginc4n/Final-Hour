using strange.extensions.command.impl;
using UnityEngine.SceneManagement;

namespace Assets.Script.Runtime.Context.Menu.Scripts.Command
{
  public class StartCommand : EventCommand
  {
    public override void Execute()
    {
      SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
  }
}
