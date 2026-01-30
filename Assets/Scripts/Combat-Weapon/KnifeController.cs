using UnityEngine;

public class KnifeController : MonoBehaviour
{
    public Transform restKnifePoint;
    public Transform holdKnifePoint;
    public int knifeDamage = 25;
    private bool hasHit = false;
    private bool soundPlayed = false;
    private bool isOnHip = false;

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

    void Start()
    {
        isOnHip = true;
    }

    // Melee Attack function (Only procs when hitting another collider)
    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        if (isOnHip) return;
        
        AIEnemy enemy = other.GetComponent<AIEnemy>();
        if (enemy != null)
        {
            hasHit = true;

            enemy.enemyHealth -= knifeDamage;
            enemy.UpdateUI();
            enemy.DoDeath();
        }
    }

    // Resets the "has hit" variable which is used so enemies do not take  damage twice
    public void ResetHit()
    {
        hasHit = false;
        soundPlayed = false;
    }

    public bool GetPlayedSound()
    {
        return soundPlayed;
    }

    public void SetSoundPlayed()
    {
        soundPlayed = true;
    }

    public void SetKnifeOnHip()
    {
        isOnHip = true;
        transform.SetParent(restKnifePoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void HoldKnifeInHand()
    {
        isOnHip = false;
        transform.SetParent(holdKnifePoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
