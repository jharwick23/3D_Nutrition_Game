using UnityEngine;

public class PlayerControllerV2 : MonoBehaviour
{
    private CharacterController _characterController;
    private CameraControllerV2 CameraController;
    [SerializeField] private float MovementSpeed = 10f;
    [SerializeField] private float RotationSpeed = 20f; // Horizontal Look Speed
    [SerializeField] private float LookSensitivityY = 20f; // Veritical Look Speed
    [SerializeField] private float MinCamAngle = 45f;
    [SerializeField] private float MaxCamAngle = -75f;
    [SerializeField] private float JumpForce = 8f;
    [SerializeField] private float Gravity = -25f;
    private float _rotationY;
    private float _verticalVelocity;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        if (CameraController == null)
        {
            CameraController = FindFirstObjectByType<CameraControllerV2>();
        }
    }

    public void Move(Vector2 movementVector)
    {
        if (_characterController == null)
        {
            Debug.LogWarning("CharacterController component not found.");
            return;
        }
        else if (_characterController.enabled == false)
        {
            Debug.LogWarning("CharacterController component is Currently Inactive.");
            return;
        }

        Vector3 move = transform.right * movementVector.x + transform.forward * movementVector.y;
        move = move * MovementSpeed * Time.deltaTime;
        _characterController.Move(move);

        // Apply gravity
        _verticalVelocity += Gravity * Time.deltaTime;

        // Small downward force to keep grounded
        if (_characterController.isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2f;
        }

        _characterController.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);
    }

    public void Rotate(Vector2 lookVector)
    {
        if (_characterController == null)
        {
            Debug.LogWarning("CharacterController component not found.");
            return;
        }
        else if (_characterController.enabled == false)
        {
            Debug.LogWarning("CharacterController component is Currently Inactive.");
            return;
        }
        
        // Character Horizontal rotation -- Camera follows the player rotation aswell (CameraControllerV2.cs)
        _rotationY += lookVector.x * RotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0, _rotationY, 0);

        // Camera Vertical rotation -- Camera pitch/angle
        CameraController.CameraPitch -= lookVector.y * LookSensitivityY * Time.deltaTime;
        CameraController.CameraPitch = Mathf.Clamp(CameraController.CameraPitch, MaxCamAngle, MinCamAngle);
    }

    public void Jump()
    {
        if (_characterController.isGrounded)
        {
            _verticalVelocity = JumpForce;
        }
    }
}
