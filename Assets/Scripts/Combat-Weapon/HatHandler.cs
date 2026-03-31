using UnityEngine;

public class HatHandler : MonoBehaviour
{
    public Transform headAttachPoint;
    public Transform gunHoldPoint;

    void Start()
    {
        if (headAttachPoint == null)
        {
            headAttachPoint = GameObject.Find("HatRestPoint").transform;
        }

        if (gunHoldPoint == null)
        {
            gunHoldPoint = GameObject.Find("HatHoldPoint").transform;
        }
    }

    public void SetOnHead()
    {
        transform.SetParent(headAttachPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void SetOnGun()
    {
        transform.SetParent(gunHoldPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
