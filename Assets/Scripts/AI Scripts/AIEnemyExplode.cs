using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class AIEnemyExplode : MonoBehaviour
{
    public Transform target;
    public float attackDistance;
    private NavMeshAgent agent;
    private float distance;
    private bool attackInProg, playerInBox;
    private Collider playerCollider;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        attackInProg = false;
        playerInBox = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Determines if character is in attack distance to hit enemy
        distance = Vector3.Distance(agent.transform.position, target.position);
        if (distance < attackDistance || attackInProg)
        {
            agent.isStopped = true;
            attackInProg = true;
            StartCoroutine(Explosion());
        }
        else
        {
            agent.isStopped = false;
            agent.destination = target.position;
        }
    }

    //Handles Damage explosion if player is in designated Area
     private IEnumerator Explosion()
    {
        yield return new WaitForSeconds(1.0f);
        if (playerInBox)
        {
            PlayerControllerV2 playerController = playerCollider.GetComponent<PlayerControllerV2>();
            if (playerController != null)
            {
                if (!playerController.GetBlocking())
                {
                    playerController.TakeDamage(30);
                }
            }
        }
        Destroy(gameObject);
    }

    // Sets a flag true once player is on to do damage
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInBox = true;
            playerCollider = other;
        }
    }
    //Sets flag false once player in out to not do damage
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInBox = false;
        }
    }
}
