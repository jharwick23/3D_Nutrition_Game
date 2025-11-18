using UnityEngine;
using TMPro;

public class AmmoUIHandler : MonoBehaviour
{
    private TextMeshProUGUI _ammoText;

    void Awake()
    {
        _ammoText = GetComponent<TextMeshProUGUI>();
        if (_ammoText == null)
        {
            Debug.LogError("Ammo TextMeshProUGUI component not found.");
        }
    }

    public void UpdateAmmoUI(string currentAmmoText)
    {
        _ammoText.text = currentAmmoText;
    }
}
