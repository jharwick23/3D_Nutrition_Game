using UnityEngine;


public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel;
    private bool isPaused = false;

    private void Awake()
    {   
        PauseMenu[] menus = FindObjectsOfType<PauseMenu>();
        if (menus.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

       
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Resume(); // Dont call resume, just disable PausePanel
    }

    private void Update()
    {
    
    }

    public void PerformPause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        // Disable Crosshair
        UIHandler _uiHandler;
        _uiHandler = FindFirstObjectByType<UIHandler>();
        if (_uiHandler)
        {
            _uiHandler.ToggleCrosshair(false);
        }
        else
        {
            Debug.Log("UI Handler was not set!");
        }

        if (pausePanel != null)
            pausePanel.SetActive(true);

        InputHandlerV2 inputHandler = FindFirstObjectByType<InputHandlerV2>();
        if (inputHandler)
        {
            inputHandler.DisableInputs();
        }
        else
        {
            Debug.Log("Inputhandler not found!");
        }
        Time.timeScale = 0f;     
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        // Enable Crosshair
        UIHandler _uiHandler;
        _uiHandler = FindFirstObjectByType<UIHandler>();
        if (_uiHandler)
        {
            _uiHandler.ToggleCrosshair(true);
        }
        else
        {
            Debug.Log("UI Handler was not set!");
        }

        if (pausePanel != null)
            pausePanel.SetActive(false);

        InputHandlerV2 inputHandler = FindFirstObjectByType<InputHandlerV2>();
        if (inputHandler)
        {
            inputHandler.EnableInputs();
        }
        else
        {
            Debug.Log("Inputhandler not found!");
        }
        
        Time.timeScale = 1f;     
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;     

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        
    }
}

