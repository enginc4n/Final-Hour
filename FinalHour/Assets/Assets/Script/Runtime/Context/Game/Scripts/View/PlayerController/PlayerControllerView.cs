using System;
using System.Collections;
using Assets.Script.Runtime.Context.Game.Scripts.Enum;
using Assets.Script.Runtime.Context.Game.Scripts.Model;
using Assets.Script.Runtime.Context.Menu.Scripts.Enum;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Script.Runtime.Context.Game.Scripts.View.PlayerController
{
  public class PlayerControllerView : EventView
  {
    public RectTransform rectTransform;
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
    //  inputActionMap = playerInput.actions.FindActionMap("Player");
     // inputActionMap.Enable();
    }
    
    private void EnableGyro()
    {
      if (gyroActive || accelActive)
      {
        return;
      }
      
      if (SystemInfo.supportsGyroscope)
      {
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
    
    private void OnJump(InputAction.CallbackContext context)
    {
      if (!_activeCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
      {
        return;
      }

      CrouchFinished();
      playerAnimator.SetBool("isJumping", true);
      dispatcher.Dispatch(PlayerControllerEvents.Jump);
    }

    private void OnCrouch(InputAction.CallbackContext context)
    {
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

    private void OnFire(InputAction.CallbackContext context)
    {
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

    private void OnDash(InputAction.CallbackContext context)
    {
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

    public void OnSpeedUpTime()
    {
      _speedState = SpeedState.Fast;
      dispatcher.Dispatch(PlayerControllerEvents.SpeedUpTime);
    }
    
    public void OnSpeedUpTime(InputAction.CallbackContext context)
    {
      _speedState = SpeedState.Fast;
      dispatcher.Dispatch(PlayerControllerEvents.SpeedUpTime);
    }

    public void OnSlowTime()
    {
      _speedState = SpeedState.Slow;
      dispatcher.Dispatch(PlayerControllerEvents.SlowDownTime);
    }
    
    public void OnSlowTime(InputAction.CallbackContext context)
    {
      _speedState = SpeedState.Slow;
      dispatcher.Dispatch(PlayerControllerEvents.SlowDownTime);
    }
    
    private void ReturnNormalSpeed(InputAction.CallbackContext context)
    {
      _speedState = SpeedState.Normal;
      dispatcher.Dispatch(PlayerControllerEvents.ReturnNormalSpeed);
    }
    
    private void ReturnNormalSpeedTutorial(InputAction.CallbackContext context)
    {
      _speedState = SpeedState.Normal;
      dispatcher.Dispatch(PlayerControllerEvents.ReturnNormalSpeedTutorial);
    }
    
    private void ReturnNormalSpeed()
    {
      _speedState = SpeedState.Normal;
      dispatcher.Dispatch(PlayerControllerEvents.ReturnNormalSpeed);
    }
    
    private void ReturnNormalSpeedTutorial()
    {
      _speedState = SpeedState.Normal;
      dispatcher.Dispatch(PlayerControllerEvents.ReturnNormalSpeedTutorial);
    }


    #region Inputsystem
    
    public void SetInputs()
    {
      if (SystemInfo.supportsGyroscope)
      {
        _gyro = Input.gyro;
      }

      crouch = playerInputActions.Player.Crouch;
      slowDownTime = playerInputActions.Player.SlowTime;
      speedUpTime = playerInputActions.Player.SpeedUpTime;
      jump = playerInputActions.Player.Jump;
      fire = playerInputActions.Player.Fire;
      dash = playerInputActions.Player.Dash;
    }
    
    public void EnableAllInputs()
    {
      if (SystemInfo.deviceType == DeviceType.Handheld)
      {
        EnableGyro();
      }
      
      slowDownTime.Enable();
      slowDownTime.performed += OnSlowTime;
      slowDownTime.canceled += ReturnNormalSpeed;
      
      speedUpTime.Enable();
      speedUpTime.performed += OnSpeedUpTime;
      speedUpTime.canceled += ReturnNormalSpeed;
      
      jump.Enable();
      jump.performed += OnJump;
      
      crouch.Enable();
      crouch.performed += OnCrouch;

      fire.Enable();
      fire.performed += OnFire;
      
      dash.Enable();
      dash.performed += OnDash;
    }
    
    public void DisableAllInputs()
    {
      if (SystemInfo.deviceType == DeviceType.Handheld)
      {
        DisableGyro();
      }      
      
      slowDownTime.Disable();
      slowDownTime.performed -= OnSlowTime;
      slowDownTime.canceled -= ReturnNormalSpeed;
      
      speedUpTime.Disable();
      speedUpTime.performed -= OnSpeedUpTime;
      speedUpTime.canceled -= ReturnNormalSpeed;
      
      jump.Disable();
      jump.performed -= OnJump;
      
      crouch.Disable();
      crouch.performed -= OnCrouch;

      fire.Disable();
      fire.performed -= OnFire;
      
      dash.Disable();
      dash.performed -= OnDash;
    }
    
    public void DisableInputsTutorial()
    {
      if (SystemInfo.deviceType == DeviceType.Handheld)
      {
        DisableGyro();
      }

      if (PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) == 6)
      {
        speedUpTime.Enable();
        speedUpTime.performed += OnSpeedUpTime;
        speedUpTime.canceled += ReturnNormalSpeed;
      }
      else  if (PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) == 7)
      {
        speedUpTime.Enable();
        speedUpTime.performed -= OnSpeedUpTime;
        speedUpTime.canceled -= ReturnNormalSpeed;
        speedUpTime.canceled += ReturnNormalSpeedTutorial;
      }
      else
      {
        speedUpTime.performed -= OnSpeedUpTime;
        speedUpTime.canceled -= ReturnNormalSpeed;
        speedUpTime.canceled -= ReturnNormalSpeedTutorial;
        speedUpTime.Disable();
      }
      
      if (PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) == 4)
      {
        slowDownTime.Enable();
        slowDownTime.performed += OnSlowTime;
        slowDownTime.canceled += ReturnNormalSpeed;
      }
      else  if (PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) == 5)
      {
        slowDownTime.Enable();
        slowDownTime.performed -= OnSlowTime;
        slowDownTime.canceled -= ReturnNormalSpeed;
        slowDownTime.canceled += ReturnNormalSpeedTutorial;
      } else
      {
        slowDownTime.performed -= OnSlowTime;
        slowDownTime.canceled -= ReturnNormalSpeed;
        slowDownTime.canceled -= ReturnNormalSpeedTutorial;
        slowDownTime.Disable();
      }

      if (PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) == 0)
      {
        jump.Enable();
        jump.performed += OnJump;
      }
      else
      {
        jump.Disable();
        jump.performed -= OnJump;
      }

      if (PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) == 1)
      {
        crouch.Enable();
        crouch.performed += OnCrouch;
      }
      else
      {
        crouch.Disable();
        crouch.performed -= OnCrouch;
      }

      if (PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) == 2)
      {
        fire.Enable();
        fire.performed += OnFire;
      }
      else
      {
        fire.Disable();
        fire.performed -= OnFire;
      }

      if (PlayerPrefs.GetInt(SettingKeys.CompletedTutorialSteps) == 3)
      {
        dash.Enable();
        dash.performed += OnDash;
      }
      else
      {
        dash.Disable();
        dash.performed -= OnDash;
      }
    }

    public void SetActionMapState(bool enable)
    {
      if (enable)
      {
        EnableAllInputs();
        playerAnimator.SetBool("isDead", false);
      }
      else
      {
        DisableAllInputs();
        playerAnimator.SetBool("isDead", true);
      }
    }
    
    #endregion

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

    public void OnJump()
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