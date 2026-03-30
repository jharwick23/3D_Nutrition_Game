using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class AIEnemyBurger : MonoBehaviour
{
    public Transform target;
    public float attackDistance;
    private NavMeshAgent agent;
    private float distance;
    public LayerMask obstructionMask;
    private bool LOS = false, following = false, routineCalled = false;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float forwardForce, upForce, randomForceModifier;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 origin = transform.position;
        float distance = Vector3.Distance(origin, target.position);

        //For Debuging to see if LOS is even being checked
        Debug.DrawLine(origin, target.position, Color.red);

        LOS = false;

        //Determines if Player is LOS and sets bool
        if (Physics.Linecast(origin, target.position, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Player"))
            {
                LOS = true;
                following = true;
            }
        }

        if (!following)
        {
            return;
        }


        //Attack pattern
        if (distance < attackDistance && LOS)
        {
            if(agent.enabled)
            {
                agent.isStopped = true;
            }
            

            //Fixes looking issue, now looks at target after stop
            Vector3 lookDirection = (target.position - transform.position).normalized;
            lookDirection.y = 0; 
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            
            if (!routineCalled && rb.isKinematic)
            {
                StartCoroutine(Attack());
                routineCalled = true;
            }
            
            
        }
        else
        {
            if (agent.enabled)
            {
                agent.isStopped = false;
                agent.destination = target.position;
            }
            
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1.0f);
        
        rb.isKinematic = false;
        agent.enabled = false;

        Vector3 dir = (target.position - transform.position);
        dir.y = 0;
        dir.Normalize();

        Vector3 force = dir * forwardForce + Vector3.up * upForce + Random.insideUnitSphere * randomForceModifier;

        rb.AddForce(force, ForceMode.Impulse);

        routineCalled = false;
        
        StartCoroutine(RecoverTime());
    }

    IEnumerator RecoverTime()
    {
        yield return new WaitForSeconds(3.0f);
        rb.isKinematic = true;
        agent.enabled = true;
    }
}
