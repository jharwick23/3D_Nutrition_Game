using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SecondLevelPortal : MonoBehaviour
{
    private TMP_Text nameTagText;
    private Material insideMat;
    private void Start()
    {
        nameTagText = transform.Find("NameTagCanvas/Text (TMP)").GetComponent<TMP_Text>();
        insideMat = transform.Find("Mesh/Portal").GetComponent<MeshRenderer>().materials[1];

        if (PlayerPrefs.GetInt("SecondLevelCompleted", 0) == 1)
        {
            nameTagText.color = Color.green;
            nameTagText.text = "Level 2\nComplete";
            Color original = insideMat.GetColor("_BaseColor");
            Color newColor = new Color(0f, 1f, 0f, original.a);
            insideMat.SetColor("_BaseColor", newColor);
        }
        else if (PlayerPrefs.GetInt("FirstLevelCompleted", 0) == 1)
        {
            nameTagText.color = Color.yellow;
            nameTagText.text = "Level 2\nIncomplete";
            Color original = insideMat.GetColor("_BaseColor");
            Color newColor = new Color(1f, 1f, 0f, original.a);
            insideMat.SetColor("_BaseColor", newColor);
        }
        else
        {
            nameTagText.color = Color.red;
            nameTagText.text = "Level 2\nLocked";
            Color original = insideMat.GetColor("_BaseColor");
            Color newColor = new Color(1f, 0f, 0f, original.a);
            insideMat.SetColor("_BaseColor", newColor);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PlayerPrefs.GetInt("FirstLevelCompleted", 0) == 1)
        {
            SceneManager.LoadScene("SecondLevel");
        }
    }
}
