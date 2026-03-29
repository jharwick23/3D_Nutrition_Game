using System.Collections;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;


public class AIEnemyBacteria : AIEnemy
{
    [SerializeField] private Transform target;
    [SerializeField] private float attackDistance, shootCooldown, upForce, height;
    [SerializeField] private GameObject aoeBullet;
    private NavMeshAgent agent;
    private bool LOS = false, following = false, coreroutineCalled = false;
    private float distance;
    private PlayerControllerV2 playerCont;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject camaratarget = player.transform.GetChild(2).GetChild(0).gameObject;
        target = camaratarget.transform;

        playerCont = FindAnyObjectByType<PlayerControllerV2>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 origin = transform.position;

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

        distance = Vector3.Distance(agent.transform.position, target.position);

        //Does shooting if in line of sight or moves, prevents bullet to parent collision
        if (LOS && distance < attackDistance) 
        {
            if (!coreroutineCalled)
            {
                coreroutineCalled = true;
                StartCoroutine(Shoot());
            }
        }
        else
        {
            agent.isStopped = false;
            agent.destination = target.position;
        }
        Vector3 pos = agent.nextPosition;
        pos.y = height;
        transform.position = pos;
        LookAtPlayer();

        //Checks if dead to prevent constant hit bug
        if (playerCont.IsDead())
        {
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            //Debug.Log("Bullet Destroying");
            foreach (GameObject bullet in bullets)
            {
                Destroy(bullet);
            }
        }
    }

    //Makes enemy look at player constantly after it has already had LOS once
    private void LookAtPlayer()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    //Shooting function for enemy, lobbing behavior
    IEnumerator Shoot()
    {
        Vector3 spawnPos = transform.position + transform.forward * 1.5f;
        spawnPos.y += 1f;
        GameObject bullet = Instantiate(aoeBullet, spawnPos, Quaternion.identity);
        Rigidbody rbb = bullet.GetComponent<Rigidbody>();
        Vector3 dir = (target.position - transform.position);
        dir.y = 0;
        dir.Normalize();

        Vector3 force = dir * distance + Vector3.up * upForce;

        rbb.AddForce(force, ForceMode.Impulse);

        yield return new WaitForSeconds(shootCooldown);
        coreroutineCalled = false;
    }

}
