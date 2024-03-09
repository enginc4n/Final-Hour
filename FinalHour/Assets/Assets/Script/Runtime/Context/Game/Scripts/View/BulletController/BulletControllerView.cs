using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.BulletController
{
  public class BulletControllerView : EventView
  {
    private Rigidbody2D bulletRigidBody;

    [Header("Settings")]
    [SerializeField]
    private float bulletSpeed = 15f;

    private void Awake()
    {
      bulletRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
      bulletRigidBody.velocity = new Vector2(bulletSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      Destroy(gameObject);
    }
  }
}
