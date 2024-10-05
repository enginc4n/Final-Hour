using System.Collections;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.PlayerController
{
  public class PlayerControllerView : EventView
  {
    public RectTransform rectTransform;
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
    private Collider2D _activeCollider;
    
    private UnityEngine.Gyroscope _gyro;
    private Vector3 _rotation;
    private bool gyroActive;
    private bool accelActive;
    
    [HideInInspector]
    public bool isDashReady = true;
    
    [HideInInspector]
    public bool isFireReady = true;

    private SpeedState _speedState;

    protected override void Awake()
    {
      playerInputActions = new PlayerInputActions();
      inputActionMap = playerInput.actions.FindActionMap("Player");
    }
    
    private void EnableGyro()
    {
      if (gyroActive || accelActive)
      {
        return;
      }
      
      if (SystemInfo.supportsGyroscope)
      {
        _gyro = Input.gyro;
        _gyro.enabled = true;
        gyroActive = _gyro.enabled;
      } else if (SystemInfo.supportsAccelerometer)
      {
        accelActive = true;
      }

      StartCoroutine(MobileControlRoutine());
    }
    
    private void DisableGyro()
    {
      if (!gyroActive && !accelActive)
      {
        return;
      }

      if (SystemInfo.supportsGyroscope)
      {
        _gyro = Input.gyro;
        _gyro.enabled = false;
        gyroActive = _gyro.enabled;
      } else if (SystemInfo.supportsAccelerometer)
      {
        accelActive = false;
      }
      
      StopCoroutine(MobileControlRoutine());
    }
    
    private IEnumerator MobileControlRoutine()
    {
      while (true)
      {
        switch (gyroActive)
        {
          case false when !accelActive:
            yield break;
          case true:
            _rotation = _gyro.attitude.eulerAngles;
            break;
          default:
          {
            if (SystemInfo.supportsAccelerometer)
            {
              _rotation = Input.acceleration;
            }

            break;
          }
        }

        switch (_rotation.x)
        {
          case < -0.1f when _speedState != SpeedState.Slow:
            OnSlowTime();
            break;
          case > 0.1f when _speedState != SpeedState.Fast:
            OnSpeedUpTime();
            break;
          default:
          {
            if (_speedState != SpeedState.Normal)
            {
              ReturnNormalSpeed(); 
            }
            break;
          }
        }
      }
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

      if (!_activeCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
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

      if (!_activeCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
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
      dispatcher.Dispatch(PlayerControllerEvents.CrouchFinished);
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
      _speedState = SpeedState.Fast;
      dispatcher.Dispatch(PlayerControllerEvents.SpeedUpTime);
    }

    private void OnSlowTime()
    {
      _speedState = SpeedState.Slow;
      dispatcher.Dispatch(PlayerControllerEvents.SlowDownTime);
    }

    
    private void ReturnNormalSpeed(InputAction.CallbackContext context)
    {
      _speedState = SpeedState.Normal;
      dispatcher.Dispatch(PlayerControllerEvents.ReturnNormalSpeed);
    }
    
    private void ReturnNormalSpeed()
    {
      _speedState = SpeedState.Normal;
      dispatcher.Dispatch(PlayerControllerEvents.ReturnNormalSpeed);
    }
    
    public void EnableInputs()
    {
      if (SystemInfo.deviceType == DeviceType.Handheld)
      {
        EnableGyro();
      }

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
    }
    
    public void DisableInputs()
    {
      if (SystemInfo.deviceType == DeviceType.Handheld)
      {
        DisableGyro();
      }      
      
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

      _activeCollider = playerBodyCollider.enabled ? playerBodyCollider : playerCrouchCollider.enabled ? playerCrouchCollider : null;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
      if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
      {
        playerAnimator.SetBool("isJumping", false);
      }
    }

    #region MobileInputs

    public void OnJump( )
    {
      if (!_activeCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
      {
        return;
      }

      CrouchFinished();
      playerAnimator.SetBool("isJumping", true);
      dispatcher.Dispatch(PlayerControllerEvents.Jump);
    }

    public void OnCrouch()
    {
      if (!_activeCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
      {
        return;
      }

      playerAnimator.SetBool("isCrouch", true);
      dispatcher.Dispatch(PlayerControllerEvents.Crouch);
    }

    public void OnFire()
    {
      if (!isFireReady)
      {
        return;
      }

      dispatcher.Dispatch(PlayerControllerEvents.FireBulletAction);
    }

    public void OnDash()
    {
     
      if (!isDashReady)
      {
        return;
      }
      
      dispatcher.Dispatch(PlayerControllerEvents.Dash);
    }

    #endregion
  }
}