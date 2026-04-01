using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ThirdLevelPortal : MonoBehaviour
{
    private UIHandler _uiHandler;
    private TMP_Text nameTagText;
    private Material insideMat;
    private void Start()
    {
        _uiHandler ??= FindFirstObjectByType<UIHandler>();
        nameTagText = transform.Find("NameTagCanvas/Text (TMP)").GetComponent<TMP_Text>();
        insideMat = transform.Find("Mesh/Portal").GetComponent<MeshRenderer>().materials[1];

        if (PlayerPrefs.GetInt("ThirdLevelCompleted", 0) == 1)
        {
            nameTagText.color = Color.green;
            nameTagText.text = "Level 3\nComplete";
            Color original = insideMat.GetColor("_BaseColor");
            Color newColor = new Color(0f, 1f, 0f, original.a);
            insideMat.SetColor("_BaseColor", newColor);
        }
        else if (PlayerPrefs.GetInt("SecondLevelCompleted", 0) == 1)
        {
            nameTagText.color = Color.yellow;
            nameTagText.text = "Level 3\nIncomplete";
            Color original = insideMat.GetColor("_BaseColor");
            Color newColor = new Color(1f, 1f, 0f, original.a);
            insideMat.SetColor("_BaseColor", newColor);
        }
        else
        {
            nameTagText.color = Color.red;
            nameTagText.text = "Level 3\nLocked";
            Color original = insideMat.GetColor("_BaseColor");
            Color newColor = new Color(1f, 0f, 0f, original.a);
            insideMat.SetColor("_BaseColor", newColor);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PlayerPrefs.GetInt("SecondLevelCompleted", 0) == 1)
        {
            SceneManager.LoadScene("ThirdLevel");
            return;
        }
        _uiHandler.SetAlertPrompt("Complete the previous level to unlock this portal!", 2f, Color.red);
    }
}
