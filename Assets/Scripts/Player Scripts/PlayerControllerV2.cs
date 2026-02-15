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
    //private CameraControllerV2 CameraController;
    public UIHandler _uiHandler;
    private Animator _animator;

    // Camera Variables
    [SerializeField] private Transform cameraRig; // rotates yaw
    [SerializeField] private Transform cameraFollowTarget; // pitch is applied

    private float yaw;
    private float pitch;

    // Handling Hat Object
    public HatHandler _hatHandler;

    // Knife Object
    public KnifeController Knife;

    // Pan Object
    public PanController Pan;
    
    // --- Player Movement/Camera Variables --- \\
    [SerializeField] private float MovementSpeed = 5f;
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
    private float _lastShootingAttackTime = 0f;
    private float _shootingHoldDuration = 5f;
    [SerializeField] private float fallVelocityThreshold = -2.5f;
    [SerializeField] private float fallTimeThreshold = 0.18f;
    private bool _isSprinting = false;
    private bool _isHatEquipped = false;
    private bool _isBlocking = false;
    private bool _isMeleeing = false;
    private bool _dodgePressed = false;
    //private bool _hasLanded = false;
    private bool _shieldBash = false;
    private bool _isDead = false;
    private float _fallTime = 0f;
    private bool _isHighFall = false;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        //if (CameraController == null)
        //{
        //    CameraController = FindFirstObjectByType<CameraControllerV2>();
        //}
        if (_uiHandler == null)
        {
            _uiHandler = FindFirstObjectByType<UIHandler>();
        }
        if (_hatHandler == null)
        {
            _hatHandler = FindFirstObjectByType<HatHandler>();
        }
        if (Knife == null)
        {
            Knife = FindFirstObjectByType<KnifeController>();
        }
        if (Pan == null)
        {
            Pan = FindFirstObjectByType<PanController>();
        }
    }

    void Start()
    {
        // Initialize Player Data
        InitializePlayerData();

        Respawn();
    }

    void Update()
    {
        if (!_dodgePressed)
        {
            HatEquippedAnim();
            MeleeAnim();
            BlockingAnim();
        }

        // Checks to see the amount of time passed between the last ranged attack
        // Also changes hat position to have the player wear the hat
        if (Time.time - _lastShootingAttackTime > _shootingHoldDuration 
            || _isMeleeing || _isBlocking)
        {
            SetHatEquipped(false);
            _hatHandler.SetOnHead();
        }
    }

    // Initializes player data from PlayerPrefs stats
    public void InitializePlayerData()
    {
        // Initialize Stats
        MaxHealth = PlayerPrefs.GetInt("MaxHealthStat", 0) * 20 + 100;
        CurrentHealth = MaxHealth;
        _uiHandler.UpdateHealthUI(CurrentHealth, MaxHealth);

        Coins = PlayerPrefs.GetInt("Coins", 0);
        _uiHandler.UpdateCoinUI(Coins);

        MovementSpeed = 5f + PlayerPrefs.GetInt("MovementSpeedStat", 0) * 0.5f;
        
    }

    // These three functions set the animator booleans
    private void HatEquippedAnim()
    {
        _animator.SetBool("IsHatEquipped", _isHatEquipped);
    }

    private void MeleeAnim()
    {
        _animator.SetBool("IsMelee", _isMeleeing);
    }

    private void BlockingAnim()
    {
        _animator.SetBool("IsBlocking", _isBlocking);
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

        // Does not allow movement if player is dead
        if(_isDead) return;
        
        // Redundant
        // If the player has landed stop horizontal movement momentarily
        //if (_hasLanded)
        //{
        //    movementVector = Vector2.zero;
        //}

        bool grounded = IsGrounded();
        _animator.SetBool("IsGrounded", grounded);

        // Tells animator JumpRequested is false when player gets in the air after jump
        if (!grounded)
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
        if (grounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2f;
        }

        // Detect if the player is falling from high ground
        if (!grounded && _verticalVelocity < fallVelocityThreshold)
        {
            _fallTime += Time.deltaTime;
        }
        else
        {
            _fallTime = 0f;
        }

        _isHighFall = _fallTime >= fallTimeThreshold;
        _animator.SetBool("IsHighFall", _isHighFall);

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
        // Player yaw from Mouse X
        yaw += lookVector.x * RotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Camera rig yaw matches player yaw
        if (cameraRig)
            cameraRig.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Camera pitch from Mouse Y
        pitch -= lookVector.y * LookSensitivityY * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, MaxCamAngle, MinCamAngle);

        if(cameraFollowTarget)
        {
            cameraFollowTarget.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
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

    public void TakeDamage(float damageAmount)
    {
        if (GetBlocking())
        {
            float baseReduction = 0.30f;
            float perPoint = 0.05f;
            int blockStat = PlayerPrefs.GetInt("BlockStrengthStat", 0);
            float totalReduction = baseReduction + (blockStat * perPoint);
            damageAmount *= 1 - totalReduction;
        }

        CurrentHealth -= Mathf.FloorToInt(damageAmount);
        if (CurrentHealth <= 0)
        {
            DeathSound();
            CurrentHealth = 0;
            _uiHandler.UpdateHealthUI(CurrentHealth, MaxHealth);
            _animator.SetTrigger("IsDead");
            _isDead = true;
        }
        else
        {
            _uiHandler.UpdateHealthUI(CurrentHealth, MaxHealth);
            TakeDamageSound();
        }
        // DoDeath();
    }

    private void TakeDamageSound()
    {
        if (!SFXManager.Instance)
        {
            Debug.LogError("SFXManager not found in scene");
            return;
        }

        SFXManager.Instance.Play(SFXManager.SFXType.DamageTaken);
    }

    private void DeathSound()
    {
        if (!SFXManager.Instance)
        {
            Debug.LogError("SFXManager not found in scene");
            return;
        }

        SFXManager.Instance.Play(SFXManager.SFXType.Death);
    }

    public void Heal(int healAmount)
    {
        CurrentHealth += healAmount;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        _uiHandler.UpdateHealthUI(CurrentHealth, MaxHealth);
        HealSound();
    }

    private void HealSound()
    {
        if (!SFXManager.Instance)
        {
            Debug.LogError("SFXManager not found in scene");
            return;
        }

        SFXManager.Instance.Play(SFXManager.SFXType.Heal);
    }

    public void AddCoins(int coinAmount)
    {
        Coins += coinAmount;
        PlayerPrefs.SetInt("Coins", Coins);
        _uiHandler.UpdateCoinUI(Coins);
    }

    public bool IsGrounded()
    {
        Vector3 bottom = transform.position + _characterController.center + Vector3.down * (_characterController.height / 2f);

        return Physics.Raycast(bottom, Vector3.down, distanceFromGroundThreshold);
    }

    public void DoDeath()
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
            if (puzzle1)
            {
               Puzzle1 Puzzle1Script = puzzle1.GetComponent<Puzzle1>();
                Puzzle1Script.resetPuzzle(); 
            }

            // Delete all enemies in the scene

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyZones");

            foreach (GameObject enemy in enemies) {
                enemy.GetComponent<EnemySpawner>().ResetArea();
                if (enemy.GetComponent<BossSpawn>() != null)
                {
                    enemy.GetComponent<BossSpawn>().ResetArea();
                }
            }

            GameObject zone = GameObject.FindGameObjectWithTag("SideZone");
            if (zone != null)
            {
                zone.GetComponent<WaveSpawner>().ResetSpawner();
            }
            
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
            _isDead = false;
        }
        else
        {
            Debug.LogError("RespawnPoint not found in the scene.");
        }
    }

    public bool IsMaxHealth()
    {
        if (CurrentHealth == MaxHealth)
        {
            return true;
        }
        return false;
    }

    // -- Animation Bool Set Function -- \\

    public void SetHatEquipped(bool isHatEquipped)
    {
        _isHatEquipped = isHatEquipped;
    }

    public bool GetHatEquipped()
    {
        return _isHatEquipped;
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

    public void SetLastShootingAttackTime()
    {
        _lastShootingAttackTime = Time.time;
    }

    //public void SetHasLanded(bool hasLanded)
    //{
    //    _hasLanded = hasLanded;
    //}

    // We want to check whether IsMovJump true or not so when we are falling it either uses the moving jump animation,
    // or the falling animation
    // If IsMovJump is true when falling while doing the moving jump animation, the normal falling animation WILL NOT play.
    public void SetMovJumpTrue()
    {
        _animator.SetBool("IsMovJump", true);
    }

    public void SetMovJumpFalse()
    {
        _animator.SetBool("IsMovJump", false);
    }

    // Sets bashing trigger to do bash animation
    public void IsBashing()
    {
        _animator.SetTrigger("BashShield");
        Pan.BeginBashDamage(0.15f);
    }

    // Sets _shieldBash variable (used to keep the shield in hand after bashing)
    public void SetShieldBash(bool shieldBash)
    {
        _shieldBash = shieldBash;
    }

    public bool GetShieldBash()
    {
        // Grabs shieldbash variable
        return _shieldBash;
    }
}
