using UnityEngine;

// Reminder for me (maher): Currently using new code on old method. Using (old) shoulder empty part
// as the Follow Target for the camera. But using new camera positioning code and logic (.rotation not LookAt).
// Good Point : 1.5, 0.6, 0
// New Good Point: 0.6, 1.5, 0
public class CameraControllerV2 : MonoBehaviour
{
    public Transform FollowTarget;
    public float FollowSpeed = 1000f;

    [Tooltip("How much to pitch the camera down in degrees")]
    public float CameraPitch = 0f; // Starting Camera Pitch/Angle degrees, Changes with Mouse Y movement
    [Tooltip("Camera offset from the follow target position")]
    public Vector3 ShoulderOffset = new Vector3(0.0f, 0.0f, 0.0f); // 0,0,0 for Old // Old changes part Position
    public float CameraDistance = 9f;

    private Vector3 _currentVelocity = Vector3.zero;

    void LateUpdate()
    {
        if (FollowTarget == null) return;

        // Old // Follow Target is Shoulder Part
        // Quaternion rotation = Quaternion.Euler(CameraPitch, FollowTarget.eulerAngles.y, 0f);
        // Vector3 targetPosition = FollowTarget.position + rotation * CameraOffset - rotation * Vector3.forward * CameraDistance;

        // New // Follow Target is Character
        Quaternion yawRotation = Quaternion.Euler(0f, FollowTarget.eulerAngles.y, 0f);
        Quaternion rotation = Quaternion.Euler(CameraPitch, FollowTarget.eulerAngles.y, 0f);
        Vector3 shoulderWorldPos = FollowTarget.position + yawRotation * ShoulderOffset;
        Vector3 targetPosition = shoulderWorldPos - rotation * Vector3.forward * CameraDistance;


        // Move camera to target position smoothly
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref _currentVelocity,
            1f / FollowSpeed
        );

        // Old
        // transform.LookAt(FollowTarget.position + FollowTarget.forward * 2f);

        // New
        transform.rotation = rotation;
    }
}
