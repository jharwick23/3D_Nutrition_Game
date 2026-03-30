using Unity.VisualScripting;
using UnityEngine;

public class BossDoor : MonoBehaviour
{

    [SerializeField] private GameObject firstBeacon;
    [SerializeField] private GameObject secondBeacon;
    [SerializeField] private GameObject self;

    //Once both objects are destroyed, it destroys it self
    void Update()
    {
        if (firstBeacon == null && secondBeacon == null)
        {
            Debug.Log("Attempted Destroy");
            Destroy(self);
        }
    }
}
