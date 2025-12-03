using UnityEngine;
using UnityEngine.UI;

public class HealthUIHandler : MonoBehaviour
{
    private Slider _healthSlider;

    void Awake()
    {
        _healthSlider = GetComponent<Slider>();
        if (_healthSlider == null)
        {
            Debug.LogError("Health slider not found as a component.");
        }
    }
    
    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = currentHealth;
    }
}
