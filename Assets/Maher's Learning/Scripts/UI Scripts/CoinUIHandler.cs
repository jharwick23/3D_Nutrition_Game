using UnityEngine;
using TMPro;

public class CoinUIHandler : MonoBehaviour
{
    private TextMeshProUGUI _coinText;

    void Awake()
    {
        _coinText = GetComponent<TextMeshProUGUI>();
        if (_coinText == null)
        {
            Debug.LogError("Coin TextMeshProUGUI component not found.");
        }
    }

    public void UpdateCoinUI(int currentCoins)
    {
        _coinText.text = currentCoins.ToString();
    }
}
