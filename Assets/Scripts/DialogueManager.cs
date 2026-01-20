using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;


public class DialogueManager : MonoBehaviour
{
    public bool IsDialogueActive => isDialogueActive;
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
        if (isDialogueActive) return;
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

        // If we're on the last line, end dialogue
        if (currentLines == null || currentLines.Length == 0 || currentIndex >= currentLines.Length - 1)
        {
            EndDialogue();
            return;
        }

        // Otherwise go to next line
        currentIndex++;
        ShowLine();
    }

    public void AdvanceOrEnd()
    {
        if (!isDialogueActive) return;

        if (currentLines == null || currentLines.Length == 0 || currentIndex >= currentLines.Length - 1)
            EndDialogue();
        else
        {
            currentIndex++;
            ShowLine();
        }
    }

    public void TryStartDialogue(string npcName, string[] lines)
    {
        if (isDialogueActive) return;
        StartDialogue(npcName, lines);
    }

    private void ShowLine()
    {
        dialogueText.text = currentLines[currentIndex];
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        currentLines = null;
        currentIndex = 0;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}

