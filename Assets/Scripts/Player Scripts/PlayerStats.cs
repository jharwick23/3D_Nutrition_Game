using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerControllerV2 PlayerController;
    private int _maxHeathStat;
    private int _movementSpeedStat;
    private int reloadSpeedStat;
    private int healingAmountStat;

    private void Awake()
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
        PlayerPrefs.SetInt("ReloadSpeedStat", reloadSpeedStat);
        PlayerPrefs.SetInt("HealingAmountStat", healingAmountStat);
    }

    private void LoadStats()
    {
        _maxHeathStat = PlayerPrefs.GetInt("MaxHealthStat", 0);
        _movementSpeedStat = PlayerPrefs.GetInt("MovementSpeedStat", 0);
        reloadSpeedStat = PlayerPrefs.GetInt("ReloadSpeedStat", 0);
        healingAmountStat = PlayerPrefs.GetInt("HealingAmountStat", 0);
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
        
        reloadSpeedStat++;
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
        
        healingAmountStat++;
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
        LoadStats();
        PlayerController.InitializePlayerData();
    }
}
