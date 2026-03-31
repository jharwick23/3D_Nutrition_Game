using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class InputHandlerV2 : MonoBehaviour
{
    public PlayerControllerV2 PlayerController;
    public ProjectileGun StandardProjectileGun;
    private InputAction _moveAction, _lookAction, _jumpAction, _attackAction, _sprintAction, _reloadAction, _switchBulletAction,
                        _meleeAction, _blockAction, _dodgeAction, _pauseAction, _interactAction;
    private PlayerInput playerInput;
    public PauseMenu pauseMenu;
    public BulletShopNPC bulletShopNPC;
    public UpgradeMenuNPC upgradeMenuNPC;
    public TutorialManager tutorialManager;
    private bool inputsEnabled = true;
    private bool attackHeld = false;
    private float hatEquipShootDelay = 0.2f; // 0.1 - 0.3
    private float nextAllowedShootTime = 0f;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput != null)
        {
            _moveAction = playerInput.actions.FindAction("Move");
            _lookAction = playerInput.actions.FindAction("Look");
            _jumpAction = playerInput.actions.FindAction("Jump");
            _attackAction = playerInput.actions.FindAction("Attack");
            _sprintAction = playerInput.actions.FindAction("Sprint");
            _reloadAction = playerInput.actions.FindAction("Reload");
            _switchBulletAction = playerInput.actions.FindAction("SwitchBullet");
            _meleeAction = playerInput.actions.FindAction("Melee");
            _blockAction = playerInput.actions.FindAction("Block");
            _dodgeAction = playerInput.actions.FindAction("Dodge");
            _pauseAction = playerInput.actions.FindAction("Pause");
            _interactAction = playerInput.actions.FindAction("Interact");
            _pauseAction.performed += OnPausePerformed;
            EnableInputs();
        }
        else
        {
            Debug.LogWarning("PlayerInput component not found on GameObject. Make sure an Input Actions asset or PlayerInput is present.");
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start()
    {
        if (PlayerController == null)
        {
            PlayerController = FindFirstObjectByType<PlayerControllerV2>();
        }
        if (StandardProjectileGun == null)
        {
            StandardProjectileGun = FindFirstObjectByType<ProjectileGun>();
        }
        if(pauseMenu == null)
        {
            pauseMenu = FindFirstObjectByType<PauseMenu>();
        }
        if (bulletShopNPC == null)
        {
            bulletShopNPC = FindFirstObjectByType<BulletShopNPC>();
        }
        if (upgradeMenuNPC == null)
        {
            upgradeMenuNPC = FindFirstObjectByType<UpgradeMenuNPC>();
        }
        if (tutorialManager == null)
        {
            tutorialManager = FindFirstObjectByType<TutorialManager>();
        }
    }

    void Update()
    {
        if (_moveAction == null || _lookAction == null || PlayerController == null)
            return;

        Vector2 movementVector = _moveAction.ReadValue<Vector2>();
        Vector2 lookVector = _lookAction.ReadValue<Vector2>();

        PlayerController.Move(movementVector);
        PlayerController.Rotate(lookVector);

        // Shoot if hat is equipped
        if (attackHeld && inputsEnabled)
        {
            if (PlayerController.GetHatEquipped() && Time.time >= nextAllowedShootTime)
            {
                StandardProjectileGun.Shoot();
                PlayerController.SetLastShootingAttackTime();
            }
        }
    }

    void OnDisable()
    {
        _pauseAction.performed -= OnPausePerformed;
        DisableInputs();
    }

    public void DisableInputs()
    {
        inputsEnabled = false;
        _reloadAction.performed -= OnReloadPerformed;
        _attackAction.performed -= OnAttackPerformed;
        _attackAction.canceled -= OnAttackCanceled;
        _meleeAction.performed -= OnMeleePerformed;
        _meleeAction.canceled -= OnMeleeCancelled;
        _blockAction.performed -= OnBlockingPerformed;
        _blockAction.canceled -= OnBlockingCancelled;
        _dodgeAction.performed -= OnDodgePerformed;
        _jumpAction.performed -= OnJumpPerformed;
        _switchBulletAction.performed -= OnSwitchBulletPerformed;
        _sprintAction.performed -= OnSprintPerformed;
        _sprintAction.canceled -= OnSprintCancelled;
        _interactAction.performed -= OnInteractPerformed;
        // Ensure actions are disabled
        // _moveAction?.Disable();
        _lookAction?.Disable();
    }

    public void EnableInputs()
    {
        DisableInputs();
        inputsEnabled = true;
        _reloadAction.performed += OnReloadPerformed;
        _attackAction.performed += OnAttackPerformed;
        _attackAction.canceled += OnAttackCanceled;
        _meleeAction.performed += OnMeleePerformed;
        _meleeAction.canceled += OnMeleeCancelled;
        _blockAction.performed += OnBlockingPerformed;
        _blockAction.canceled += OnBlockingCancelled;
        _dodgeAction.performed += OnDodgePerformed;
        _jumpAction.performed += OnJumpPerformed;
        _switchBulletAction.performed += OnSwitchBulletPerformed;
        _sprintAction.performed += OnSprintPerformed;
        _sprintAction.canceled += OnSprintCancelled;
        _interactAction.performed += OnInteractPerformed;
        // Ensure actions are enabled
        _moveAction?.Enable();
        _lookAction?.Enable();
    }

    public void DisableInputsForVendors()
    {
        inputsEnabled = false;
        _reloadAction.performed -= OnReloadPerformed;
        _attackAction.performed -= OnAttackPerformed;
        _attackAction.canceled -= OnAttackCanceled;
        _meleeAction.performed -= OnMeleePerformed;
        _meleeAction.canceled -= OnMeleeCancelled;
        _blockAction.performed -= OnBlockingPerformed;
        _blockAction.canceled -= OnBlockingCancelled;
        _dodgeAction.performed -= OnDodgePerformed;
        _jumpAction.performed -= OnJumpPerformed;
        _switchBulletAction.performed -= OnSwitchBulletPerformed;
        _sprintAction.performed -= OnSprintPerformed;
        _sprintAction.canceled -= OnSprintCancelled;
        _lookAction?.Disable();
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        PlayerController.Jump();
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (!inputsEnabled) return;

        attackHeld = true;

        if(PlayerController.GetBlocking())
        {
            PlayerController.IsBashing();
            PlayerController.SetShieldBash(true);
            return;
        }

        // Equip hat only
        if (!PlayerController.GetHatEquipped())
        {
            PlayerController.SetHatEquipped(true);
            PlayerController._hatHandler.SetOnGun();
            PlayerController.SetLastShootingAttackTime();
            nextAllowedShootTime = Time.time + hatEquipShootDelay;
            return;
        }
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        attackHeld = false;
    }

    private void OnMeleePerformed(InputAction.CallbackContext context)
    {
        if (!PlayerController.GetBlocking())
        {
            PlayerController.IsMeleeAttacking(true);
            PlayerController.Knife.HoldKnifeInHand();
        }
    }

    private void OnMeleeCancelled(InputAction.CallbackContext context)
    {
        PlayerController.IsMeleeAttacking(false);
    }

    private void OnBlockingPerformed(InputAction.CallbackContext context)
    {
        if (!PlayerController.GetMeleeAttacking())
        {
            PlayerController.IsBlocking(true);
            PlayerController.Pan.HoldPanInHand();   
        }
    }

    private void OnBlockingCancelled(InputAction.CallbackContext context)
    {
        PlayerController.IsBlocking(false);
        PlayerController.Pan.SetPanOnBack();
    }

    private void OnDodgePerformed(InputAction.CallbackContext context)
    {
        PlayerController.IsDodging();
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

    private void OnReloadPerformed(InputAction.CallbackContext context)
    {
        StandardProjectileGun.StartReloading();
    }

    private void OnSwitchBulletPerformed(InputAction.CallbackContext context)
    {
        StandardProjectileGun.SwitchBulletType();
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        pauseMenu.PerformPause();
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (bulletShopNPC != null && bulletShopNPC.playerInRange)
        {
            bulletShopNPC.OnInteract();   
        }
        else if (upgradeMenuNPC != null && upgradeMenuNPC.playerInRange)
        {
            upgradeMenuNPC.OnInteract();   
        }
    }
}
