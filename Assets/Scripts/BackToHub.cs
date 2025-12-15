using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToHub : MonoBehaviour
{
   public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
