using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSmoothTime = 0.1f;
    public float sprintMultiplier = 2f;
    public float gravity = -9.81f;
    public float jumpHeight = 1f;

    private CharacterController controller;
    private Animator animator;
    private Transform cam;

    private Vector2 moveInput;
    private Vector3 velocity;
    private float turnSmoothVelocity;

    private bool isGrounded;
    private bool jumpPressed;
    private bool sprintPressed;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cam = Camera.main.transform;
    }

    public void Walk(InputAction.CallbackContext context)
    {
        // If WASD/Arrow keys are pressed set the moveinput to the Vector2 value
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        // If spacebar is pressed set jumpPressed to true
        if (context.performed)
        {
            jumpPressed = true;
            animator.SetBool("JumpRequested", true);
        }
    }
    
    public void Sprint(InputAction.CallbackContext context)
    {
        sprintPressed = context.ReadValue<float>() > 0;
    }

    private void Update()
    {
        isGrounded = controller.isGrounded;
        animator.SetBool("IsGrounded", isGrounded);

        // Reset the vertical velocity when you hit the ground
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Calculate movement direction
        Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        // Compute target speed value for Animator
        float targetSpeed = inputDir.magnitude * (sprintPressed ? sprintMultiplier : 1f);

        // Set Blend Tree parameter
        animator.SetFloat("Speed", targetSpeed, 0.1f, Time.deltaTime);

        if (inputDir.magnitude >= 0.1f)
        {
            // Camera-relative direction
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // Rotate player smoothly toward movement direction
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Move in that direction
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            float speed = sprintPressed ? moveSpeed * sprintMultiplier : moveSpeed;
            controller.Move(speed * Time.deltaTime * moveDir.normalized);
        }

        // Apply Jump
        if (isGrounded && jumpPressed)
        {
            animator.SetBool("JumpRequested", false);
            jumpPressed = false;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
