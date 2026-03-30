using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToHub : MonoBehaviour
{
    

    private void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
