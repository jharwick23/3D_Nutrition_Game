using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class OrangeBullet : MonoBehaviour
{
    private Rigidbody rb;
    public float Speed = 100f;
    public int bulletDamage = 10;
    public float BulletDrop = 5f; // Gravity 
    public float LifeTime = 5f;
    public int maxAmmo = 24;
    public float timeBetweenShooting = 0.2f;
    public GameObject ImpactDecalPrefab;
    public ProjectileSFX projectileSFX;

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

    void Start()
    {
        projectileSFX = GetComponent<ProjectileSFX>();
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
        // Ignore player collisions
        if (collision.gameObject.CompareTag("Player") || (collision.transform.parent != null && collision.transform.parent.CompareTag("Player")))
        {
            return;
        }
        
        // Impact decal logic
        ContactPoint contact = collision.contacts[0];

        Vector3 hitPoint = contact.point;
        Vector3 hitNormal = contact.normal;

        Quaternion hitRotation = Quaternion.LookRotation(hitNormal);
        Vector3 spawnPosition = hitPoint + hitNormal * 0.01f; // Slight offset to avoid z-fighting

        if (ImpactDecalPrefab != null)
        {
            Instantiate(
                ImpactDecalPrefab,
                spawnPosition,
                hitRotation     
            );
        }
        
        // Enemey damage logic
        bool isEnemy = collision.gameObject.CompareTag("Enemy");

        if (isEnemy && (collision.gameObject.GetComponent<AIEnemy>() != null) )
        {
            collision.gameObject.GetComponent<AIEnemy>().DoDamage(bulletDamage);
            collision.gameObject.GetComponent<AIEnemy>().UpdateUI();
            collision.gameObject.GetComponent<AIEnemy>().DoDeath();
        }

        projectileSFX.Play();
        Destroy(gameObject);
    }
}
