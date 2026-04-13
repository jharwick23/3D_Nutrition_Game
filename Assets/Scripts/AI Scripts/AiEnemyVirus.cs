using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AiEnemyVirus : AIEnemy
{
    [SerializeField] private Transform target;
    [SerializeField] private GameObject self;
    [SerializeField] private LayerMask losMask;
    [SerializeField] private float dashForce, interval, height, bobSpeed, bobAmount;
    private bool LOS = false, following = false, dashCooldown = false, SubSpawn = true; 
    private NavMeshAgent agent;
    
    
    // Gets rigibody, agent components
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        GameObject camaratarget = player.transform.GetChild(2).GetChild(0).gameObject;
        target = camaratarget.transform;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;
        transform.rotation = Quaternion.LookRotation(direction);

        Vector3 origin = transform.position;
        float distance = Vector3.Distance(origin, target.position);

        //For Debuging to see if LOS is even being checked
        Debug.DrawLine(origin, target.position, Color.red);

        Vector3 pos = transform.position;
        pos.y = height;
        transform.position = pos;

        //Determines if Player is LOS and sets bool
        if (Physics.Linecast(origin, target.position, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Player"))
            {
                LOS = true;
                following = true;
            }
            else
            {
                LOS = false;
                //Debug.Log(hit.transform.gameObject.name);
            }
        }

        if (!following)
        {
            return;
        }

        //If LOS, looks at player and doea attack if not on cooldown, otherwise navmesh pathing
        if (LOS)
        {
            agent.isStopped = true;
            agent.updatePosition = false;

            LookAtPlayer();

            if (!dashCooldown)
            {
                dashCooldown = true;
                StartCoroutine(DashAttack());
            }
        }
        else
        {
            NavAgentOn();

        }

    }

    //Makes it so it looks at player
    private void LookAtPlayer()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    //Turns on navmesh agent and kinematic property for navmesh pathing
    private void NavAgentOn()
    {
        //Debug.Log("Attempting movement");
        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.updateUpAxis = false;
            agent.updatePosition = true;
            agent.isStopped = false;
            agent.destination = target.position;
        }  
    }
    
    
    //Dash attack routine that sends the enemy forward
    IEnumerator DashAttack() 
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;

        float dashDuration = 0.5f;
        float timer = 0f;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + direction * dashForce;

        while (timer < dashDuration)
        {
            float t = timer / dashDuration;

            Vector3 pos = Vector3.Lerp(startPos, endPos, t);

            // optional: simple hover/bob instead of physics
            pos.y = height + Mathf.Sin(Time.time * bobSpeed) * bobAmount;

            // collision stop
            Vector3 moveDir = (pos - transform.position).normalized;
            float dist = Vector3.Distance(transform.position, pos);

            if (Physics.Raycast(transform.position, moveDir, dist, losMask))
            {
                break;
            }

            transform.position = pos;

            timer += Time.deltaTime;
            yield return null;
        }

        agent.Warp(transform.position);
        agent.updatePosition = true;
        agent.isStopped = false;

        yield return new WaitForSeconds(interval);
        dashCooldown = false;
    }

    //function that is called when enemies are spawned so they dont spawn more, disabling infinite spawning
    public void SetSpawn()
    {
        SubSpawn = false;
    }

    //Funciton to spawn enemies
    private void SpawnEnemies()
    {
        if (SubSpawn == true)
        {
            Debug.Log("Attempting spawn");
            GameObject subenemy1 = Instantiate(self, transform.position, Quaternion.identity);
            GameObject subenemy2 = Instantiate(self, transform.position, Quaternion.identity);
        
            subenemy2.SetActive(true);
            subenemy1.SetActive(true);  
            AiEnemyVirus current = subenemy1.GetComponent<AiEnemyVirus>();
            current.SetSpawn();
            current = subenemy2.GetComponent<AiEnemyVirus>();
            current.SetSpawn();
            Debug.Log("Spawned");
        }
        
    }

    //Inherited function that spawns enemies when death
    protected override void OnDeathEvent()
    {
        SpawnEnemies();
    }
}
