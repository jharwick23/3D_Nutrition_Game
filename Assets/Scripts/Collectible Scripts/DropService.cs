using UnityEngine;

public class DropService : MonoBehaviour
{
    public GameObject AppleHealPrefab;
    public GameObject MelonCoinPrefab;

    private void Start()
    {
        if (!AppleHealPrefab|| !MelonCoinPrefab)
        {
            Debug.LogWarning("Heal & Coin Prefabs not Set!");
        }
    }

    public void DropHeal(Vector3 position, int healAmount = 10)
    {
        if (AppleHealPrefab != null)
        {
            GameObject appleHeal = Instantiate(AppleHealPrefab, position, Quaternion.identity);
            appleHeal.GetComponent<CollectHeal>().baseHealAmount = healAmount;
        }
        else
        {
            Debug.LogWarning("AppleHealPrefab not assigned!");
        }
    }

    public void DropCoin(Vector3 position, int amount = 1)
    {
        if (MelonCoinPrefab != null)
        {
            GameObject melonCoin = Instantiate(MelonCoinPrefab, position, MelonCoinPrefab.transform.rotation);
            melonCoin.GetComponent<CollectCoin>().coinAmount = amount;
        }
        else
        {
            Debug.LogWarning("MelonCoinPrefab not assigned!");
        }
    }
}
