using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIEnemyC : MonoBehaviour
{
    public Transform target;
    public float attackDistance;
    private NavMeshAgent agent;
    private float distance;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Determines if character is in attack distance to hit enemy
        distance = Vector3.Distance(agent.transform.position, target.position);
        if (distance < attackDistance)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
            agent.destination = target.position;
        }
    }

}