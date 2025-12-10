using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject control;

    //Starts game and load main hub
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    //Handles quit, closes application
    public void Quit()
    {
        Application.Quit();
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
           
    }
}
