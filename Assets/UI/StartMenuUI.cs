using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class StartMenuUI : MonoBehaviour
{
    private Button startButton;
    private Button quitButton;

    void onEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        startButton = root.Q<Button>("start-button");
        quitButton = root.Q<Button>("quit-button");

        startButton.clicked += StartGame;
        quitButton.clicked += QuitGame;
    }

    void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
    
}
