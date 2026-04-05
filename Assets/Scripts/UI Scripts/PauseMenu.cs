using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel;
    public TutorialManager tutorialManager;
    private PlayerControllerV2 _playerController;
    public GameObject VerticalSensitivityTitle;
    public GameObject HorizontalSensitivityTitle;
    private Slider VerticalSensitivitySlider;
    private Slider HorizontalSensitivitySlider;
    private TextMeshProUGUI VerticalSensitivityValueText;
    private TextMeshProUGUI HorizontalSensitivityValueText;
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
        pausePanel.SetActive(true);
        if (tutorialManager == null)
        {
            tutorialManager = FindFirstObjectByType<TutorialManager>();
        }
        if (_playerController == null)
        {
            _playerController = FindFirstObjectByType<PlayerControllerV2>();
        }
        
        if (VerticalSensitivityTitle == null)
        {
            VerticalSensitivityTitle = GameObject.Find("VerticalSensitivityTitle");
        }
        VerticalSensitivitySlider = VerticalSensitivityTitle.GetComponentInChildren<Slider>();
        VerticalSensitivityValueText = VerticalSensitivityTitle.transform.Find("Value").GetComponentInChildren<TextMeshProUGUI>();

        _playerController.LookSensitivityY = PlayerPrefs.GetFloat("VerticalSensitivity", 20);
        VerticalSensitivitySlider.value = _playerController.LookSensitivityY;
        VerticalSensitivityValueText.text = _playerController.LookSensitivityY.ToString("F1");
        
        if (HorizontalSensitivityTitle == null)
        {
            HorizontalSensitivityTitle = GameObject.Find("HorizontalSensitivityTitle");
        }
        HorizontalSensitivitySlider = HorizontalSensitivityTitle.GetComponentInChildren<Slider>();
        HorizontalSensitivityValueText = HorizontalSensitivityTitle.transform.Find("Value").GetComponentInChildren<TextMeshProUGUI>();

        _playerController.RotationSpeed = PlayerPrefs.GetFloat("HorizontalSensitivity", 20);
        HorizontalSensitivitySlider.value = _playerController.RotationSpeed;
        HorizontalSensitivityValueText.text = _playerController.RotationSpeed.ToString("F1");
        pausePanel.SetActive(false);
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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
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
    }

    public void TutorialButtonClicked()
    {
        tutorialManager.TutorialButtonClicked();
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

    public void OnHorizontalSensitivityChanged()
    {
        float value = HorizontalSensitivitySlider.value;
        PlayerPrefs.SetFloat("HorizontalSensitivity", value);
        _playerController.RotationSpeed = value;
        HorizontalSensitivityValueText.text = value.ToString("F1");
    }

    public void OnVerticalSensitivityChanged()
    {
        float value = VerticalSensitivitySlider.value;
        PlayerPrefs.SetFloat("VerticalSensitivity", value);
        _playerController.LookSensitivityY = value;
        VerticalSensitivityValueText.text = value.ToString("F1");
    }
    
}

