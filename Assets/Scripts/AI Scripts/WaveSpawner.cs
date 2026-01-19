using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Collider triggerCollider;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private GameObject enemyTypeA;
    [SerializeField] private GameObject enemyTypeB;
    [SerializeField] private GameObject TypeC;
    [SerializeField] private GameObject TypeD;
    [SerializeField] private string enemyTag = "Enemy";

    [SerializeField] private Transform[] spawnPoints = new Transform[3];

    [SerializeField] private int totalWaves = 3;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private float timeBetweenSpawns = 0.5f;
    [SerializeField] private float enemyCheckInterval = 0.5f;

    private int currentWave = 0;
    private bool hasStarted = false;

    private Coroutine waveCoroutine;
    private Coroutine enemyCheckCoroutine;

    private int currentEnemyCount = 0;

    //Starts the initial spawning
    private void OnTriggerEnter(Collider other)
    {
        if (hasStarted)
            return;

        if (other.CompareTag(playerTag))
        {
            StartSpawner();
        }
    }

    //Launches two core routines to detect enemies and the waves
    private void StartSpawner()
    {
        hasStarted = true;
        triggerCollider.enabled = false;

        waveCoroutine = StartCoroutine(WaveRoutine());
        enemyCheckCoroutine = StartCoroutine(CheckAliveEnemies());
    }

    // Handles all wave info and functions
    private IEnumerator WaveRoutine()
    {
        while (currentWave < totalWaves)
        {
            currentWave++;
            yield return StartCoroutine(SpawnWave());

            // Wait until no enemies remain in the scene
            yield return new WaitUntil(() => currentEnemyCount == 0);
        }

        Finish();
    }

    //Spawns a wave based on set numbers
    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            Transform spawnPoint = GetRandomSpawnPoint();
            GameObject enemyPrefab = Random.value > 0.5f ? enemyTypeA : enemyTypeB;

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            enemy.SetActive(false);

            enemy.SetActive(true);

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    // Checks the amount of enemies remainding
    private IEnumerator CheckAliveEnemies()
    {
        while (hasStarted)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            currentEnemyCount = enemies.Length;

            yield return new WaitForSeconds(enemyCheckInterval);
        }
    }

    // Gets random spawns from three possible spawns
    private Transform GetRandomSpawnPoint()
    {
        Transform[] validPoints = System.Array.FindAll(spawnPoints, p => p != null);
        return validPoints[Random.Range(0, validPoints.Length)];
    }

    /// Fully resets the spawner so it can be triggered again
    public void ResetSpawner()
    {
        if (waveCoroutine != null)
            StopCoroutine(waveCoroutine);

        if (enemyCheckCoroutine != null)
            StopCoroutine(enemyCheckCoroutine);

        StopAllCoroutines();

        // Destroy remaining enemies by tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        currentEnemyCount = 0;
        currentWave = 0;
        hasStarted = false;

        if (triggerCollider != null)
            triggerCollider.enabled = true;
    }

    //Finishing function to spawn portal
    private void Finish()
    {
        hasStarted = false;

        if (enemyCheckCoroutine != null)
            StopCoroutine(enemyCheckCoroutine);

        TypeC.SetActive(false);
        TypeD.SetActive(true);
    }
}
