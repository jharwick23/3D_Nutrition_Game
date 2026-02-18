using System.Diagnostics;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyTypeA, portal;
    [SerializeField] private Transform transform;
    private GameObject newBoss;
    private bool spawned = false, dead = true;

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
                dead = false;
            }
        }
        
    }

    void Update()
    {
        if (spawned && !dead)
        {
            if(newBoss == null)
            {
                portal.SetActive(true);
            }
        }
    }

    // Reset Boss
    public void ResetArea()
    {
        Destroy(newBoss);
        spawned = false;
        dead = true;
        portal.SetActive(false);
        GetComponent<EnableWalls>().DeactivateWalls();
    }
}
