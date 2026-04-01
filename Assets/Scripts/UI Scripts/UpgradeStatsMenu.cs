using UnityEngine;
using TMPro;

public class UpgradeStatsMenu : MonoBehaviour
{
    public PlayerStats playerStats;
    private UIHandler _uiHandler;

    [SerializeField] private GameObject upgradePanel;

    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI movementSpeedText;
    public TextMeshProUGUI reloadSpeedText;
    public TextMeshProUGUI healingAmountText;
    public TextMeshProUGUI blockStrengthText;
    private int maxHealthCap = 10;
    private int movementSpeedCap = 10;
    private int reloadSpeedCap = 10;
    private int healingAmountCap = 10;
    private int blockStrengthCap = 10;

    private void Start()
    {
        _uiHandler = FindFirstObjectByType<UIHandler>();
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
            inputHandler.DisableInputsForVendors();
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

    public bool isUpgradeMenuActive()
    {
        return upgradePanel.activeSelf;
    }

    public void UpdateUI()
    {
        maxHealthText.text = "Lvl. " + PlayerPrefs.GetInt("MaxHealthStat", 0);
        movementSpeedText.text = "Lvl. " + PlayerPrefs.GetInt("MovementSpeedStat", 0);
        reloadSpeedText.text = "Lvl. " + PlayerPrefs.GetInt("ReloadSpeedStat", 0);
        healingAmountText.text = "Lvl. " + PlayerPrefs.GetInt("HealingAmountStat", 0);
        blockStrengthText.text = "Lvl. " + PlayerPrefs.GetInt("BlockStrengthStat", 0);
    }

    public void OnUpgradeMaxHealthButton()
    {
        if (PlayerPrefs.GetInt("MaxHealthStat", 0) >= maxHealthCap)
        {
            _uiHandler.SetAlertPrompt("Max Health is already at max level " + maxHealthCap + "!", 2f, Color.red);
            return;
        }
        playerStats.UpgradeMaxHealth();
        UpdateUI();
    }

    public void OnUpgradeHealingAmountButton()
    {
        if (PlayerPrefs.GetInt("HealingAmountStat", 0) >= healingAmountCap)
        {
            _uiHandler.SetAlertPrompt("Healing Amount is already at max level " + healingAmountCap + "!", 2f, Color.red);
            return;
        }
        playerStats.UpgradeHealingAmount();
        UpdateUI();
    }

    public void OnUpgradeReloadSpeedButton()
    {
        if (PlayerPrefs.GetInt("ReloadSpeedStat", 0) >= reloadSpeedCap)
        {
            _uiHandler.SetAlertPrompt("Reload Speed is already at max level " + reloadSpeedCap + "!", 2f, Color.red);
            return;
        }
        playerStats.UpgradeReloadSpeed();
        UpdateUI();
    }

    public void OnUpgradeMovementSpeedButton()
    {
        if (PlayerPrefs.GetInt("MovementSpeedStat", 0) >= movementSpeedCap)
        {
            _uiHandler.SetAlertPrompt("Movement Speed is already at max level " + movementSpeedCap + "!", 2f, Color.red);
            return;
        }
        playerStats.UpgradeMovementSpeed();
        UpdateUI();
    }

    public void OnUpgradeBlockStrengthButton()
    {
        if (PlayerPrefs.GetInt("BlockStrengthStat", 0) >= blockStrengthCap)
        {
            _uiHandler.SetAlertPrompt("Block Strength is already at max level " + blockStrengthCap + "!", 2f, Color.red);
            return;
        }
        playerStats.UpgradeBlockStrength();
        UpdateUI();
    }
    public void OnWipeStatsButton()
    {
        playerStats.WipeStats();
        UpdateUI();
    }
}
