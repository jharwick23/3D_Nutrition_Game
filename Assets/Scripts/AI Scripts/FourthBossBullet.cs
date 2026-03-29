using System.Collections;
using UnityEngine;

public class FourthBossBullet : MonoBehaviour
{
    [SerializeField] private int takeDamageNum;
    private bool dieOnHit = true;
    public float timer;
    private void OnTriggerEnter(Collider other)
    {
        //Adds some form of chaotic bounce
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(Random.insideUnitSphere * 5f, ForceMode.Impulse);
        rb.angularVelocity += Random.insideUnitSphere * 5f;

        if (other.CompareTag("Player"))
        {
            PlayerControllerV2 playerController = other.GetComponent<PlayerControllerV2>();
            if (playerController != null)
            {

                playerController.TakeDamage(takeDamageNum);

            }
        }
        if (dieOnHit)
        {
            Debug.Log("Bulle Destrtoy via contact");
            Destroy(gameObject);
        }
    }

    public void ChangeBulletType(bool hitType)
    {
        dieOnHit = hitType;
    }

    public void MakeBulletDestroy()
    {
        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet ()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
