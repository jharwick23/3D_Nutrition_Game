using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenMenu : MonoBehaviour
{
    [SerializeField] private GameObject deathScreenPanel;

    public void EnableDeathScreen()
    {
        deathScreenPanel.SetActive(true);
        Time.timeScale = 0f;

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

        // Disable Inputs
        InputHandlerV2 inputHandler = FindFirstObjectByType<InputHandlerV2>();
        if (inputHandler)
        {
            inputHandler.DisableInputs();
        }
        else
        {
            Debug.Log("Inputhandler not found!");
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public bool CheckDeathScreenPanelActive()
    {
        if (deathScreenPanel.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Retry()
    {
        deathScreenPanel.SetActive(false);
        Time.timeScale = 1f;
        PlayerControllerV2 playercontroller = FindFirstObjectByType<PlayerControllerV2>();
        playercontroller.DoDeath();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
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

        // Enable Inputs
        InputHandlerV2 inputHandler = FindFirstObjectByType<InputHandlerV2>();
        if (inputHandler)
        {
            inputHandler.EnableInputs();
        }
        else
        {
            Debug.Log("Inputhandler not found!");
        }
    }

    public void BackToHub()
    {
        deathScreenPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("MainScene");

    }
}
