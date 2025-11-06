/*using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;

    CharacterController controller;

    Vector2 moveInput = Vector2.zero;
    Vector2 lookInput = Vector2.zero;

    [Header("Camera Settings")]
    public Camera playerCamera;
    public float mouseSensitivity = 0.2f;
    [Tooltip("Limits how far up/down you can look in degrees")]
    public float maxLookAngle = 60f;
    
    [Header("Third Person Camera")]
    [Tooltip("How far behind the player the camera should be")]
    public float cameraDistance = 5f;
    [Tooltip("How high above the player the camera should be")]
    public float cameraHeight = 2f;
    [Tooltip("Default camera angle in degrees, positive = looking down")]
    public float defaultCameraAngle = 0f;
    [Tooltip("How smoothly the camera follows the player (higher = more responsive)")]
    public float cameraSmoothness = 12f;

    private float currentVerticalRotation = 0f; // For camera high low
    private Vector3 cameraVelocity = Vector3.zero;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        ApplyMouseLook();
        ApplyMovement();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void Start()
    {
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();

        ToggleCursorLock(true);
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

        // Rotate the player left/right
        transform.Rotate(Vector3.up * mouseDelta.x);

        // Update camera vertical rotation
        currentVerticalRotation -= mouseDelta.y;
        currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, -maxLookAngle, maxLookAngle);

        // Calculate desired camera position
        float totalAngle = defaultCameraAngle + currentVerticalRotation;
        
        // Calculate the position behind and above the player
        Vector3 targetPosition = transform.position
            - transform.forward * cameraDistance * Mathf.Cos(totalAngle * Mathf.Deg2Rad)  // Back
            + Vector3.up * (cameraHeight + cameraDistance * Mathf.Sin(totalAngle * Mathf.Deg2Rad)); // Up

        // Smoothly move camera to target position
        playerCamera.transform.position = Vector3.SmoothDamp(
            playerCamera.transform.position,
            targetPosition,
            ref cameraVelocity,
            1f / cameraSmoothness
        );

        // Make camera look at player (slightly above player)
        Vector3 lookTarget = transform.position + Vector3.up * 1f;
        playerCamera.transform.LookAt(lookTarget);
    }

    void ApplyMovement()
    {
        // Movement relative to the player's orientation
        Vector3 right = transform.right;
        Vector3 forward = transform.forward;

        Vector3 desired = right * moveInput.x + forward * moveInput.y;
        if (desired.sqrMagnitude > 1f) desired.Normalize();

        Vector3 move = desired * walkSpeed;
        controller.Move(move * Time.deltaTime);
    }
}
*/