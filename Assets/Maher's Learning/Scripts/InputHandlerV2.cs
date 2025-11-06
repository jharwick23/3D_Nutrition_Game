using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandlerV2 : MonoBehaviour
{
    public PlayerControllerV2 PlayerController;
    private InputAction _moveAction, _lookAction, _jumpAction;
    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            _moveAction = playerInput.actions.FindAction("Move");
            _lookAction = playerInput.actions.FindAction("Look");
            _jumpAction = playerInput.actions.FindAction("Jump");
            _jumpAction.performed += OnJumpPerformed;
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
}
