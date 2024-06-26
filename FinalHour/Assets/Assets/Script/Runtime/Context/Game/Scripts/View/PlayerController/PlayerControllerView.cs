using System.Collections;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.PlayerController
{
  public class PlayerControllerView : EventView
  {
    public Rigidbody2D playerRigidbody2D;
    public CircleCollider2D playerBodyCollider;
    public CapsuleCollider2D playerCrouchCollider;
    public SpriteRenderer spriteRenderer;
    public PlayerInput playerInput;
    public Animator animator;

    private PlayerInputActions playerInputActions;
    public InputActionMap inputActionMap;
    private InputAction crouch;
    private InputAction slowDownTime;
    private InputAction speedUpTime;
    private float dashCoolDown;
    private float fireCoolDown;

    [HideInInspector]
    public bool isDashReady = true;
    
    [HideInInspector]
    public bool isFireReady = true;

    protected override void Awake()
    {
      playerInputActions = new PlayerInputActions();
      inputActionMap = playerInput.actions.FindActionMap("Player");
    }

    public void EnableInputs()
    {
      crouch = playerInputActions.Player.Crouch;
      crouch.Enable();
      crouch.canceled += CrouchFinished;
      slowDownTime = playerInputActions.Player.SlowTime;
      slowDownTime.Enable();
      slowDownTime.canceled += ReturnNormalSpeed;
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
      if (!playerCrouchCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
      {
        return;
      }

      animator.SetBool("isCrouch", false);
      dispatcher.Dispatch(PlayerControllerEvents.Crouch);
    }

    private void OnFire(InputValue inputValue)
    {
      if (!inputValue.isPressed)
      {
        return;
      }

      if (!isFireReady)
      {
        return;
      }

      dispatcher.Dispatch(PlayerControllerEvents.FireBulletAction);
    }
    
    public IEnumerator FireCooldown()
    {
      yield return new WaitForSeconds(GameControlSettings.FireCooldown);
      
      isFireReady = true;
    }

    private void OnDash(InputValue inputValue)
    {
      if (!inputValue.isPressed)
      {
        return;
      }

      if (!isDashReady)
      {
        return;
      }
      
      dispatcher.Dispatch(PlayerControllerEvents.Dash);
    }
    
    public IEnumerator DashCooldown()
    {
      yield return new WaitForSeconds(GameControlSettings.DashCooldown);
      
      isDashReady = true;
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

    public void DisableInputs()
    {
      crouch.canceled -= CrouchFinished;
      crouch.Disable();
      slowDownTime.canceled -= ReturnNormalSpeed;
      slowDownTime.Disable();
      speedUpTime.canceled -= ReturnNormalSpeed;
      speedUpTime.Disable();
      inputActionMap.Disable();
    }

    public void SetActionMapState(bool enable)
    {
      if (enable)
      {
        EnableInputs();
        animator.SetBool("isDead", false);
      }
      else
      {
        DisableInputs();
        animator.SetBool("isDead", true);
      }
    }

    public void ChangeColor(Color color)
    {
      spriteRenderer.color = color;
    }

    public void SetColliders()
    {
      playerBodyCollider.enabled = !playerBodyCollider.enabled;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
      if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
      {
        animator.SetBool("isJumping", false);
      }
    }
  }
}
