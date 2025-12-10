using UnityEngine;

public class Puzzle1 : MonoBehaviour
{
    // Check if children "Objective1" and "Objective2" have completed their objectives with checkCompleted().
    
    public void resetPuzzle()
    {
        Transform objective1 = transform.Find("Objective1");
        Transform objective2 = transform.Find("Objective2");
        Transform objective3 = transform.Find("Objective3");
        Transform invisibleBorder = transform.Find("InvisibleBorderPuzzle");
        Transform PuzzleStreet1 = transform.Find("PuzzleStreet1");
        Transform PuzzleStreet2 = transform.Find("PuzzleStreet2");

        if (objective1 != null && objective2 != null && objective3 != null && invisibleBorder != null && PuzzleStreet1 != null && PuzzleStreet2 != null)
        {
            Puzzle1Objective obj1Script = objective1.GetComponent<Puzzle1Objective>();
            Puzzle1Objective obj2Script = objective2.GetComponent<Puzzle1Objective>();
            Puzzle1Objective obj3Script = objective3.GetComponent<Puzzle1Objective>();

            if (obj1Script != null && obj2Script != null && obj3Script != null)
            {
                obj1Script.resetObjective();
                obj2Script.resetObjective();
                obj3Script.resetObjective();

                // Disable Mesh Collider and Mesh Renderer of PuzzleStreet1 and PuzzleStreet2
                PuzzleStreet1.gameObject.GetComponent<MeshCollider>().enabled = false;
                PuzzleStreet1.gameObject.GetComponent<MeshRenderer>().enabled = false;
                PuzzleStreet2.gameObject.GetComponent<MeshCollider>().enabled = false;
                PuzzleStreet2.gameObject.GetComponent<MeshRenderer>().enabled = false;
                // Enable Box Collider of InvisibleBorderPuzzle
                invisibleBorder.gameObject.GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                Debug.LogError("Puzzle1Objective script not found on one of the objectives.");
            }
        }
        else
        {
            Debug.LogError("One of the objectives or invisible border not found.");
        }

    }
    
    public void checkPuzzleCompletion()
    {
        Transform objective1 = transform.Find("Objective1");
        Transform objective2 = transform.Find("Objective2");
        Transform objective3 = transform.Find("Objective3");
        Transform invisibleBorder = transform.Find("InvisibleBorderPuzzle");
        Transform PuzzleStreet1 = transform.Find("PuzzleStreet1");
        Transform PuzzleStreet2 = transform.Find("PuzzleStreet2");

        if (objective1 != null && objective2 != null && objective3 != null && invisibleBorder != null && PuzzleStreet1 != null && PuzzleStreet2 != null)
        {
            Puzzle1Objective obj1Script = objective1.GetComponent<Puzzle1Objective>();
            Puzzle1Objective obj2Script = objective2.GetComponent<Puzzle1Objective>();
            Puzzle1Objective obj3Script = objective3.GetComponent<Puzzle1Objective>();

            if (obj1Script != null && obj2Script != null && obj3Script != null)
            {
                if (obj1Script.getCompleted() && obj2Script.getCompleted() && obj3Script.getCompleted())
                {
                    // Enable Mesh Collider and Mesh Renderer of PuzzleStreet1 and PuzzleStreet2
                    PuzzleStreet1.gameObject.GetComponent<MeshCollider>().enabled = true;
                    PuzzleStreet1.gameObject.GetComponent<MeshRenderer>().enabled = true;
                    PuzzleStreet2.gameObject.GetComponent<MeshCollider>().enabled = true;
                    PuzzleStreet2.gameObject.GetComponent<MeshRenderer>().enabled = true;
                    // Disable Box Collider of InvisibleBorderPuzzle
                    invisibleBorder.gameObject.GetComponent<BoxCollider>().enabled = false;
                }
            }
            else
            {
                Debug.LogError("Puzzle1Objective script not found on one of the objectives.");
            }
        }
        else
        {
            Debug.LogError("One of the objectives or invisible border not found.");
        }
    }
}
