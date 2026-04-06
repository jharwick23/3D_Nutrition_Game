using UnityEngine;

public class GeneralDestroyerFunction : MonoBehaviour
{
    [SerializeField] private GameObject[] checkingList;
    [SerializeField] private GameObject[] destroyingList;
    private bool stillAlive = true;

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject game in checkingList)
        {
            if (game != null)
            {
                stillAlive = true;
                break;
            }
            else
            {
                stillAlive = false;
            }
        }
        if (!stillAlive)
        {
            foreach(GameObject game in destroyingList)
            {
                Destroy(game);
            }
        }
    }
}
