using UnityEngine;

public class UpgradeMenuNPC : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UpgradeStatsMenu upgradeStatsMenu = FindFirstObjectByType<UpgradeStatsMenu>();
            if (upgradeStatsMenu)
            {
                upgradeStatsMenu.EnableUpgradeMenu();
            }
            else
            {
                Debug.Log("Upgrade Stats Menu not found!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UpgradeStatsMenu upgradeStatsMenu = FindFirstObjectByType<UpgradeStatsMenu>();
            if (upgradeStatsMenu)
            {
                upgradeStatsMenu.DisableUpgradeMenu();
            }
            else
            {
                Debug.Log("Upgrade Stats Menu not found!");
            }
        }
    }
    
}
