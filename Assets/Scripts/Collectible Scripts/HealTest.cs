using UnityEngine;

public class HealTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControllerV2 playerController = other.GetComponent<PlayerControllerV2>();
            if (playerController != null)
            {
                playerController.Heal(10);
            }
        }
    }
}
