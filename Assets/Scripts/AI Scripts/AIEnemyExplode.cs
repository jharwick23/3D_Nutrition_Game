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
    private bool attackInProg, playerInBox, routineCalled = false;
    private Collider playerCollider;
    public AudioClip explodeAudio;
    public GameObject explosionVFX;


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
            if (!routineCalled)
            {
                StartCoroutine(Explosion());
                routineCalled = true;
            }
            
        }
        else
        {
            agent.isStopped = false;
            agent.destination = target.position;
        }
    }

    private void PlayExplosionSound()
    {
        if (explodeAudio)
        {
            AudioSource.PlayClipAtPoint(explodeAudio, transform.position, 5f);
        }
    }

    //Handles Damage explosion if player is in designated Area
    private IEnumerator Explosion()
    {
        yield return new WaitForSeconds(0.5f);
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
        routineCalled = false;
        PlayExplosionSound();
        if(explosionVFX != null)
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
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
