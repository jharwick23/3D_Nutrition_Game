using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                // Check if current scene is "FirstLevel" and if so set playerpref firstlevelbossdefeated to true
                if (SceneManager.GetActiveScene().name == "FirstLevel")
                {
                    PlayerPrefs.SetInt("FirstLevelCompleted", 1);
                }
                else if (SceneManager.GetActiveScene().name == "SecondLevel")
                {
                    PlayerPrefs.SetInt("SecondLevelCompleted", 1);
                }
                else if (SceneManager.GetActiveScene().name == "ThirdLevel")
                {
                    PlayerPrefs.SetInt("ThirdLevelCompleted", 1);
                }
                PlayerPrefs.Save();
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
