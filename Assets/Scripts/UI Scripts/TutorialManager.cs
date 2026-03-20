using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    private GameObject _tutorialPanel;
    private TextMeshProUGUI _tutorialTitleTMP;
    private TextMeshProUGUI _tutorialPromptTMP;
    private int tutorialStep = -1;

    void Awake()
    {
        if (!_tutorialPanel)
        {
            _tutorialPanel = transform.Find("TutorialPanel").gameObject;
        }
        if (!_tutorialPromptTMP) 
        {
            Transform prompt = _tutorialPanel.transform.Find("TutorialPrompt");
            if (prompt != null)
                _tutorialPromptTMP = prompt.GetComponent<TextMeshProUGUI>();
        }
        if (!_tutorialTitleTMP)
        {
            Transform title = _tutorialPanel.transform.Find("TutorialTitle");
            if (title != null)
                _tutorialTitleTMP = title.GetComponent<TextMeshProUGUI>();
        }
        if (_tutorialPromptTMP == null || _tutorialTitleTMP == null)
        {
            Debug.LogWarning("Tutorial TMPro components not found.");
        }
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("TutorialCompleted", 0) != 1)
        {
            StartTutorial();
        }
    }

    public void TutorialButtonClicked()
    {
        if (!_tutorialPanel.activeSelf)
        {
            StartTutorial();
        }
        else
        {
            EndTutorial();
        }
    }

    public void EndTutorial()
    {
        if (!_tutorialPanel.activeSelf) return;
        _tutorialPanel.SetActive(false);
        tutorialStep = -1;
    }

    public void StartTutorial()
    {
        _tutorialPanel.SetActive(true);
        tutorialStep = 0;

        _tutorialTitleTMP.text = "Tutorial: Movement";
        _tutorialPromptTMP.text = "Use WASD to move and SHIFT to run.";
    }

    public void DoMovement()
    {
        if (tutorialStep != 0) return;
        tutorialStep = 1;

        _tutorialTitleTMP.text = "Tutorial: Jump";
        _tutorialPromptTMP.text = "Press SPACE to Jump.";
    }

    public void DoJump()
    {
        if (tutorialStep != 1) return;
        tutorialStep = 2;

        _tutorialTitleTMP.text = "Tutorial: Dash";
        _tutorialPromptTMP.text = "Move and Press ALT to Dash.";
    }

    public void DoDodge()
    {
        if (tutorialStep != 2) return;
        tutorialStep = 3;

        _tutorialTitleTMP.text = "Tutorial: Shoot Projectile";
        _tutorialPromptTMP.text = "Press LEFT-CLICK to shoot a projectile.";
    }

    public void DoProjectileAttack()
    {
        if (tutorialStep != 3) return;
        tutorialStep = 4;

        _tutorialTitleTMP.text = "Tutorial: Reloading";
        _tutorialPromptTMP.text = "Press R to Reload.";
    }

    public void DoReloading()
    {
        if (tutorialStep != 4) return;
        tutorialStep = 5;

        _tutorialTitleTMP.text = "Tutorial: Melee";
        _tutorialPromptTMP.text = "Press F to Melee Attack.";
    }

    public void DoMeleeAttack()
    {
        if (tutorialStep != 5) return;
        tutorialStep = 6;

        _tutorialTitleTMP.text = "Tutorial: Blocking";
        _tutorialPromptTMP.text = "Hold RIGHT-CLICK to Block.";
    }

    public void DoBlock()
    {
        if (tutorialStep != 6) return;
        tutorialStep = 7;

        _tutorialTitleTMP.text = "Tutorial: Bashing";
        _tutorialPromptTMP.text = "While Blocking, click LEFT-CLICK to Bash.";
    }

    public void DoBash()
    {
        if (tutorialStep != 7) return;
        tutorialStep = 8;

        _tutorialTitleTMP.text = "Tutorial: COMPLETE";
        _tutorialPromptTMP.text = "Well Done, you have completed the tutorial! You can always restart it in the 'ESC' Menu.";
        StartCoroutine(CompleteTutorial());
    }

    private IEnumerator CompleteTutorial()
    {
        PlayerPrefs.SetInt("TutorialCompleted", 1);
        PlayerPrefs.Save();
        yield return new WaitForSeconds(5);
        
        EndTutorial();
    }
}
