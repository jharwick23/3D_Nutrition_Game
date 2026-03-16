using UnityEngine;

public class PanController : MonoBehaviour
{
    public Transform holdPanPoint;
    public Transform restPanPoint;
    public int _shieldDamage = 10;
    private bool isOnBack = false;

    private bool damageActive = false;
    private bool hasHitThisBash = false;
    public TutorialManager tutorialManager;
    
    void Awake()
    {
        if (holdPanPoint == null)
        {
            holdPanPoint = GameObject.Find("HoldPanPoint").transform;
        }

        if (restPanPoint == null)
        {
            restPanPoint = GameObject.Find("PanRestPoint").transform;
        }
    }

    void Start()
    {
        isOnBack = true;
        if (tutorialManager == null)
        {
            tutorialManager = FindFirstObjectByType<TutorialManager>();
        }
    }

    // Shield bash function (Only procs when hitting another collider)
    private void OnTriggerStay(Collider other)
    {
        if (isOnBack){
            return;
        }
        if (!damageActive){
            return;
        }
        if (hasHitThisBash){
            return;
        }

        AIEnemy enemy = other.GetComponent<AIEnemy>();
        if (enemy != null)
        {
            hasHitThisBash = true;
            enemy.enemyHealth -= _shieldDamage;
            enemy.UpdateUI();
            enemy.DoDeath();
        }
    }

    // Opens short damage window for the shield bash
    // Only called once bash start
    // Resets the hit lock so the bash can damage again
    public void BeginBashDamage(float duration)
    {
        if (isOnBack) return;

        damageActive = true;
        hasHitThisBash = false;

        CancelInvoke(nameof(EndBashDamage));
        Invoke(nameof(EndBashDamage), duration);
    }

    public void EndBashDamage()
    {
        damageActive = false;
        tutorialManager.DoBash();
    }

    public void SetPanOnBack()
    {
        isOnBack = true;
        transform.SetParent(restPanPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void HoldPanInHand()
    {
        isOnBack = false;
        transform.SetParent(holdPanPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        tutorialManager.DoBlock();
    }
}
