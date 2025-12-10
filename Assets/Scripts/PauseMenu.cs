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
        Resume(); 
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
        if (pausePanel != null)
            pausePanel.SetActive(true);

        Time.timeScale = 0f;     
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        Debug.Log("test");
        if (pausePanel != null)
            pausePanel.SetActive(false);

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

