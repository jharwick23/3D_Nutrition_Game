using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    // UI Handlers
    private HealthUIHandler _healthUIHandler;
    private CoinUIHandler _coinUIHandler;
    private AmmoUIHandler _ammoUIHandler;
    private BulletTypeUIHandler _bulletTypeUIHandler;
    private Image _crosshairImage;

    void Awake()
    {
        _healthUIHandler = GetComponentInChildren<HealthUIHandler>();
        _coinUIHandler = GetComponentInChildren<CoinUIHandler>();
        _ammoUIHandler = GetComponentInChildren<AmmoUIHandler>();
        _bulletTypeUIHandler = GetComponentInChildren<BulletTypeUIHandler>();
        _crosshairImage = transform.Find("Crosshair").GetComponent<Image>();
    }

    public void ToggleCrosshair(bool value)
    {
        _crosshairImage.enabled = value;
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

    public void UpdateBulletTypeUI(string bulletType)
    {
        if (_bulletTypeUIHandler != null)
        {
            _bulletTypeUIHandler.UpdateBulletType(bulletType);
        }
        else
        {
            Debug.LogWarning("BulletTypeUIHandler component not found.");
        }
    }
}
