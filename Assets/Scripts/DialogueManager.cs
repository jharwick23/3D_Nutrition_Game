using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    private string[] currentLines;
    private int currentIndex;
    private bool isDialogueActive;

    private InputAction interactAction;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        dialoguePanel.SetActive(false);

        var playerInput = FindFirstObjectByType<PlayerInput>();
        interactAction = playerInput.actions.FindAction("Interact");
        interactAction.performed += OnInteract;
    }

    public void StartDialogue(string npcName, string[] lines)
    {
        if (lines == null || lines.Length == 0) return;

        currentLines = lines;
        currentIndex = 0;
        isDialogueActive = true;

        dialoguePanel.SetActive(true);
        ShowLine();

        // Lock player input feel (simple version)
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!isDialogueActive) return;

        currentIndex++;

        if (currentIndex >= currentLines.Length)
            EndDialogue();
        else
            ShowLine();
    }

    private void ShowLine()
    {
        dialogueText.text = currentLines[currentIndex];
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}

