using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel;
    private bool isPaused = false;

    private void Awake()
    {   
        // PauseMenu[] menus = FindObjectsOfType<PauseMenu>();
        // if (menus.Length > 1)
        // {
        //     Debug.Log("Destroyed GameObject");
        //     Destroy(gameObject);
        //     return;
        // }

       
        // DontDestroyOnLoad(gameObject);
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
        DeathScreenMenu deathScreenMenu = FindFirstObjectByType<DeathScreenMenu>();
        if (deathScreenMenu)
        {
            if (deathScreenMenu.CheckDeathScreenPanelActive())
            {
                return;
            }
        }
        else
        {
            Debug.Log("DeathScreenMenu Unavailable for Pause Menu!");
        }

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

        if (pausePanel)
        {
            pausePanel.SetActive(true);   
        }
        else
        {
            Debug.Log("Pause Panel not found!");
        }

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

        if (pausePanel)
        {
            pausePanel.SetActive(false);   
        }
        else
        {
            Debug.Log("Pause Panel not found!");
        }

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

    public void BackToMainMenu()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main menu");
    }

    public void BackToHub()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("MainScene");
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

