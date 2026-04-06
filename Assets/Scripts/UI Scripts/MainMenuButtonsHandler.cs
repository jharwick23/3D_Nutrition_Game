using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonsHandler : MonoBehaviour
{
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject control;
    [SerializeField] private GameObject credits;

    //Starts game and load main hub
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    //Starts new game, deletes all player prefs and load main hub
    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainScene");
    }

    //Handles quit, closes application
    public void Quit()
    {
        Debug.Log("Quit Game");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    //Moves to control page
    public void Controls()
    {
        control.SetActive(true);
        main.SetActive(false);

    }

    //Moves back to main page
    public void Back()
    {
        main.SetActive(true); 
        control.SetActive(false);
        credits.SetActive(false);
    }

    public void Credits()
    {
        main.SetActive(false);
        credits.SetActive(true);
    }
}
