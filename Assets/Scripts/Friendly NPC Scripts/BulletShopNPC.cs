using UnityEngine;

public class BulletShopNPC : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BuyBulletsMenu buyBulletsMenu = FindFirstObjectByType<BuyBulletsMenu>();
            if (buyBulletsMenu)
            {
                buyBulletsMenu.EnableUpgradeMenu();
            }
            else
            {
                Debug.Log("Buy Bullets Menu not found!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BuyBulletsMenu buyBulletsMenu = FindFirstObjectByType<BuyBulletsMenu>();
            if (buyBulletsMenu)
            {
                buyBulletsMenu.DisableUpgradeMenu();
            }
            else
            {
                Debug.Log("Buy Bullets Menu not found!");
            }
        }
    }
}
