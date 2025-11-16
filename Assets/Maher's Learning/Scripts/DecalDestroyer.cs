using UnityEngine;

public class DecalDestroyer : MonoBehaviour
{
    public float lifetime = 30f;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

}
