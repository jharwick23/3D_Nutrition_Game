using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerControllerV2 PlayerController;
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
            return;
        }
        
        _maxHeathStat++;
        SaveStats();
        PlayerController.AddCoins(-50);
        PlayerController.InitializePlayerData();
    }

    public void UpgradeMovementSpeed()
    {
        if (PlayerPrefs.GetInt("Coins", 0) < 50)
        {
            Debug.Log("Not enough coins to upgrade Movement Speed!");
            return;
        }
        
        _movementSpeedStat++;
        SaveStats();
        PlayerController.AddCoins(-50);
        PlayerController.InitializePlayerData();
    }

    public void UpgradeReloadSpeed()
    {
        if (PlayerPrefs.GetInt("Coins", 0) < 50)
        {
            Debug.Log("Not enough coins to upgrade Reload Speed!");
            return;
        }
        
        _reloadSpeedStat++;
        SaveStats();
        PlayerController.AddCoins(-50);
        PlayerController.InitializePlayerData();
    }

    public void UpgradeHealingAmount()
    {
        if (PlayerPrefs.GetInt("Coins", 0) < 50)
        {
            Debug.Log("Not enough coins to upgrade Healing Amount!");
            return;
        }
        
        _healingAmountStat++;
        SaveStats();
        PlayerController.AddCoins(-50);
        PlayerController.InitializePlayerData();
    }

    public void UpgradeBlockStrength()
    {
        if (PlayerPrefs.GetInt("Coins", 0) < 50)
        {
            Debug.Log("Not enough coins to upgrade Block Strength!");
            return;
        }
        
        _blockStrengthStat++;
        SaveStats();
        PlayerController.AddCoins(-50);
        PlayerController.InitializePlayerData();
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
    }
}
