using UnityEngine;
using static UnityEditor.PlayerSettings;

public class AiEnemyTurretCoke : AIEnemy
{
    private bool LOS;
    public Transform target;
    public Transform shootingTarget;
    [SerializeField] public float attackDistance;
    public LayerMask obstructionMask;
    [SerializeField] private float height = 1f, bobSpeed = 2f, bobAmount = 0.3f, rotationScaler = 1f, frontScale = 1.5f, bulletSpeed = 10f;
    [SerializeField] private GameObject cokeBulletRay;
    private float fireRate = 0;
    [SerializeField] private float shootingRate;
    private PlayerControllerV2 player;
    public AudioClip cokeBullet;
    public AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = FindAnyObjectByType<PlayerControllerV2>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = height + Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        transform.position = pos;

        Vector3 origin = transform.position;
        float distance = Vector3.Distance(origin, target.position);

        //For Debuging to see if LOS is even being checked
        //Debug.DrawLine(origin, target.position, Color.red);

        LOS = false;

        //Determines if Player is LOS and sets bool
        if (Physics.Linecast(origin, target.position, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Player"))
            {
                LOS = true;
            }
        }
        //Debug.Log(1f / Time.deltaTime);
        
        //Attack pattern
        if (distance < attackDistance && LOS && !(player.IsDead()))
        {

            //Fixes looking issue, now looks at target after stop
            Vector3 lookDirection = (target.position - transform.position).normalized;
            lookDirection.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationScaler);

            if (fireRate <= Time.time)
            {
                Shoot();
                fireRate = Time.time + shootingRate;
            }


        }

        //Checks if dead to prevent constant hit bug
        if (player.IsDead())
        {
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            //Debug.Log("Bullet Destroying");
            foreach (GameObject bullet in bullets)
            {
                Destroy(bullet);
            }
        }
    }

    //Handles shooting, very high speeds to look like ray
    void Shoot()
    {
        // Spawn slightly in front of enemy
        Vector3 spawnPos = transform.position + transform.forward * frontScale;
        Vector3 direction = (shootingTarget.position - spawnPos).normalized;
        direction.y = direction.y + 0.05f;
        Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90f, 0f, 0f);
        GameObject bullet = Instantiate(cokeBulletRay, spawnPos, rotation);
        
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * bulletSpeed;
        Destroy(bullet, 2f);
        PlayShootingSound();
    }
    
    void PlayShootingSound()
    {
        if (cokeBullet)
        {
            audioSource.PlayOneShot(cokeBullet, 0.2f);
        }
    }
}
