using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StandardBullet : MonoBehaviour
{
    private Rigidbody rb;
    public float Speed = 100f;
    public float BulletDrop = 5f; // Gravity 
    public float LifeTime = 5f;
    public int maxAmmo = 24;
    public GameObject ImpactDecalPrefab;

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

    void OnCollisionEnter(Collision collision)
    {
        // Add impact effects or damage logic here if needed
        ContactPoint contact = collision.contacts[0];

        Vector3 hitPoint = contact.point;
        Vector3 hitNormal = contact.normal;

        Quaternion hitRotation = Quaternion.LookRotation(hitNormal);
        Vector3 spawnPosition = hitPoint + hitNormal * 0.01f; // Slight offset to avoid z-fighting

        Instantiate(
            ImpactDecalPrefab,
            spawnPosition,
            hitRotation     
        );

        Destroy(gameObject);
    }
}
