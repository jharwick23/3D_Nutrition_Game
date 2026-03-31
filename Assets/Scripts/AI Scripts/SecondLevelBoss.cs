using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;


public class BossController : MonoBehaviour
{
    public enum BossState { PhaseOne, PhaseTwo, Charging, Stunned }
    public BossState currentState;
    public int maxHealth = 1000;
    private int currentHealth;
    public GameObject[] minionPrefabs;
    public Transform[] minionSpawnPoints;
    public int maxMinionsAlive = 4;
    public int minionDamageToBoss = 5;
    private List<GameObject> activeMinions = new List<GameObject>();
    public Transform firePointLeft;
    public Transform firePointRight;
    public GameObject projectilePrefab;
    public float shootInterval = 2f;
    public float chargeSpeed = 20f;
    public float stunDuration = 3f;
    public float playerDamage = 20f;
    public float knockbackForce = 6f;
    public float chargeCooldown = 2f;
    private Transform player;
    private Vector3 chargeDirection;
    public Slider HealthSlider;
    private Rigidbody rbb;
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip sugarBulletAudio, crashAudio;

    void Awake()
    {
        if(!animator)
        {
            animator = GetComponent<Animator>();
        }
    }

    //Gets player automatically and begins phase one.
    void Start()
    {
        rbb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        currentHealth = maxHealth;
        currentState = BossState.PhaseOne;

        StartCoroutine(ShootRoutine());
        StartCoroutine(MinionMaintenance());
    }

    //Handles the charging movement and its interval
    void FixedUpdate()
    {
        if (currentState == BossState.PhaseTwo)
        {
            currentState = BossState.Charging;
            chargeDirection = player.position - rbb.position;
            chargeDirection.y = 0f;
            chargeDirection.Normalize();

        }
        if (currentState == BossState.Charging)
        {

            animator.SetBool("IsInjured", false);
            animator.SetBool("IsRunning", true);

            Vector3 newPosition = rbb.position + chargeDirection * chargeSpeed * Time.fixedDeltaTime;
            rbb.MovePosition(newPosition);

            if (chargeDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(chargeDirection);
                rbb.MoveRotation(targetRotation);
            }
        }
    }

    void PlayCrashSound()
    {
        if (crashAudio)
        {
            audioSource.PlayOneShot(crashAudio);
        }
    }

    void PlayShootingSound()
    {
        if (sugarBulletAudio)
        {
            audioSource.PlayOneShot(sugarBulletAudio, 0.5f);
        }
    }

    //Routine to shoot while in phase one
    IEnumerator ShootRoutine()
    {
        while (currentState == BossState.PhaseOne)
        {
            Shoot();
            yield return new WaitForSeconds(shootInterval);
        }
    }

    //Shoots player from two angles, can be made one if two is not necessary
    void Shoot()
    {
        GameObject bullet = Instantiate(projectilePrefab, firePointLeft.position, firePointLeft.rotation);
        GameObject bullet2 = Instantiate(projectilePrefab, firePointRight.position, firePointRight.rotation);
        bullet.SetActive(true);
        bullet2.SetActive(true);

        Vector3 direction = (player.position - firePointLeft.position).normalized;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * 100f;

        Vector3 direction2 = (player.position - firePointRight.position).normalized;
        Rigidbody rb2 = bullet2.GetComponent<Rigidbody>();
        rb2.linearVelocity = direction2 * 100f;
        PlayShootingSound();
    }

    //Handles phase one minion enemy control
    IEnumerator MinionMaintenance()
    {
        while (currentState == BossState.PhaseOne)
        {
            activeMinions.RemoveAll(m => m == null);

            if (activeMinions.Count < maxMinionsAlive)
            {
                SpawnMinion();
            }

            yield return new WaitForSeconds(2f);
        }
    }

    //Spawns an enemy from the three main current enemies
    void SpawnMinion()
    {
        int randomEnemy = Random.Range(0, minionPrefabs.Length);
        int randomSpawn = Random.Range(0, minionSpawnPoints.Length);

        

        GameObject minion = Instantiate(
            minionPrefabs[randomEnemy],
            minionSpawnPoints[randomSpawn].position,
            Quaternion.identity
        );

        if (randomEnemy == 0)
        {
            minion.GetComponent<AIEnemyS>().height = 28;
        }

        activeMinions.Add(minion);

        MinionLink link = minion.AddComponent<MinionLink>();
        link.boss = this;
        minion.SetActive(true);
    }

    //Deals damage to enemy via minion
    public void MinionDied()
    {
        TakeDamage(minionDamageToBoss);
    }

    //Handles actual value damage change, Adds new damage script for phase 2
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        HealthSlider.value = currentHealth;

        if (currentHealth <= maxHealth / 2 && currentState == BossState.PhaseOne)
        {
            StopAllCoroutines();
            
            AIEnemy link = this.AddComponent<AIEnemy>();
            link.enemyHealth = currentHealth;
            link.healthSlider = HealthSlider;
            link.healthSlider.maxValue = maxHealth;
            link.healthSlider.value = currentHealth;
            link.isCube = true;
            StartCoroutine(EnterPhaseTwo());
        }
    }

    //Phase two control, triggered by lower tahn 50 percent health, kills all minions
    IEnumerator EnterPhaseTwo()
    {
        currentState = BossState.PhaseTwo;

        foreach (GameObject m in activeMinions)
        {
            if (m != null)
                Destroy(m);
        }
        Debug.Log("Entered phase two");

        activeMinions.Clear();

        yield return new WaitForSeconds(1f);

        currentState = BossState.PhaseTwo;
    }

    //Damage to player while also pushing away to avoid continuos damage, also handles stun if boss crashes
    void OnTriggerEnter(Collider other)
    {
         if (currentState != BossState.Charging)
            return;
 
         if (other.gameObject.CompareTag("Player"))
         {
            //Movement displacement to prevent continious damage TODO / Maybe
            
            PlayerControllerV2 playerController = other.gameObject.GetComponent<PlayerControllerV2>();
            if (playerController != null)
            {
                if (!playerController.GetBlocking())
                {
                    playerController.TakeDamage(10);
                }
            }

            //TODO: Stun boss maybe, to give time window
         }
         else if (other.gameObject.CompareTag("StunWall"))
         {
             StartCoroutine(StunDuration(5f));
         }
         else if (other.gameObject.CompareTag("Wall"))
         {
            StartCoroutine(StunDuration(2f));
         }
    }

    IEnumerator StunDuration(float time)
    {
        currentState = BossState.Stunned;

        animator.SetBool("IsInjured", true);
        PlayCrashSound();

        yield return new WaitForSeconds(time);
        currentState = BossState.PhaseTwo;
    }

    //Simple time wait state change to display stun behavior

    //Destroys enemies when dead such as enemy reset via player death
    void OnDestroy()
    {
        GameObject[] minions = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject m in minions)
        {
            Destroy(m);
        }

    }


}
