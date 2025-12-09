using UnityEngine;

public class Puzzle1Objective : MonoBehaviour
{
    public Material SuccessMaterial;
    public Material DefaultMaterial;
    private bool completed = false;
    public bool getCompleted()
    {
        return completed;
    }

    public void resetObjective()
    {
        // Change the material of child X1 and X2 to DefaultMaterial
            Transform childX1 = transform.Find("X1");
            Transform childX2 = transform.Find("X2");
            if (childX1 != null)
            {
                Renderer rendererX1 = childX1.GetComponent<Renderer>();
                if (rendererX1 != null && DefaultMaterial != null)
                {
                    rendererX1.material = DefaultMaterial;
                }
            }
            if (childX2 != null)
            {
                Renderer rendererX2 = childX2.GetComponent<Renderer>();
                if (rendererX2 != null && DefaultMaterial != null)
                {
                    rendererX2.material = DefaultMaterial;
                }
            }
            completed = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            // Change the material of child X1 and X2 to SuccessMaterial
            Transform childX1 = transform.Find("X1");
            Transform childX2 = transform.Find("X2");
            if (childX1 != null)
            {
                Renderer rendererX1 = childX1.GetComponent<Renderer>();
                if (rendererX1 != null)
                {
                    rendererX1.material = SuccessMaterial;
                }
            }
            if (childX2 != null)
            {
                Renderer rendererX2 = childX2.GetComponent<Renderer>();
                if (rendererX2 != null)
                {
                    rendererX2.material = SuccessMaterial;
                }
            }
            completed = true;
            // Call checkPuzzleCompletion() from parent Puzzle1 script
            Puzzle1 puzzle1Script = transform.parent.GetComponent<Puzzle1>();
            if (puzzle1Script != null)
            {
                puzzle1Script.checkPuzzleCompletion();
            }
            else
            {
                Debug.LogError("Puzzle1 script not found on parent.");
            }

            // Play "ding" sound
            
            
        }
    }
}
