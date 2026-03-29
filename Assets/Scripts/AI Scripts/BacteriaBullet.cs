using UnityEngine;

public class BacteriaBullet : BulleScript
{
    [SerializeField] private float height = 0f;
    [SerializeField] private GameObject AOE;

    //spawns puddle and then deletes itself
    protected override void OnTriggerEnter(Collider other)
    {
        Vector3 pos = transform.position;
        pos.y = height;
        Instantiate(AOE, pos, Quaternion.identity);
        Debug.Log(other.gameObject.name);
        Destroy(gameObject);

    }
}
