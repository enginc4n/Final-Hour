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
    public Animator playerAnimator;
    public GameObject dashParticle;
    public GameObject deadParticle;
    
    private PlayerInputActions playerInputActions;
    public InputActionMap inputActionMap;
    private InputAction jump;
    private InputAction dash;
    private InputAction fire;
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

    public void ResetPosition()
    {
      transform.GetComponent<RectTransform>().anchoredPosition = GameMechanicSettings.PlayerSpawnPosition;
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

      CrouchFinished();
      playerAnimator.SetBool("isJumping", true);
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

      playerAnimator.SetBool("isCrouch", true);
      dispatcher.Dispatch(PlayerControllerEvents.Crouch);
    }

    public void CrouchFinished()
    {
      playerAnimator.SetBool("isCrouch", false);
      SetColliders(false);
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
      yield return new WaitForSeconds(GameMechanicSettings.FireCooldown);
      
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
      yield return new WaitForSeconds(GameMechanicSettings.DashCooldown);
      
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
    
    public void EnableInputs()
    {
      crouch = playerInputActions.Player.Crouch;
      crouch.Enable();
      slowDownTime = playerInputActions.Player.SlowTime;
      slowDownTime.Enable();
      slowDownTime.canceled += ReturnNormalSpeed;
      speedUpTime = playerInputActions.Player.SpeedUpTime;
      speedUpTime.Enable();
      speedUpTime.canceled += ReturnNormalSpeed;
      jump = playerInputActions.Player.Jump;
      jump.Enable();
      fire = playerInputActions.Player.Fire;
      fire.Enable();
      dash = playerInputActions.Player.Dash;
      dash.Enable();
      inputActionMap.Enable();
      
      
      ReturnNormalSpeed(default);
    }
    
    public void DisableInputs()
    {
      crouch.Disable();
      slowDownTime.canceled -= ReturnNormalSpeed;
      slowDownTime.Disable();
      speedUpTime.canceled -= ReturnNormalSpeed;
      speedUpTime.Disable();
      jump.Disable();
      fire.Disable();
      dash.Disable();
      inputActionMap.Disable();
    }

    public void SetActionMapState(bool enable)
    {
      if (enable)
      {
        EnableInputs();
        playerAnimator.SetBool("isDead", false);
      }
      else
      {
        DisableInputs();
        playerAnimator.SetBool("isDead", true);
      }
    }

    public void ChangeColor(Color color)
    {
      spriteRenderer.color = color;
    }

    public void SetColliders(bool isCrouch)
    {
      playerBodyCollider.enabled = !isCrouch;
      playerCrouchCollider.enabled = isCrouch;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
      if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
      {
        playerAnimator.SetBool("isJumping", false);
      }
    }
  }
}
