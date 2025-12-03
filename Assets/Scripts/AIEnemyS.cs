using UnityEngine;
using UnityEngine.AI;
public class AIEnemyS : MonoBehaviour
{
    public Transform target;
    public GameObject sugarBullet;
    public Transform firePoint;
    public float attackDistance, fireRate, rotationSpeed;
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
        //Checks if enemy is in distance to shoot and shoots
        distance = Vector3.Distance(agent.transform.position, target.position);
        if (distance < attackDistance)
        {
            Vector3 dir = (target.position - transform.position).normalized;  
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotationSpeed * Time.deltaTime);
            if (fireRate <= Time.time)
            {
                Shoot();
                fireRate = Time.time + Random.Range(2f, 4f);
            }
        }
        //Used to make enemy float and bob a bit
        agent.destination = target.position;
        Vector3 pos = agent.nextPosition;
        pos.y = height + Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        transform.position = pos;
    }

    // Shooting function for subar ball
    void Shoot()
    {
        // Spawn slightly in front of enemy
        Vector3 spawnPos = transform.position + transform.forward * 1.5f;
        GameObject bullet = Instantiate(sugarBullet, spawnPos, Quaternion.identity);
        Vector3 direction = (target.position - spawnPos).normalized;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * 10f;
    }
}
