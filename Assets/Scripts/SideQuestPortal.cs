using UnityEngine;
using UnityEngine.SceneManagement;

public class SideQuestPortal : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("SideQuestOne");
        }
    }
}
