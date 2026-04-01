using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UIHandler : MonoBehaviour
{
    // UI Handlers
    private HealthUIHandler _healthUIHandler;
    private CoinUIHandler _coinUIHandler;
    private AmmoUIHandler _ammoUIHandler;
    private BulletTypeUIHandler _bulletTypeUIHandler;
    private Image _crosshairImage;
    private GameObject _interactPromptObject; 
    private GameObject _alertPromptObject;
    private Coroutine _alertCoroutine; 

    void Awake()
    {
        _healthUIHandler = GetComponentInChildren<HealthUIHandler>();
        _coinUIHandler = GetComponentInChildren<CoinUIHandler>();
        _ammoUIHandler = GetComponentInChildren<AmmoUIHandler>();
        _bulletTypeUIHandler = GetComponentInChildren<BulletTypeUIHandler>();
        _crosshairImage = transform.Find("Crosshair").GetComponent<Image>();
        _interactPromptObject = transform.Find("InteractPrompt").gameObject;
        _alertPromptObject = transform.Find("AlertPrompt").gameObject;
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

    public void SetInteractPrompt(bool show)
    {
        if (_interactPromptObject != null)
        {
            _interactPromptObject.SetActive(show);
        }
        else
        {
            Debug.LogWarning("InteractPromptText component not found.");
        }
    }

    public void SetAlertPrompt(string message = "", float duration = 2f, Color? color = null)
    {
        if (_alertPromptObject == null) return;
        
        TextMeshProUGUI alertText = _alertPromptObject.GetComponent<TextMeshProUGUI>();
        if (alertText == null) return;

        if (_alertCoroutine != null)
        {
            StopCoroutine(_alertCoroutine);
        }

        alertText.text = message;
        alertText.color = color ?? Color.red;
        _alertPromptObject.SetActive(true);
        _alertCoroutine = StartCoroutine(AlertTimer(duration));
    }
    
    private IEnumerator AlertTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        _alertPromptObject.SetActive(false);
    }
}
