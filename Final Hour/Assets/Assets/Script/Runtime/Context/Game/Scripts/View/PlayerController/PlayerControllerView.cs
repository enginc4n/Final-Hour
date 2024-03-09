using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerView : EventView
{
    [SerializeField]
    private float jumpSpeed;

    private Rigidbody2D playerRigidboyd2d;
    private CapsuleCollider2D playerBodyCollider;
    private BoxCollider2D playerCrouchCollider;
    private PlayerInputActions playerInputActions;
    private InputAction crouch;

    private void Awake()
    {
        playerRigidboyd2d = GetComponent<Rigidbody2D>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerCrouchCollider = GetComponent<BoxCollider2D>();
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        crouch = playerInputActions.Player.Crouch;
        crouch.Enable();
        crouch.canceled += CrouchFinished;
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

        playerRigidboyd2d.velocity += new Vector2(0f, jumpSpeed);
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

    private void OnDisable()
    {
        crouch.Disable();
    }

    private void SetColliders()
    {
        playerBodyCollider.enabled = !playerBodyCollider.enabled;
        playerCrouchCollider.enabled = !playerCrouchCollider.enabled;
    }
}
