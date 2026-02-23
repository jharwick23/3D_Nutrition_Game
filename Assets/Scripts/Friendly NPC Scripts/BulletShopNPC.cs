using UnityEngine;

public class BulletShopNPC : MonoBehaviour
{
    public bool playerInRange = false;
    public BuyBulletsMenu buyBulletsMenu;
    public UIHandler uiHandler;

    void Start()
    {
        buyBulletsMenu = FindFirstObjectByType<BuyBulletsMenu>();
        uiHandler = FindFirstObjectByType<UIHandler>();
        if (uiHandler == null)
        {
            Debug.Log("UIHandler not found!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (uiHandler != null)
            uiHandler.SetInteractPrompt(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            buyBulletsMenu.DisableUpgradeMenu();
            uiHandler.SetInteractPrompt(false);
        }
    }

    public void OnInteract()
    {
        if (buyBulletsMenu == null)
        {
            Debug.Log("Buy Bullets Menu not found!");
            return;
        }

        if (buyBulletsMenu.isUpgradeMenuActive())
        {
            buyBulletsMenu.DisableUpgradeMenu();
            uiHandler.SetInteractPrompt(true);
        }
        else
        {
            buyBulletsMenu.EnableUpgradeMenu();
            uiHandler.SetInteractPrompt(false);
        }
    }
}
