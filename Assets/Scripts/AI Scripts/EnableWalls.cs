using UnityEngine;

public class EnableWalls : MonoBehaviour
{
    public GameObject walls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            walls.SetActive(true);
        }
    }

    public void DeactivateWalls()
    {
        walls.SetActive(false);
    }
}
