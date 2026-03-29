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
    private Rigidbody rb;
    
    
    // Gets rigibody, agent components
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
            }
        }

        if (!following)
        {
            return;
        }

        //If LOS, looks at player and doea attack if not on cooldown, otherwise navmesh pathing
        if (LOS)
        {
            rb.isKinematic = false;
            agent.enabled = false;
            Vector3 pos = transform.position;
            pos.y = height;
            transform.position = pos;
            LookAtPlayer();
            if (!dashCooldown)
            {
                dashCooldown = true;
                StartCoroutine(DashAttack());
            }
            
        }
        else
        {
            Vector3 pos = transform.position;
            pos.y = height;
            transform.position = pos;
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
        if (!rb.isKinematic)
        {
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }
        rb.isKinematic = true;
        agent.enabled = true;
        
        if (agent.enabled)
        { 
            agent.updateUpAxis = false;
            agent.isStopped = false;
           
        }
        agent.destination = target.position;
        
    }
    
    IEnumerator DashAttack() 
    {
        

        //Does force application
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 Force = direction * dashForce;

        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Force, ForceMode.Impulse);


        yield return new WaitForSeconds(interval);
        dashCooldown = false;
    }
    public void SetSpawn()
    {
        SubSpawn = false;
    }

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

    protected override void OnDeathEvent()
    {
        SpawnEnemies();
    }
}
