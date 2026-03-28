using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float damageInterval = 1f;

    private float lastDamageTime = 0f;

    void OnParticleCollision(GameObject other)
    {
        TryDealDamage(other);
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
}

