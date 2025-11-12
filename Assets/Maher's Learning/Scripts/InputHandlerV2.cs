using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandlerV2 : MonoBehaviour
{
    public PlayerControllerV2 PlayerController;
    public ProjectileGun StandardProjectileGun;
    private InputAction _moveAction, _lookAction, _jumpAction, _attackAction, _sprintAction;
    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            _moveAction = playerInput.actions.FindAction("Move");
            _lookAction = playerInput.actions.FindAction("Look");
            _jumpAction = playerInput.actions.FindAction("Jump");
            _attackAction = playerInput.actions.FindAction("Attack");
            _sprintAction = playerInput.actions.FindAction("Sprint");
            _attackAction.performed += OnAttackPerformed;
            _jumpAction.performed += OnJumpPerformed;
            _sprintAction.performed += OnSprintPerformed;
            _sprintAction.canceled += OnSprintCancelled;
        }
        else
        {
            Debug.LogWarning("PlayerInput component not found on GameObject. Make sure an Input Actions asset or PlayerInput is present.");
        }

        // Ensure actions are enabled
        _moveAction?.Enable();
        _lookAction?.Enable();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (_moveAction == null || _lookAction == null || PlayerController == null)
            return;

        Vector2 movementVector = _moveAction.ReadValue<Vector2>();
        Vector2 lookVector = _lookAction.ReadValue<Vector2>();

        PlayerController.Move(movementVector);
        PlayerController.Rotate(lookVector);
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        PlayerController.Jump();
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        StandardProjectileGun.Shoot();
    }

    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        // Set sprinting bool in Player Controller to true if sprint action is not null
        //bool sprinting = _sprintAction != null && _sprintAction.ReadValue<float>() > 0;
        PlayerController.SetSprinting(true);
    }

    private void OnSprintCancelled(InputAction.CallbackContext context)
    {
        PlayerController.SetSprinting(false);
    }
}
