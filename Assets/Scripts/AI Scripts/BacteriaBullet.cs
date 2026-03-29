using UnityEngine;

public class BacteriaBullet : BulleScript
{
    [SerializeField] private float height = 0f;
    [SerializeField] private GameObject AOE;

    //
    protected override void OnTriggerEnter(Collider other)
    {
        Vector3 pos = transform.position;
        pos.y = height;
        Instantiate(AOE, pos, Quaternion.identity);
        Debug.Log(other.gameObject.name);
        Destroy(gameObject);

    }
}
