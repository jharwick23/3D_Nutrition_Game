using UnityEngine;

public class DamageTest : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControllerV2 playerController = other.GetComponent<PlayerControllerV2>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
            }
        }
    }
}
