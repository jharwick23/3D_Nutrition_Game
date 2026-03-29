using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float damageInterval = 1f;

    private float lastDamageTime = 0f;


    //Calls particle damage if object enters
    void OnParticleCollision(GameObject other)
    {
        TryDealDamage(other);
    }

    //Calls damage if object is still inside object
    private void OnTriggerStay(Collider other)
    {
        TryDealDamage(other.gameObject);
    }

    //Does damage over a second of time 
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
}

