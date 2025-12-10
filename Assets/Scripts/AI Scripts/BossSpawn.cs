using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyTypeA;
    [SerializeField] private Transform transform;
    private GameObject newBoss;
    private bool spawned = false;

    // Spawn boss when player enters
    public void OnTriggerEnter(Collider other)
    {
        if (!spawned)
        {
            if (other.CompareTag("Player"))
            {
                newBoss = Instantiate(enemyTypeA, transform.position, transform.rotation);
                newBoss.SetActive(true);
                spawned = true;
            }
        }
        
    }

    // Reset Boss
    public void ResetArea()
    {
        Destroy(newBoss);
        spawned = false;
    }
}
