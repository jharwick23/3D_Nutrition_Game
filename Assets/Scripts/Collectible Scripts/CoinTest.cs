using UnityEngine;

public class CoinTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("Player entered coin trigger but is missing PlayerControllerV2 component!");
        print("Coin Triggered");
        if (other.CompareTag("Player"))
        {
            PlayerControllerV2 playerController = other.GetComponent<PlayerControllerV2>();
            if (playerController != null)
            {
                playerController.AddCoins(1);
            }
        }
    }
}
