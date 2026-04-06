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

    [SerializeField] private Transform target, spawnpos1, spawnpos2;
    [SerializeField] private GameObject enemy, bullet, slamVFX;
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
        float radius = 2.5f;
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

    IEnumerator SlamAttack()
    {
        yield return new WaitForSeconds(0.4f);

        animator.SetTrigger("SlamJumpRequested");

        float upwardVelocity = 35f;
        float gravity = -60f;
        float moveSpeed = 16f;
        float airControl = 0.6f;

        Vector3 toPlayer = target.position - transform.position;
        toPlayer.y = 0f;

        float distance = toPlayer.magnitude;
        Vector3 dir = toPlayer.normalized;

        // dynamic offset so it doesn't land on player
        float safeRadius = 0.01f;

        Vector3 targetPos = target.position - dir * safeRadius;

        Vector3 velocity = Vector3.up * upwardVelocity;

        bool grounded = false;

        while (!grounded)
        {
            // horizontal movement toward locked position
            Vector3 moveDir = (targetPos - transform.position).normalized * moveSpeed;

            // smoother air control (prevents overshoot)
            velocity.x = Mathf.Lerp(velocity.x, moveDir.x, airControl);
            velocity.z = Mathf.Lerp(velocity.z, moveDir.z, airControl);

            // gravity
            velocity.y += gravity * Time.deltaTime;

            // move
            transform.position += velocity * Time.deltaTime;

            // ground check
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f))
            {
                float offset = GetComponent<Collider>().bounds.extents.y;

                if (transform.position.y <= hit.point.y + offset + 0.05f)
                {
                    // snap cleanly to ground (no sinking)
                    transform.position = hit.point + Vector3.up * (offset + 0.25f);

                    grounded = true;
                }
            }

            yield return null;
        }

        // small delay after landing (impact feel)
        if(slamVFX != null)
        {
            Instantiate(slamVFX, transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(0.4f);

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

        float timer = 0f;
        //Spinning and enabling if the liquid effect which have damage handling
        StartLiquidSound();
        LiquidHandling(true);

        while (timer < SpinTimer)
        {
            transform.Rotate(Vector3.up * 300f * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

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
