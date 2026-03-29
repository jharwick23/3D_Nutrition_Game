using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AIEnemyFourthBoss : AIEnemy
{
    private Transform target;
    private enum BossPhase { FirstPhase, SecondPhase, ThirdPhase };
    private BossPhase currentPhase;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float firstPhaseBulletSpeed, secondPhaseBulletSpeed, thirdPhaseBulletSpeed;
    [SerializeField] private float firstPhaseSwicthRate, thirdPhaseSwicthRate, liquidChangeRate;
    [SerializeField] private float firstPhaseBulletRate, secondPhaseBulletRate, thirdPhaseBulletRate;
    [SerializeField] private GameObject firstSetOfSickRays, secondSetOfSickRays;
    [SerializeField] private GameObject[] ListOfSwicthSpots;
    [SerializeField] private GameObject SecondPhaseSwitchSpot;
    private bool easyRaySwicth = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Sets camara follow for target looking and other functions
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject camaratarget = player.transform.GetChild(2).GetChild(0).gameObject;
        target = camaratarget.transform;
        currentPhase = BossPhase.FirstPhase;
        FirstPhaseHandling();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;
        transform.rotation = Quaternion.LookRotation(direction);

        PhaseControl();

        
    }

    //Checks for enemy health and does phase transitioning
    private void PhaseControl()
    {
        if (enemyHealth < 1000 && enemyHealth > 500 )
        {
            if (currentPhase != BossPhase.SecondPhase)
            {
                currentPhase = BossPhase.SecondPhase;
                StopAllCoroutines();
                SecondPhaseHandling();
            }
        }
        else if (enemyHealth <= 500 )
        {
            if (currentPhase != BossPhase.ThirdPhase)
            {
                currentPhase = BossPhase.ThirdPhase;
                StopAllCoroutines();
                ThirdPhaseHandling();
            }
        }
    }

    //All first phase handling
    void FirstPhaseHandling()
    {
        Debug.Log("Attempted FirstPhase");
        StartCoroutine(SwitchFunction(firstPhaseSwicthRate));
        StartCoroutine(Shooting(firstPhaseBulletSpeed, firstPhaseBulletRate));
    }

    //All second phase handling
    void SecondPhaseHandling()
    {
        Debug.Log("Second Phase");
        SetImmunity(false);
        transform.position = SecondPhaseSwitchSpot.transform.position;
        StartCoroutine(HealthTick());
        StartCoroutine (Shooting(secondPhaseBulletSpeed, secondPhaseBulletRate));
    }
    
    //All third phase handling
    void ThirdPhaseHandling()
    {
        Debug.Log("Third Phase");
        SetImmunity(true);
        StartCoroutine (Shooting(thirdPhaseBulletSpeed, thirdPhaseBulletRate));
        StartCoroutine(SwitchFunction(thirdPhaseSwicthRate));
        StartCoroutine(RayOfLiquidChangeInterval(liquidChangeRate));
    }

    //Second phase health tick, Currently: 25 Second phase
    IEnumerator HealthTick()
    {
        while (true)
        {
            enemyHealth -= 10;
            UpdateUI();
            //Debug.Log("TickDamage");
            yield return new WaitForSeconds(0.5f);
        }
    }

    //Switch between two groups of rays
    IEnumerator RayOfLiquidChangeInterval(float changeSpeed)
    {
        while (true)
        {
            easyRaySwicth = !easyRaySwicth;
            firstSetOfSickRays.SetActive(easyRaySwicth);
            secondSetOfSickRays.SetActive(!easyRaySwicth);
            yield return new WaitForSeconds(liquidChangeRate);
        }
    }

    //Switch function for teleport
    IEnumerator SwitchFunction(float switchS)
    {
        while (true)
        {
            int length = ListOfSwicthSpots.Length;
            int randomIndex = Random.Range(0, ListOfSwicthSpots.Length);
            GameObject randomRay = ListOfSwicthSpots[randomIndex];

            transform.position = randomRay.transform.position;
            yield return new WaitForSeconds(switchS);
        }
    }
    
    //Handles shooting for all three phases, takes speed and rate of fire
    IEnumerator Shooting(float speed, float rate)
    {
        while (true)
        {
            Shoot(speed);
            yield return new WaitForSeconds(rate);
        }
    }

    //General shooting function
    private void Shoot(float speed)
    {
        Vector3 spawnPos = transform.position + transform.forward * 4f;
        GameObject currBullet = Instantiate(bullet, spawnPos, Quaternion.identity);
        Rigidbody rbb = currBullet.GetComponent<Rigidbody>();
        Vector3 dir = (target.position - transform.position).normalized;
        
        rbb.angularVelocity = new Vector3( Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        rbb.linearVelocity = dir * speed;


        if (currentPhase == BossPhase.SecondPhase || currentPhase == BossPhase.ThirdPhase)
        {
            FourthBossBullet currSript = currBullet.gameObject.GetComponent<FourthBossBullet>();
            currSript.ChangeBulletType(false);
            currSript.timer = 2f;
            currSript.MakeBulletDestroy();
        }
    }

    //Destroys all liquidrays
    private void OnDestroy()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
    }
}
