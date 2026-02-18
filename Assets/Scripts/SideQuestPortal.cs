using UnityEngine;
using UnityEngine.SceneManagement;

public class SideQuestPortal : MonoBehaviour
{
    private PortalGate_Controller _portalController;

    private void Start()
    {
        _portalController = GetComponent<PortalGate_Controller>();
        _portalController.F_TogglePortalGate(true);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("SideQuestOne");
        }
    }
}
