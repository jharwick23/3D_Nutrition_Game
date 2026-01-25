using UnityEngine;
using TMPro;

public class UpgradeStatsMenu : MonoBehaviour
{
    public PlayerStats playerStats;

    [SerializeField] private GameObject upgradePanel;

    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI movementSpeedText;
    public TextMeshProUGUI reloadSpeedText;
    public TextMeshProUGUI healingAmountText;

    private void Start()
    {
        if (playerStats == null)
        {
            playerStats = FindFirstObjectByType<PlayerStats>();
        }
        UpdateUI();
    }

    public void EnableUpgradeMenu()
    {
        upgradePanel.SetActive(true);

        // Disable Crosshair
        UIHandler _uiHandler;
        _uiHandler = FindFirstObjectByType<UIHandler>();
        if (_uiHandler)
        {
            _uiHandler.ToggleCrosshair(false);
        }
        else
        {
            Debug.Log("UI Handler was not set!");
        }

        // Disable Inputs
        InputHandlerV2 inputHandler = FindFirstObjectByType<InputHandlerV2>();
        if (inputHandler)
        {
            inputHandler.DisableInputs();
        }
        else
        {
            Debug.Log("Inputhandler not found!");
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DisableUpgradeMenu()
    {
        upgradePanel.SetActive(false);

        // Enable Crosshair
        UIHandler _uiHandler;
        _uiHandler = FindFirstObjectByType<UIHandler>();
        if (_uiHandler)
        {
            _uiHandler.ToggleCrosshair(true);
        }
        else
        {
            Debug.Log("UI Handler was not set!");
        }

        // Enable Inputs
        InputHandlerV2 inputHandler = FindFirstObjectByType<InputHandlerV2>();
        if (inputHandler)
        {
            inputHandler.EnableInputs();
        }
        else
        {
            Debug.Log("Inputhandler not found!");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void UpdateUI()
    {
        maxHealthText.text = "Lvl. " + PlayerPrefs.GetInt("MaxHealthStat", 0);
        movementSpeedText.text = "Lvl. " + PlayerPrefs.GetInt("MovementSpeedStat", 0);
        reloadSpeedText.text = "Lvl. " + PlayerPrefs.GetInt("ReloadSpeedStat", 0);
        healingAmountText.text = "Lvl. " + PlayerPrefs.GetInt("HealingAmountStat", 0);
    }

    public void OnUpgradeMaxHealthButton()
    {
        playerStats.UpgradeMaxHealth();
        UpdateUI();
    }

    public void OnUpgradeHealingAmountButton()
    {
        playerStats.UpgradeHealingAmount();
        UpdateUI();
    }

    public void OnUpgradeReloadSpeedButton()
    {
        playerStats.UpgradeReloadSpeed();
        UpdateUI();
    }

    public void OnUpgradeMovementSpeedButton()
    {
        playerStats.UpgradeMovementSpeed();
        UpdateUI();
    }

    public void OnWipeStatsButon()
    {
        playerStats.WipeStats();
        UpdateUI();
    }
}
