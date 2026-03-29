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
        StartCoroutine(Tick());
    }

    private void OnTriggerEnter(Collider other)
    {
        TryDealDamage(other.gameObject);
    }
    private void OnTriggerStay(Collider other)
    {
        TryDealDamage(other.gameObject);
    }
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
    IEnumerator Tick()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
