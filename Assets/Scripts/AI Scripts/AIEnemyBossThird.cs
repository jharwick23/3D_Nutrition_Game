using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemyBossThird : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private float attackDistance, distance;
    [SerializeField] private Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(agent.transform.position, target.position);
        if (distance < attackDistance)
        {
            agent.destination = target.position;
        }


    }
}
