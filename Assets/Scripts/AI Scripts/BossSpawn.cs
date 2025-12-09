using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyTypeA;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyTypeA.SetActive(true);
        }
    }
}
