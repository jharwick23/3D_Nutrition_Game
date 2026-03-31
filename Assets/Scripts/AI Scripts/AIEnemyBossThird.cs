using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class AIEnemyBossThird : AIEnemy
{
    private AIEnemy healthScript;
    private enum bossStage {FirstPhase, SecondPhase }
    private enum bossAttack { SlamAttack, Cooldown, LiquidShot, EnemySpawn, NoAttack}
    private bossAttack currentAttack;
    private bossStage currentStage;
    private bool slam = true, liquidShot = true, enemySpawn = true, enemiesDead = true;
    [SerializeField] float slamTimer, shootTimer, spawnTimer, SpinTimer;
    private GameObject[] enemies = new GameObject[2];
    private Rigidbody rb;

    [SerializeField] private Transform target, spawnpos1, spawnpos2;
    [SerializeField] private GameObject enemy, bullet;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip greaseSound;
    [SerializeField] private Animator animator;
    

    // initialized the boss
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        healthScript = GetComponent<AIEnemy>();
        currentStage = bossStage.FirstPhase;
        currentAttack = bossAttack.NoAttack;
        rb = GetComponent<Rigidbody>();
        StartCoroutine(MinionMaintanence());
    }

    // Update called for behaviors 
    void Update()
    {
        //Handles regular looking for all attacks execpt liquid shooting
        if (currentAttack != bossAttack.LiquidShot)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0f;
            transform.rotation = Quaternion.LookRotation(direction);

        }
        
        //Checks health for phase transitioning
        if (currentStage != bossStage.SecondPhase && healthScript.enemyHealth <= 300)
        {
            currentStage = bossStage.SecondPhase;
        }

        //Handles function calling based on states
        if (currentStage == bossStage.FirstPhase)
        {
            if (currentAttack == bossAttack.NoAttack)
            {
                currentAttack = bossAttack.SlamAttack;
                StartCoroutine(SlamAttack());
            }
        }
        else
        {
            if (currentAttack == bossAttack.NoAttack)
            {

                if (enemySpawn && enemiesDead)
                {
                    currentAttack = bossAttack.EnemySpawn;
                    EnemySpawn();
                }
                else if (liquidShot)
                {
                    liquidShot = false;
                    currentAttack = bossAttack.LiquidShot;
                    StartCoroutine(Shoot());
                }
                else if (slam)
                {
                    slam = false;
                    currentAttack = bossAttack.SlamAttack;
                    StartCoroutine (SlamAttack());
                }
            }
        }

        // Check if Boss is grounded for jump
        float radius = 0.5f;
        float distance = 1.5f;

        bool grounded = Physics.SphereCast(
            transform.position + Vector3.up * 0.1f,
            radius,
            Vector3.down,
            out RaycastHit hit,
            distance
        );

        animator.SetBool("IsGrounded", grounded);
    }


    //Spawns two burger enemies and sets them active, sets a timer routine
    public void EnemySpawn()
    {

        //Debug.Log("Attempting Spawning");
        GameObject enemy1 = Instantiate(enemy, spawnpos1);
        GameObject enemy2 = Instantiate(enemy, spawnpos2);

        enemy1.SetActive(true);
        enemy2.SetActive(true);

        enemies[0] = enemy1;
        enemies[1] = enemy2;
        enemySpawn = false;
        currentAttack = bossAttack.NoAttack;

        StartCoroutine(SetTimer(spawnTimer, "enemySpawn"));
        
    }


    //Handles minion checking for more spawning
    IEnumerator MinionMaintanence()
    {
        while (true)
        {
            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    //Debug.Log("Null");
                    enemiesDead = false;
                    break;
                }
                else
                {
                    enemiesDead=true;
                    break;
                }
            }
            yield return new WaitForSeconds(5f);
        }
    }

    //Refactored Timer 
    IEnumerator SetTimer(float timer, string type)
    {
        yield return new WaitForSeconds(timer);
        
        switch (type)
        {
            case "slam":
                slam = true;
                break;
            case "shoot":
                liquidShot = true;
                break;
            case "enemySpawn":
                enemySpawn = true;
                break;
            default:
                break;
        }
    }

    //Coreroutine for small attack
    IEnumerator SlamAttack()
    {
        //Reset any velocity
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;  

        animator.SetTrigger("SlamJumpRequested");

        //Add force for jump
        rb.AddForce(Vector3.up * 800f, ForceMode.Impulse);
        //Debug.Log("Attmepted slam");


        yield return new WaitForSeconds(2f);

        //Add force for slam
        rb.linearVelocity = Vector3.zero;
        Vector3 distance = (target.position - transform.position).normalized;
        Vector3 SlamForce = (distance * 3000f) + (Vector3.down * 500f);
        rb.AddForce(SlamForce, ForceMode.Impulse);

        yield return new WaitForSeconds(2f);

        currentAttack = bossAttack.NoAttack;
        if (currentStage == bossStage.SecondPhase)
        {
            StartCoroutine(SetTimer(slamTimer, "slam"));
        }
        
    }

    //Does shooting, looks complex but not
    IEnumerator Shoot()
    {
        animator.SetBool("IsSpinning", true);
        //Debug.Log("Attempting shot");

        //Spinning and enabling if the liquid effect which have damage handling
        float timer = 0f;
        rb.angularDamping = 0f;
        StartLiquidSound();
        LiquidHandling(true);
        while (timer < SpinTimer)
        {
            rb.angularVelocity = Vector3.up * 5f;

            timer += Time.deltaTime;
            yield return null;
        }
        rb.angularDamping = 0.05f;
        rb.angularVelocity = Vector3.zero;
        LiquidHandling(false);
        StopLiquidSound();

        animator.SetBool("IsSpinning", false);

        currentAttack = bossAttack.NoAttack;
        StartCoroutine(SetTimer(shootTimer, "shoot"));
    }
    
    //Enables and disables the liquid rays if grease
    private void LiquidHandling(bool set)
    {
        bullet.SetActive(set);
    }

    //Destroy Handling
    private void OnDestroy()
    {
        foreach (var gameObject in enemies)
        {
            Destroy(gameObject);
        }
    }

    void StartLiquidSound()
    {
        if (greaseSound == null) return;

        audioSource.clip = greaseSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    void StopLiquidSound()
    {
        audioSource.Stop();
    }
}
