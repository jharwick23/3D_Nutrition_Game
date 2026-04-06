using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Serialized Variables to Give different Settings for each zone, 
    [SerializeField] private GameObject enemyTypeA;
    [SerializeField] private GameObject enemyTypeB;
    [SerializeField] private Transform[] spawnPoints = new Transform[5];
    [SerializeField] private int initialSpawnCount = 10;  
    [SerializeField] private int respawnAmount = 3;        
    [SerializeField] private int respawnThreshold = 2;     
    [SerializeField] private bool spawningDone = false;
    [SerializeField] private int maxTotalSpawns = 20;
    private int totalSpawned = 0;
 

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (spawningDone) return;

        if (other.CompareTag("Player"))
        {
            spawningDone = true;
            SpawnInitialEnemies();
        }
    }

    private void Update()
    {
        // Clean null entries (dead enemies)
        spawnedEnemies.RemoveAll(e => e == null);

        // Respawn logic
        if (spawningDone && spawnedEnemies.Count <= respawnThreshold)
        {
            RespawnEnemies();
        }
    }

    // Spawns the initial wave of enemies
    private void SpawnInitialEnemies()
    {
        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnRandomEnemyAtRandomLocation();
        }
    }

    //Respawn enemies after a certain number of enemies is decreased
    private void RespawnEnemies()
    {
        for (int i = 0; i < respawnAmount; i++)
        {
            SpawnRandomEnemyAtRandomLocation();
        }
    }

    //Spawns enemies at random locations using 5 preset locations, also randomly determines enemies
    private void SpawnRandomEnemyAtRandomLocation()
    {
        if (totalSpawned >= maxTotalSpawns)
        {
            return;
        }
        
        GameObject prefabToSpawn = Random.value < 0.5f ? enemyTypeA : enemyTypeB;
        int pointIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[pointIndex];
        GameObject newEnemy = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        newEnemy.SetActive(true);
        spawnedEnemies.Add(newEnemy);
        totalSpawned++;
    }

    // Function to be called when player dies to reset enemies
    public void ResetArea()
    {
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }

        spawnedEnemies.Clear();
        spawningDone = false; 
        totalSpawned = 0;

        GameObject[] smallEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        //Just for fourthlevel
        foreach (var enemy in smallEnemies)
        {
            if (enemy.name == "VirusEnemySmall(Clone)")
            {
                Destroy(enemy);
            }
        }
    }
}
