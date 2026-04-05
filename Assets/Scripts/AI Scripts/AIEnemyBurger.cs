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
    //private float distance;
    public LayerMask obstructionMask;
    private bool LOS = false, following = false, routineCalled = false;
    [SerializeField] private float forwardForce;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();

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

        
            //Determiens routines by rigidbody and singleton routine handling
            if (!routineCalled)
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

    //Handles Burger Attack Timer and agent and rigibody manipulation
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1.0f);

        agent.isStopped = true;
        agent.updatePosition = false;

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;

        float attackDuration = 0.3f;
        float timer = 0f;
        float jumpHeight = 0.5f;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + direction * forwardForce * attackDuration;

        while (timer < attackDuration)
        {
            float t = timer / attackDuration;

            // horizontal movement
            Vector3 pos = Vector3.Lerp(startPos, endPos, t);

            // vertical arc
            float yOffset;

            if (t < 0.5f)
            {
                // going up 
                yOffset = Mathf.Lerp(0, jumpHeight, t * 2f);
            }
            else
            {
                // falling 
                yOffset = Mathf.Lerp(jumpHeight, 0, (t - 0.5f) * 2f);
                yOffset *= 0.6f;
            }

            pos.y += yOffset;

            // collision check before moving
            Vector3 moveDir = (pos - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, pos);

            if (Physics.Raycast(transform.position, moveDir, distance))
            {
                // drop straight down instead of freezing mid-air
                RaycastHit groundHit;
                if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 10f))
                {
                    float heightOffset = GetComponent<Collider>().bounds.extents.y;
                    transform.position = groundHit.point + Vector3.up * heightOffset;
                }

                break;
            }

            transform.position = pos;

            timer += Time.deltaTime;
            yield return null;
        }


        //routineCalled = false;

        StartCoroutine(RecoverTime());
    }

    //handles recovery time and rigidbody and agent manipulation
    IEnumerator RecoverTime()
    {
        yield return new WaitForSeconds(3.5f);
        routineCalled = false;

        agent.Warp(transform.position);
        agent.updatePosition = true;
        agent.isStopped = false;
    }
}
