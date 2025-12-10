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

    public void DropHeal(Vector3 position)
    {
        if (AppleHealPrefab != null)
        {
            Instantiate(AppleHealPrefab, position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("AppleHealPrefab not assigned!");
        }
    }

    public void DropCoin(Vector3 position)
    {
        if (MelonCoinPrefab != null)
        {
            Instantiate(MelonCoinPrefab, position, MelonCoinPrefab.transform.rotation);
        }
        else
        {
            Debug.LogWarning("MelonCoinPrefab not assigned!");
        }
    }
}
