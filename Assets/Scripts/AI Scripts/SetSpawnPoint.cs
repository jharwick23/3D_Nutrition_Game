using UnityEngine;

public class SetSpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject checkpoint;


    //Sets a new spawnpoint when player enters a the boss
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (spawnPoint != null && checkpoint != null)
            {
                checkpoint.SetActive(true);
                spawnPoint.SetActive(false);
            }
        }
    }
}
