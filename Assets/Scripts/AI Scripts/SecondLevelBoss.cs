using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;


public class BossController : MonoBehaviour
{
    public enum BossState { PhaseOne, PhaseTwo, Charging, Stunned }
    public BossState currentState;
    public int maxHealth = 100;
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
    private NavMeshAgent agent;
    private Transform player;
    private bool isCharging = false;
    private Vector3 chargeDirection;
    public Slider healthSlider;
    [SerializeField]private AIEnemy newDamage;

    //Gets player automatically and begins phase one.
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        currentHealth = maxHealth;
        currentState = BossState.PhaseOne;

        StartCoroutine(ShootRoutine());
        StartCoroutine(MinionMaintenance());
    }

    //Handles the charging movement and its interval
    void Update()
    {
        if (currentState == BossState.Charging)
        {
            ChargeMovement();
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
        rb.linearVelocity = direction * 10f;

        Vector3 direction2 = (player.position - firePointRight.position).normalized;
        Rigidbody rb2 = bullet2.GetComponent<Rigidbody>();
        rb2.linearVelocity = direction2 * 10f;
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

            yield return new WaitForSeconds(1f);
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

    //Handles actual value damage change
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthSlider.value = currentHealth;

        if (currentHealth <= maxHealth / 2 && currentState == BossState.PhaseOne)
        {
            StopAllCoroutines();

            newDamage.enabled = true;
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

        StartCoroutine(ChargeLoop());
    }

    //The initial charge loop, waits for a cooldown
    IEnumerator ChargeLoop()
    {
        while (currentState == BossState.PhaseTwo)
        {
            yield return new WaitForSeconds(chargeCooldown);

            StartCharge();
            Debug.Log("Charge Started");
        }
    }

    //Handles charge initialization
    void StartCharge()
    {
        currentState = BossState.Charging;

        agent.isStopped = true;

        chargeDirection = (player.position - transform.position).normalized;

        isCharging = true;
    }

    //Handles numerical charge speed
    void ChargeMovement()
    {
        if (!isCharging) return;

        transform.position += chargeDirection * chargeSpeed * Time.deltaTime;
    }

    //Sets charge flag to false
    void StopCharge()
    {
        isCharging = false;
        currentState = BossState.PhaseTwo;
    }


    //Damage to player while also pushing away to avoid continuos damage, also handles stun if boss crashes
    void OnCollisionEnter(Collision collision)
    {
        if (currentState != BossState.Charging)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            //Movement displacement to prevent continious damage TODO
            
            PlayerControllerV2 playerController = collision.gameObject.GetComponent<PlayerControllerV2>();
            if (playerController != null)
            {
                if (!playerController.GetBlocking())
                {
                    playerController.TakeDamage(10);
                }
            }

            StopCharge();
        }
        else if (collision.gameObject.CompareTag("StunWall"))
        {
            StartCoroutine(Stun());
        }
        else 
        {
            StopCharge();
        }
    }

    //Simple time wait state change to display stun behavior
    IEnumerator Stun()
    {
        isCharging = false;
        currentState = BossState.Stunned;

        yield return new WaitForSeconds(stunDuration);

        currentState = BossState.PhaseTwo;
    }

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
