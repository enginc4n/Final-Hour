using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script.Runtime.Context.Game.Scripts.Config
{
  public class ObjectPool : MonoBehaviour
  {
    public static ObjectPool instance;

    private List<GameObject> pooledBullets = new();

    private int amountToPoolBullet = 10;

    [SerializeField]
    private GameObject bulletPrefab;

    private void Awake()
    {
      if (instance == null)
      {
        instance = this;
      }
    }

    private void Start()
    {
      if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
      {
        for (int i = 0; i < amountToPoolBullet; i++)
        {
          GameObject go = Instantiate(bulletPrefab);
          go.SetActive(false);
          pooledBullets.Add(go);
        }
      }
    }

    public GameObject GetPooledBullet()
    {
      for (int i = 0; i < pooledBullets.Count; i++)
      {
        if (!pooledBullets[i].activeInHierarchy)
        {
          return pooledBullets[i];
        }
      }

      return null;
    }
  }
}
