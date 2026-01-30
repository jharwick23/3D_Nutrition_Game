using UnityEngine;

public class PanController : MonoBehaviour
{
    public Transform holdPanPoint;
    public Transform restPanPoint;
    public int _shieldDamage = 10;
    private bool isOnBack = false;
    
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
    }

    // Shield bash function (Only procs when hitting another collider)
    private void OnTriggerEnter(Collider other)
    {
        if (isOnBack) return; // Makes sure the shield does not do damage while the player is not holding it

        Debug.Log("Hit");
        AIEnemy enemy = other.GetComponent<AIEnemy>();
        if (enemy != null)
        {
            Debug.Log("Hit");
            enemy.enemyHealth -= _shieldDamage;
            enemy.UpdateUI();
            enemy.DoDeath();
        }
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
    }
}
