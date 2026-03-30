using UnityEngine;

public class BulleScript : MonoBehaviour
{
    [SerializeField] private int takeDamageNum = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
    
        if (other.CompareTag("Player"))
        {
            PlayerControllerV2 playerController = other.GetComponent<PlayerControllerV2>();
            if (playerController != null)
            {
                if (!playerController.GetBlocking())
                {
                    playerController.TakeDamage(takeDamageNum);
                }
            }
        }
        Destroy(gameObject);
    }
}
