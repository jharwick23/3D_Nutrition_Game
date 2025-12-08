using UnityEngine;

public class NPCPlaceholder : MonoBehaviour
{
    [Header("NPC Settings")]
    public string npcName = "NPC";
    [TextArea]
    public string[] dialogueLines;

    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.E;

    private bool playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        // Only react if the thing entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log($"Press {interactKey} to talk to {npcName}.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log($"You walked away from {npcName}.");
        }
    }

    private void Update()
    {
        // Only listen for input if the player is in range
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        Debug.Log($"Starting dialogue with {npcName}.");

        // For now, just print lines to the Console as a placeholder
        if (dialogueLines != null && dialogueLines.Length > 0)
        {
            foreach (string line in dialogueLines)
            {
                Debug.Log($"{npcName}: {line}");
            }
        }
        else
        {
            Debug.Log($"{npcName} has nothing to say yet. (Add lines in the Inspector.)");
        }
    }
}

