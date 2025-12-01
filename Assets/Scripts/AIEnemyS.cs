using UnityEngine;
using UnityEngine.AI;
public class AIEnemyS : MonoBehaviour
{
    public Transform target;
    public float attackDistance;
    private NavMeshAgent agent;
    private float distance;
    public float height = 2f, bobSpeed = 2f, bobAmount = 0.3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateUpAxis = false;
        agent.isStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if enemy is in distance to shoot
        distance = Vector3.Distance(agent.transform.position, target.position);
        if (distance < attackDistance)
        {
            
        }
        //Used to make enemy float and bob a bit
        agent.destination = target.position;
        Vector3 pos = agent.nextPosition;
        pos.y = height + Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        transform.position = pos;
    }
}
