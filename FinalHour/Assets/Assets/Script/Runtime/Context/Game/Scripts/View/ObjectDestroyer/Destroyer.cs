using UnityEngine;

public class Destroyer : MonoBehaviour
{
  // Start is called before the first frame update
  private void Start()
  {
    Destroy(gameObject, 3f);
  }
}
