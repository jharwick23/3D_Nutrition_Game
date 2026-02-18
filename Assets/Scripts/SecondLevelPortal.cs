using UnityEngine;
using UnityEngine.SceneManagement;

public class SecondLevelPortal : MonoBehaviour
{
    private PortalGate_Controller _portalController;

    private void Start()
    {
        _portalController = GetComponent<PortalGate_Controller>();
        if (PlayerPrefs.GetInt("FirstLevelCompleted", 0) == 1)
        {
            _portalController.F_TogglePortalGate(true);
        }
        else
        {
            _portalController.F_TogglePortalGate(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PlayerPrefs.GetInt("FirstLevelCompleted", 0) == 1)
        {
            SceneManager.LoadScene("SecondLevel");
        }
    }
}
