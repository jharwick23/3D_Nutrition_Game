using UnityEngine;

public class UIHandler : MonoBehaviour
{
    // UI Handlers
    private HealthUIHandler _healthUIHandler;
    private CoinUIHandler _coinUIHandler;
    private AmmoUIHandler _ammoUIHandler;

    void Awake()
    {
        _healthUIHandler = GetComponentInChildren<HealthUIHandler>();
        _coinUIHandler = GetComponentInChildren<CoinUIHandler>();
        _ammoUIHandler = GetComponentInChildren<AmmoUIHandler>();
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (_healthUIHandler != null)
        {
            _healthUIHandler.UpdateHealthUI(currentHealth, maxHealth);
        }
        else
        {
            Debug.LogWarning("healthUIHandler component not found.");
        }
    }

    public void UpdateCoinUI(int currentCoins)
    {
        if (_coinUIHandler != null)
        {
            _coinUIHandler.UpdateCoinUI(currentCoins);
        }
        else
        {
            Debug.LogWarning("CoinUIHandler component not found.");
        }
    }

    public void UpdateAmmoUI(string currentAmmoText)
    {
        if (_ammoUIHandler != null)
        {
            _ammoUIHandler.UpdateAmmoUI(currentAmmoText);
        }
        else
        {
            Debug.LogWarning("AmmoUIHandler component not found.");
        }
    }
}
