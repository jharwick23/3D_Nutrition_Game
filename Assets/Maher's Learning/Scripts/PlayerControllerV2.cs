using UnityEngine;

public class PlayerControllerV2 : MonoBehaviour
{
    // --- Player Stats Variables --- \\
    [SerializeField] private int MaxHealth = 100;
    [SerializeField] private int CurrentHealth = 100;
    // [SerializeField] private int Level = 1;
    [SerializeField] private int Coins = 0;
    
    // --- Assigned Controllers Variables --- \\
    private CharacterController _characterController;
    private CameraControllerV2 CameraController;
    public UIHandler _uiHandler;
    private Animator _animator;

    // --- Player Movement/Camera Variables --- \\
    [SerializeField] private float MovementSpeed = 10f;
    [SerializeField] private float RotationSpeed = 20f; // Horizontal Look Speed
    [SerializeField] private float LookSensitivityY = 20f; // Veritical Look Speed
    [SerializeField] private float MinCamAngle = 45f;
    [SerializeField] private float MaxCamAngle = -75f;
    [SerializeField] private float JumpForce = 8f;
    [SerializeField] private float Gravity = -25f;
    [SerializeField] private float SprintMultiplier = 2f;
    private float _rotationY;
    private float _verticalVelocity;
    private bool _isSprinting = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        if (CameraController == null)
        {
            CameraController = FindFirstObjectByType<CameraControllerV2>();
        }
        if (_uiHandler == null)
        {
            _uiHandler = FindFirstObjectByType<UIHandler>();
        }

        // Initialize UI
        _uiHandler.UpdateHealthUI(CurrentHealth, MaxHealth);
        _uiHandler.UpdateCoinUI(Coins);
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

        // Tells the animator whether or not the character is grounded
        bool grounded = _characterController.isGrounded;
        _animator.SetBool("IsGrounded", grounded);

        // Tells animator JumpRequested is false when player gets in the air after jump
        if (!_characterController.isGrounded)
        {
            _animator.SetBool("JumpRequested", false);
        }

        Vector3 move = transform.right * movementVector.x + transform.forward * movementVector.y;
        move = move * MovementSpeed * Time.deltaTime;

        // Compute target speed value for Animator
        // If isSprinting is true then double speed
        float targetSpeed = movementVector.magnitude * (_isSprinting ? SprintMultiplier : 1f);
        _animator.SetFloat("Speed", targetSpeed, 0.1f, Time.deltaTime); // Comment out for Maher's capsule character if needed
        
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
            _animator.SetBool("JumpRequested", true);
            _verticalVelocity = JumpForce;
        }
    }

    public void SetSprinting(bool isSprinting)
    {
        _isSprinting = isSprinting;
    }

    public void TakeDamage(int damageAmount)
    {
        CurrentHealth -= damageAmount;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
        _uiHandler.UpdateHealthUI(CurrentHealth, MaxHealth);
    }

    public void Heal(int healAmount)
    {
        CurrentHealth += healAmount;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        _uiHandler.UpdateHealthUI(CurrentHealth, MaxHealth);
    }

    public void AddCoins(int coinAmount)
    {
        Coins += coinAmount;
        _uiHandler.UpdateCoinUI(Coins);
    }

    private bool IsNearGround(float distance)
    {
        return Physics.Raycast(transform.position, Vector3.down, distance);
    }
}
