using UnityEngine;

public class UpgradeMenuNPC : MonoBehaviour
{
    public bool playerInRange = false;
    UpgradeStatsMenu upgradeStatsMenu;
    public UIHandler uiHandler;

    void Start()
    {
        upgradeStatsMenu = FindFirstObjectByType<UpgradeStatsMenu>();
        uiHandler = FindFirstObjectByType<UIHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            uiHandler.SetInteractPrompt(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            upgradeStatsMenu.DisableUpgradeMenu();
            uiHandler.SetInteractPrompt(false);
        }
    }

    public void OnInteract()
    {
        if (upgradeStatsMenu == null)
        {
            Debug.Log("Upgrade Menu not found!");
            return;
        }

        if (upgradeStatsMenu.isUpgradeMenuActive())
        {
            upgradeStatsMenu.DisableUpgradeMenu();
            uiHandler.SetInteractPrompt(true);
        }
        else
        {
            upgradeStatsMenu.EnableUpgradeMenu();
            uiHandler.SetInteractPrompt(false);
        }
    }
}
