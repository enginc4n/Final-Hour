using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.PlayerController
{
  public class PlayerControllerView : EventView
  {
    [SerializeField]
    private float fireRate;

    [SerializeField]
    private float dashRate;

    public float dashDuration;

    public Rigidbody2D playerRigidboyd2d;

    public CapsuleCollider2D playerBodyCollider;
    private BoxCollider2D playerCrouchCollider;
    private PlayerInputActions playerInputActions;
    private InputAction crouch;
    private InputAction fire;
    private InputAction slowTime;
    private InputAction speedUpTime;
    private SpriteRenderer spriteRenderer;

    private float dashCoolDown;
    private float fireCoolDown;

    private void Awake()
    {
      playerBodyCollider = GetComponent<CapsuleCollider2D>();
      playerCrouchCollider = GetComponent<BoxCollider2D>();
      spriteRenderer = GetComponent<SpriteRenderer>();
      playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
      crouch = playerInputActions.Player.Crouch;
      crouch.Enable();
      crouch.canceled += CrouchFinished;
      slowTime = playerInputActions.Player.SlowTime;
      slowTime.Enable();
      slowTime.canceled += ReturnNormalSpeed;
      speedUpTime = playerInputActions.Player.SpeedUpTime;
      speedUpTime.Enable();
      speedUpTime.canceled += ReturnNormalSpeed;
    }

    private void OnJump(InputValue inputValue)
    {
      if (!inputValue.isPressed)
      {
        return;
      }

      if (!playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
      {
        return;
      }

      dispatcher.Dispatch(PlayerControllerEvents.Jump);
    }

    private void OnCrouch(InputValue inputValue)
    {
      if (!inputValue.isPressed)
      {
        return;
      }

      if (!playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
      {
        return;
      }

      SetColliders();
    }

    private void CrouchFinished(InputAction.CallbackContext context)
    {
      SetColliders();
    }

    private void OnFire(InputValue inputValue)
    {
      if (!inputValue.isPressed)
      {
        return;
      }

      if (Time.time < fireCoolDown)
      {
        return;
      }

      fireCoolDown = Time.time + 1f / fireRate;
      dispatcher.Dispatch(PlayerControllerEvents.FireBulletAction);
    }

    private void SetColliders()
    {
      playerBodyCollider.enabled = !playerBodyCollider.enabled;
      playerCrouchCollider.enabled = !playerCrouchCollider.enabled;
    }

    private void OnDash(InputValue inputValue)
    {
      if (!inputValue.isPressed)
      {
        return;
      }

      if (Time.time < dashCoolDown)
      {
        return;
      }

      dashCoolDown = Time.time + 1f / dashRate;
      dispatcher.Dispatch(PlayerControllerEvents.Dash);
    }

    private void OnSpeedUpTime()
    {
      dispatcher.Dispatch(PlayerControllerEvents.SpeedUpTime);
    }

    private void OnSlowTime()
    {
      dispatcher.Dispatch(PlayerControllerEvents.SlowDownTime);
    }

    public void ChangeColor(Color color)
    {
      spriteRenderer.color = color;
    }

    private void ReturnNormalSpeed(InputAction.CallbackContext context)
    {
      dispatcher.Dispatch(PlayerControllerEvents.ReturnNormalSpeed);
    }

    private void OnDisable()
    {
      crouch.Disable();
      slowTime.Disable();
      speedUpTime.Disable();
    }
  }
}
