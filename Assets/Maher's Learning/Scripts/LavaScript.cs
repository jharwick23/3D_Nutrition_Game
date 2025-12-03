using UnityEngine;

public class LavaScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControllerV2 playerController = other.GetComponent<PlayerControllerV2>();
            if (playerController != null)
            {
                playerController.TakeDamage(100);
            }
            else
            {
                Debug.LogWarning("PlayerControllerV2 component not found on the player.");
            }
        }
    }
}
