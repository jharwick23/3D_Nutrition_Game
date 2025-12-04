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

    // Handling Hat Object
    public HatHandler _hatHandler;
    
    // --- Player Movement/Camera Variables --- \\
    [SerializeField] private float MovementSpeed = 10f;
    [SerializeField] private float RotationSpeed = 20f; // Horizontal Look Speed
    [SerializeField] private float LookSensitivityY = 20f; // Veritical Look Speed
    [SerializeField] private float MinCamAngle = 45f;
    [SerializeField] private float MaxCamAngle = -75f;
    [SerializeField] private float JumpForce = 8f;
    [SerializeField] private float Gravity = -25f;
    [SerializeField] private float SprintMultiplier = 2f;
    private float distanceFromGroundThreshold = 0.4f;

    // Movement and Animation variables
    private float _rotationY;
    private float _verticalVelocity;
    private float _lastAttackTime = 0f;
    private float _attackHoldDuration = 1f;
    private bool _isSprinting = false;
    private bool _isEquipped = false;
    private bool _isBlocking = false;
    private bool _isMeleeing = false;
    private bool _dodgePressed = false;
    private bool _hasLanded = false;

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
        if (_hatHandler == null)
        {
            _hatHandler = FindFirstObjectByType<HatHandler>();
        }

        // Initialize UI
        _uiHandler.UpdateHealthUI(CurrentHealth, MaxHealth);
        _uiHandler.UpdateCoinUI(Coins);
        Respawn();
    }

    void Update()
    {
        if (!_dodgePressed)
        {
            EquippedAnim();
            MeleeAnim();
            BlockingAnim();
        }

        // Checks to see the amount of time passed between the last attack
        // If it is greater than one second we will set the _isEquipped variable
        // to false
        // Also changes hat position to have the player wear the hat
        if (Time.time - _lastAttackTime > _attackHoldDuration 
            || _isMeleeing || _isBlocking)
        {
            SetEquipped(false);
            _hatHandler.SetOnHead();
        }
    }

    // These three functions set the animator booleans
    private void EquippedAnim()
    {
        _animator.SetBool("IsEquipped", _isEquipped);
    }

    private void MeleeAnim()
    {
        _animator.SetBool("IsMelee", _isMeleeing);
    }

    private void BlockingAnim()
    {
        _animator.SetBool("IsBlocking", _isBlocking);
    }

    private void UpdateLayerWeight(bool isActive, int layerIndex, float speed)
    {
        float currentLayerWeight = _animator.GetLayerWeight(layerIndex);
        float targetLayerWeight;

        if (isActive)
        {
            targetLayerWeight = 1;
        }
        else
        {
            targetLayerWeight = 0;
        }

        float newLayerWeight = Mathf.MoveTowards(
            currentLayerWeight,
            targetLayerWeight,
            Time.deltaTime * speed
        );

        _animator.SetLayerWeight(layerIndex, newLayerWeight);
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
        
        // If the player has landed stop horizontal movement momentarily
        if (_hasLanded)
        {
            movementVector = Vector2.zero;
        }

        // Tells the animator whether or not the character is grounded
        _animator.SetBool("IsGrounded", IsGrounded());

        // Tells animator JumpRequested is false when player gets in the air after jump
        if (!IsGrounded())
        {
            _animator.SetBool("JumpRequested", false);
        }

        Vector3 move = transform.right * movementVector.x + transform.forward * movementVector.y;
        move = move * MovementSpeed * Time.deltaTime * (_isSprinting ? SprintMultiplier : 1f);

        // Compute target speed value for Animator
        // If isSprinting is true then double speed
        float targetSpeed = movementVector.magnitude * (_isSprinting ? SprintMultiplier : 1f);
        
        // Get the horizontal and vertical inputs to determine movement direction
        float movementDirectionX = 0f;
        float movementDirectionY = 0f;

        if (Mathf.Abs(movementVector.x) > Mathf.Epsilon || Mathf.Abs(movementVector.y) > Mathf.Epsilon)
        {
            if (movementVector.x > 0) // Right
            {
                movementDirectionX = 1f;
            }
            else if (movementVector.x < 0) // Left
            {
                movementDirectionX = -1f;
            }

            if (movementVector.y > 0) // Forward
            {
                movementDirectionY = 1f;
            }
            else if (movementVector.y < 0) // Backward
            {
                movementDirectionY = -1f;
            }

            // Handle diagonal movement (combine forward/backward and left/right)
            if (Mathf.Abs(movementVector.x) > Mathf.Epsilon && Mathf.Abs(movementVector.y) > Mathf.Epsilon)
            {
                if (movementVector.x > 0 && movementVector.y > 0) // Forward-Right
                {
                    movementDirectionX = 0.5f;
                    movementDirectionY = 0.5f;
                }
                else if (movementVector.x < 0 && movementVector.y > 0) // Forward-Left
                {
                    movementDirectionX = -0.5f;
                    movementDirectionY = 0.5f;
                }
                else if (movementVector.x > 0 && movementVector.y < 0) // Backward-Right
                {
                    movementDirectionX = 0.5f;
                    movementDirectionY = -0.5f;
                }
                else if (movementVector.x < 0 && movementVector.y < 0) // Backward-Left
                {
                    movementDirectionX = -0.5f;
                    movementDirectionY = -0.5f;
                }
            }
        }

        // Set animator parameters for blending movement
        _animator.SetFloat("MovementX", movementDirectionX, 0.1f, Time.deltaTime); // X controls Left-Right
        _animator.SetFloat("MovementY", movementDirectionY, 0.1f, Time.deltaTime); // Y controls Forward-Backward

        _animator.SetFloat("Speed", targetSpeed, 0.1f, Time.deltaTime); // Comment out for Maher's capsule character if needed

        // If the dodge pressed bool is true and the speed is greater than 0.1 we can set the dodge trigger
        if (_dodgePressed && (targetSpeed > 0.1f))
        {
            _animator.SetTrigger("Dodge");
            _dodgePressed = false;
        }
        else
        {
            _dodgePressed = false;
        }
        
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
        if (IsGrounded())
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
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
        }
        _uiHandler.UpdateHealthUI(CurrentHealth, MaxHealth);
        DoDeath();
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

    public bool IsGrounded()
    {
        Vector3 bottom = transform.position + _characterController.center + Vector3.down * (_characterController.height / 2f);

        return Physics.Raycast(bottom, Vector3.down, distanceFromGroundThreshold);
    }

    private void DoDeath()
    {
        if (CurrentHealth <= 0)
        {
            // Respawn & Set Health to Max
            Respawn();
            CurrentHealth = MaxHealth;
            _uiHandler.UpdateHealthUI(CurrentHealth, MaxHealth);

            // Reset Ammo
            ProjectileGun HatWeapon = FindFirstObjectByType<ProjectileGun>();
            if (HatWeapon)
            {
                HatWeapon.StartReloading();
            }
            else
            {
                Debug.Log("HatWeapon not found in transform children.");
            }

            // Reset Puzzle
            GameObject puzzle1 = GameObject.FindGameObjectWithTag("Puzzle1");
            Puzzle1 Puzzle1Script = puzzle1.GetComponent<Puzzle1>();
            Puzzle1Script.resetPuzzle();

            // Delete all enemies in the scene


            
        }
    }

    private void Respawn()
    {
        // Find position of the object tagged "RespawnPoint"
        GameObject respawnPoint = GameObject.FindGameObjectWithTag("Spawnpoint");
        if (respawnPoint != null)
        {
            _characterController.enabled = false;
            transform.position = respawnPoint.transform.position;
            _characterController.enabled = true;
            _verticalVelocity = -2f;
        }
        else
        {
            Debug.LogError("RespawnPoint not found in the scene.");
        }
    }

    public void SetEquipped(bool isEquipped)
    {
        _isEquipped = isEquipped;
    }

    public bool GetEquipped()
    {
        return _isEquipped;
    }

    public void IsMeleeAttacking(bool isMeleeing)
    {
        _isMeleeing = isMeleeing;
    }

    public bool GetMeleeAttacking()
    {
        return _isMeleeing;
    }

    public void IsBlocking(bool isBlocking)
    {
        _isBlocking = isBlocking;
    }
    
    public bool GetBlocking()
    {
        return _isBlocking;
    }

    public void IsDodging()
    {
        _dodgePressed = true;
    }

    public void SetLastAttackTime()
    {
        _lastAttackTime = Time.time;
    }

    public void SetHasLanded(bool hasLanded)
    {
        _hasLanded = hasLanded;
    }
}
