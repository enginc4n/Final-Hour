using System;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.PlayerController
{
  public class PlayerControllerView : EventView
  {
    [Header("Fire Settings")]
    [SerializeField]
    private float fireRate;

    [SerializeField]
    private float fireCost;

    [Header("Dash Settings")]
    [SerializeField]
    private float dashRate;

    [SerializeField]
    private float dashCost;

    public float dashDuration;

    [Header("References")]
    public Rigidbody2D playerRigidboyd2d;

    public BoxCollider2D playerBodyCollider;
    public CapsuleCollider2D playerCrouchCollider;
    public SpriteRenderer spriteRenderer;
    public PlayerInput playerInput;
    public Animator animator;

    private PlayerInputActions playerInputActions;
    private InputActionMap inputActionMap;
    private InputAction crouch;
    private InputAction slowTime;
    private InputAction speedUpTime;
    private float dashCoolDown;
    private float fireCoolDown;

    private void Awake()
    {
      playerInputActions = new PlayerInputActions();
      inputActionMap = playerInput.actions.FindActionMap("Player");
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
      inputActionMap.Enable();
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

      animator.SetBool("isJumping", true);
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

      animator.SetBool("isCrouch", true);
      dispatcher.Dispatch(PlayerControllerEvents.Crouch);
    }

    private void CrouchFinished(InputAction.CallbackContext context)
    {
      animator.SetBool("isCrouch", false);
      dispatcher.Dispatch(PlayerControllerEvents.Crouch);
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

    public void SetActionMapState(bool enable)
    {
      if (enable)
      {
        inputActionMap.Enable();
      } 
      else
      {
        inputActionMap.Disable();
        animator.SetTrigger("dead");
      }
    }

    public void ChangeColor(Color color)
    {
      spriteRenderer.color = color;
    }

    public void SetColliders()
    {
      playerBodyCollider.enabled = !playerBodyCollider.enabled;
      playerCrouchCollider.enabled = !playerCrouchCollider.enabled;
    }

    public void ChangeGravityScale(float scale)
    {
      playerRigidboyd2d.gravityScale = scale;
    }

    public void ChangeAnimationSpeed(float speed)
    {
      animator.SetFloat("speed", speed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
      if (other.gameObject.layer==LayerMask.NameToLayer("Ground"))
      {
        animator.SetBool("isJumping", false);
      }
    }
  }
}
