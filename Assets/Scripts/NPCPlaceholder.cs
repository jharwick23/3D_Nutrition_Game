using UnityEngine;
using UnityEngine.InputSystem;

public class NPCPlaceholder : MonoBehaviour
{
    [Header("NPC Settings")]
    public string npcName = "NPC";

    [TextArea]
    public string[] dialogueLines;

    private bool playerInRange;
    private bool subscribed;
    private InputAction interactAction;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.root.CompareTag("Player"))
            return;

        playerInRange = true;

        var playerInput = other.transform.root.GetComponentInChildren<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogWarning("No PlayerInput found on Player.");
            return;
        }

        interactAction = playerInput.actions.FindAction("Interact", false);
        if (interactAction == null)
        {
            Debug.LogWarning("Interact action not found.");
            return;
        }

        if (!subscribed)
        {
            interactAction.performed += OnInteractPerformed;
            subscribed = true;
        }

        interactAction.Enable();

        Debug.Log($"In range of {npcName}. Press Interact to talk.");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.transform.root.CompareTag("Player"))
            return;

        playerInRange = false;
    }

    private void OnDisable()
    {
        if (interactAction != null && subscribed)
            interactAction.performed -= OnInteractPerformed;
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (!playerInRange)
            return;

        StartDialogue();
    }

    private void StartDialogue()
    {
        if (DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager.Instance is NULL. Make sure DialogueCanvas/DialogueManager exists in the scene.");
            return;
        }

        DialogueManager.Instance.StartDialogue(npcName, dialogueLines);
    }

}

