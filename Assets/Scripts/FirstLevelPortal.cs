using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstLevelPortal : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("FirstLevel");
        }
    }
}
