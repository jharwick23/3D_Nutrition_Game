using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StandardBullet : MonoBehaviour
{
    private Rigidbody rb;
    public float Speed = 100f;
    public float BulletDrop = 5f; // Gravity 
    public float LifeTime = 5f;

    private Vector3 _velocity;

    // prev non manual grav
    // void Awake()
    // {
    //     _rigidbody = GetComponent<Rigidbody>();
    //     _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    //     _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    // }

    // public void Init(Vector3 direction)
    // {
    //     _rigidbody.linearVelocity = direction.normalized * Speed;
    //     // Destroy(gameObject, LifeTime);
    // }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // manual gravity
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void Init(Vector3 shootDirection)
    {
        rb.linearVelocity = shootDirection * Speed;
        Destroy(gameObject, LifeTime);
    }

    void FixedUpdate()
    {
        rb.linearVelocity += Vector3.down * BulletDrop * Time.fixedDeltaTime;
    }
}
