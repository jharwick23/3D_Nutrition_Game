using UnityEngine;

public class KnifeController : MonoBehaviour
{
    public Transform restKnifePoint;
    public Transform holdKnifePoint;
    public int knifeDamage = 25;
    private bool hasHit = false;

    void Awake()
    {
        if (holdKnifePoint == null)
        {
            holdKnifePoint = GameObject.Find("HoldKnifePoint").transform;
        }

        if (restKnifePoint == null)
        {
            restKnifePoint = GameObject.Find("RestKnifePoint").transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        
        AIEnemy enemy = other.GetComponent<AIEnemy>();
        if (enemy != null)
        {
            hasHit = true;

            enemy.enemyHealth -= knifeDamage;
            enemy.UpdateUI();
            enemy.DoDeath();
        }
    }

    public void ResetHit()
    {
        hasHit = false;
    }

    public void SetKnifeOnHip()
    {
        transform.SetParent(restKnifePoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void HoldKnifeInHand()
    {
        transform.SetParent(holdKnifePoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
