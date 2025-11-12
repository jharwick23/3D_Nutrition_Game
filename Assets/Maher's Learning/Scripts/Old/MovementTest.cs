using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(CharacterController))]
public class MovementTest : MonoBehaviour
{
    // -- // -- // Movement Variables -- // -- //
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 2f;
    [Tooltip("Set to true to enable sprinting with Shift key")]
    [SerializeField] private bool enableSprint = true;

    private CharacterController controller;
    private Animator animator;
    private bool isSprinting = false;
    private string currentAnimation = "";

    // -- // -- // Input Variables // -- // -- //

    Vector2 moveInput = Vector2.zero;
    Vector2 lookInput = Vector2.zero;

    // -- // -- // Camera Variables -- // -- //

    [Header("Camera Settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float mouseSensitivity = 0.2f;
    [Tooltip("Limits how far up/down you can look in degrees")]
    [SerializeField] private float maxLookAngle = 60f;

    // -- // -- // Third Person Camera Variables -- // -- //

    [Header("Third Person Camera")]
    [Tooltip("How far behind the player the camera should be")]
    [SerializeField] private float cameraDistance = 3f;
    [Tooltip("How high above the player the camera should be")]
    [SerializeField] private float cameraHeight = 2f;
    [Tooltip("Default camera angle in degrees, positive = looking down")]
    [SerializeField] private float defaultCameraAngle = 0f;
    [Tooltip("How smoothly the camera follows the player (higher = more responsive)")]
    [SerializeField] private float cameraSmoothness = 100f;

    // -- // -- // Jump & Gravity Variables -- // -- //

    [Header("Jump & Gravity")]
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -20f;

    private float currentVerticalRotation = 0f; // For camera high low
    private Vector3 cameraVelocity = Vector3.zero;
    private float verticalVelocity = 0f;
    private bool jumpRequested = false;
    private float yaw = 0f; // horizontal rotation tracked separately

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        ApplyMouseLook();
        ApplyMovement();
        CheckAnimationState();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
            jumpRequested = true;
    }

    void Start()
    {
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();

        // Unparent camera so its unaffected by player animations
        if (playerCamera != null && playerCamera.transform.IsChildOf(transform))
        {
            playerCamera.transform.SetParent(null);
        }

        // Players current transform yaw
        yaw = transform.eulerAngles.y;

        ToggleCursorLock(true);
        ChangeAnimation("MaherIdle");
    }

    void ToggleCursorLock(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    void ApplyMouseLook()
    {
        if (playerCamera == null) return;

        // Scale by mouse sensitivity
        Vector2 mouseDelta = lookInput * mouseSensitivity;

        // Accumulate yaw from mouse X and apply to the player's root rotation.
        yaw += mouseDelta.x;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Update camera vertical rotation (independent of player rotation)
        currentVerticalRotation -= mouseDelta.y;
        currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, -maxLookAngle, maxLookAngle);

        // Calculate desired camera position
        float totalAngle = defaultCameraAngle + currentVerticalRotation;

        // Pivot point for the camera (player position plus height)
        Vector3 pivot = transform.position + Vector3.up * cameraHeight;

        // Compute camera offset using yaw and pitch so camera doesn't inherit any transform parenting/animation
        Quaternion camRot = Quaternion.Euler(totalAngle, yaw, 0f);
        Vector3 cameraOffset = camRot * new Vector3(0f, 0f, -cameraDistance);
        Vector3 targetPosition = pivot + cameraOffset;

        // Smoothly move camera to target position
        playerCamera.transform.position = Vector3.SmoothDamp(
            playerCamera.transform.position,
            targetPosition,
            ref cameraVelocity,
            1f / cameraSmoothness
        );

        // Make camera look at player (slightly above player)
        Vector3 lookTarget = transform.position + Vector3.up * 1.2f;
        playerCamera.transform.LookAt(lookTarget);
    }

    void ApplyMovement()
    {
        // Check for sprint (only when moving forward)
        isSprinting = enableSprint && (Keyboard.current?.shiftKey.isPressed ?? false) && moveInput.y > 0.1f && Mathf.Abs(moveInput.x) < 0.3f;
        
        // Movement relative to the player's orientation
        Vector3 right = transform.right;
        Vector3 forward = transform.forward;
        Vector3 desired = right * moveInput.x + forward * moveInput.y;
        if (desired.sqrMagnitude > 1f) desired.Normalize();
        
        // Apply sprint multiplier only when moving forward
        float currentSpeed = isSprinting ? walkSpeed * sprintMultiplier : walkSpeed;
        Vector3 move = desired * currentSpeed;

        // Gravity and jumping
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0f)
                verticalVelocity = -2f; // Small downward force to keep grounded
            if (jumpRequested)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                ChangeAnimation("Jump Up");
                jumpRequested = false;
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;
        controller.Move(move * Time.deltaTime);
    }

    private void ChangeAnimation(string newAnimation, float crossfade = 0.2f)
    {
        if (currentAnimation == newAnimation) return;
        animator.CrossFade(newAnimation, crossfade);
        currentAnimation = newAnimation;
    }
    
    private void CheckAnimationState()
    {


        // Handle jump/fall animations
        if (!controller.isGrounded)
        {
            if (verticalVelocity > 0f)
            {
                // ChangeAnimation("Jump Up");
                return;
            }
            else if (verticalVelocity < 0f)
            {
                // ChangeAnimation("Falling");
                return;
            }
        }

        // Idle if no input
        if (moveInput.magnitude < 0.1f)
        {
            ChangeAnimation("MaherIdle");
            return;
        }

        // Determine direction from input: x = right, y = forward
        float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg; // -180-180, 0 = right
        float angleNorm = (angle + 360f) % 360f; // 0-360 degrees

        string anim = "MaherIdle";

        // Check if we're sprinting forward first
        if (isSprinting)
        {
            anim = "Run Forward";
        }
        // Otherwise map angle to 8 directions for walking
        else if (angleNorm >= 337.5f || angleNorm < 22.5f)
            anim = "Walk Right";
        else if (angleNorm >= 22.5f && angleNorm < 67.5f)
            anim = "Walk Forward Right";
        else if (angleNorm >= 67.5f && angleNorm < 112.5f)
            anim = "Walk Forward";
        else if (angleNorm >= 112.5f && angleNorm < 157.5f)
            anim = "Walk Forward Left";
        else if (angleNorm >= 157.5f && angleNorm < 202.5f)
            anim = "Walk Left";
        else if (angleNorm >= 202.5f && angleNorm < 247.5f)
            anim = "Walk Backward Left";
        else if (angleNorm >= 247.5f && angleNorm < 292.5f)
            anim = "Walk Backward";
        else if (angleNorm >= 292.5f && angleNorm < 337.5f)
            anim = "Walk Backward Right";

        ChangeAnimation(anim);
    }

}

