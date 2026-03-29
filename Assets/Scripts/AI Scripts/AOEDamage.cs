using System.Collections;
using UnityEngine;

public class AOEDamage : MonoBehaviour
{

    [SerializeField] private int damage = 10;
    [SerializeField] private float damageInterval = 1f, timer = 10f;

    private float lastDamageTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Routine to killitself afterwards
        StartCoroutine(Tick());
    }

    //Call function for damdage if it enters trigger
    private void OnTriggerEnter(Collider other)
    {
        TryDealDamage(other.gameObject);
    }

    //Calls same damage function if collider is still inside
    private void OnTriggerStay(Collider other)
    {
        TryDealDamage(other.gameObject);
    }

    //Does damage over a second interval that can be modified
    void TryDealDamage(GameObject other)
    {
        if (!other.CompareTag("Player")) return;

        if (Time.time >= lastDamageTime + damageInterval)
        {
            PlayerControllerV2 player = other.GetComponent<PlayerControllerV2>();
            if (player != null)
            {
                player.TakeDamage(damage);
                lastDamageTime = Time.time;
            }
        }
    }

    //Tick that after a timer goes off, it destroys itself
    IEnumerator Tick()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
