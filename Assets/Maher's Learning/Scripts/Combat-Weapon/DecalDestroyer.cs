using UnityEngine;

public class DecalDestroyer : MonoBehaviour
{
    public float lifetime = 3f;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

}
