using UnityEngine;
using UnityEngine.InputSystem;

public class NPCPlaceholder : MonoBehaviour
{
    public string npcName = "NPC";
    [TextArea] public string[] dialogueLines;

    private bool playerInRange;
    private InputAction interactAction;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.root.CompareTag("Player")) return;

        playerInRange = true;

        var playerInput = other.transform.root.GetComponentInChildren<PlayerInput>();
        interactAction = playerInput != null ? playerInput.actions.FindAction("Interact", false) : null;

        Debug.Log($"In range of {npcName}. Press Interact to talk.");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.transform.root.CompareTag("Player")) return;
        playerInRange = false;
    }

    private void Update()
    {
        if (!playerInRange) return;
        if (DialogueManager.Instance == null) return;
        if (interactAction == null) return;

        // If dialogue is open, let E advance/close.
        // If dialogue is closed, let E start.
        if (interactAction.WasPressedThisFrame())
        {
            if (DialogueManager.Instance.IsDialogueActive)
                DialogueManager.Instance.AdvanceOrEnd();
            else
                DialogueManager.Instance.TryStartDialogue(npcName, dialogueLines);
        }
    }
}


