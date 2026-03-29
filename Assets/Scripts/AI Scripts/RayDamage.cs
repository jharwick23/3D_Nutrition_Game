using UnityEngine;

public class RayDamage : MonoBehaviour
{
    [SerializeField] private int damage = 30;
    [SerializeField] private float damageInterval = 1f;

    private float lastDamageTime = 0f;

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

   
}
