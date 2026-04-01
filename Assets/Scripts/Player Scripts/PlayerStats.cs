using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerControllerV2 PlayerController;
    private UIHandler _uiHandler;
    private int _maxHeathStat;
    private int _movementSpeedStat;
    private int _reloadSpeedStat;
    private int _healingAmountStat;
    private int _blockStrengthStat;

    private void Start()
    {
        if (PlayerController == null)
        {
            PlayerController = FindFirstObjectByType<PlayerControllerV2>();
        }
        _uiHandler ??= FindFirstObjectByType<UIHandler>();
        LoadStats();
    }

    public void SaveStats()
    {
        PlayerPrefs.SetInt("MaxHealthStat", _maxHeathStat);
        PlayerPrefs.SetInt("MovementSpeedStat", _movementSpeedStat);
        PlayerPrefs.SetInt("ReloadSpeedStat", _reloadSpeedStat);
        PlayerPrefs.SetInt("HealingAmountStat", _healingAmountStat);
        PlayerPrefs.SetInt("BlockStrengthStat", _blockStrengthStat);
        PlayerPrefs.Save();
    }

    private void LoadStats()
    {
        _maxHeathStat = PlayerPrefs.GetInt("MaxHealthStat", 0);
        _movementSpeedStat = PlayerPrefs.GetInt("MovementSpeedStat", 0);
        _reloadSpeedStat = PlayerPrefs.GetInt("ReloadSpeedStat", 0);
        _healingAmountStat = PlayerPrefs.GetInt("HealingAmountStat", 0);
        _blockStrengthStat = PlayerPrefs.GetInt("BlockStrengthStat", 0);
    }

    public void UpgradeMaxHealth()
    {
        if (PlayerPrefs.GetInt("Coins", 0) < 50)
        {
            Debug.Log("Not enough coins to upgrade Max Health!");
            _uiHandler.SetAlertPrompt("Not enough coins to upgrade Max Health!", 2f, Color.red);
            return;
        }
        
        _maxHeathStat++;
        SaveStats();
        PlayerController.AddCoins(-50);
        PlayerController.InitializePlayerData();
        _uiHandler.SetAlertPrompt("Max Health Upgraded!", 2f, Color.green);
    }

    public void UpgradeMovementSpeed()
    {
        if (PlayerPrefs.GetInt("Coins", 0) < 50)
        {
            Debug.Log("Not enough coins to upgrade Movement Speed!");
            _uiHandler.SetAlertPrompt("Not enough coins to upgrade Movement Speed!", 2f, Color.red);
            return;
        }
        
        _movementSpeedStat++;
        SaveStats();
        PlayerController.AddCoins(-50);
        PlayerController.InitializePlayerData();
        _uiHandler.SetAlertPrompt("Movement Speed Upgraded!", 2f, Color.green);
    }

    public void UpgradeReloadSpeed()
    {
        if (PlayerPrefs.GetInt("Coins", 0) < 50)
        {
            Debug.Log("Not enough coins to upgrade Reload Speed!");
            _uiHandler.SetAlertPrompt("Not enough coins to upgrade Reload Speed!", 2f, Color.red);
            return;
        }
        
        _reloadSpeedStat++;
        SaveStats();
        PlayerController.AddCoins(-50);
        PlayerController.InitializePlayerData();
        _uiHandler.SetAlertPrompt("Reload Speed Upgraded!", 2f, Color.green);
    }

    public void UpgradeHealingAmount()
    {
        if (PlayerPrefs.GetInt("Coins", 0) < 50)
        {
            Debug.Log("Not enough coins to upgrade Healing Amount!");
            _uiHandler.SetAlertPrompt("Not enough coins to upgrade Healing Amount!", 2f, Color.red);
            return;
        }
        
        _healingAmountStat++;
        SaveStats();
        PlayerController.AddCoins(-50);
        PlayerController.InitializePlayerData();
        _uiHandler.SetAlertPrompt("Healing Amount Upgraded!", 2f, Color.green);
    }

    public void UpgradeBlockStrength()
    {
        if (PlayerPrefs.GetInt("Coins", 0) < 50)
        {
            Debug.Log("Not enough coins to upgrade Block Strength!");
            _uiHandler.SetAlertPrompt("Not enough coins to upgrade Block Strength!", 2f, Color.red);
            return;
        }
        
        _blockStrengthStat++;
        SaveStats();
        PlayerController.AddCoins(-50);
        PlayerController.InitializePlayerData();
        _uiHandler.SetAlertPrompt("Block Strength Upgraded!", 2f, Color.green);
    }
   
   public void WipeStats()
    {
        PlayerPrefs.DeleteKey("MaxHealthStat");
        PlayerPrefs.DeleteKey("MovementSpeedStat");
        PlayerPrefs.DeleteKey("ReloadSpeedStat");
        PlayerPrefs.DeleteKey("HealingAmountStat");
        PlayerPrefs.DeleteKey("BlockStrengthStat");
        LoadStats();
        PlayerController.InitializePlayerData();
        _uiHandler.SetAlertPrompt("Stats Wiped!", 2f, Color.green);
    }
}
