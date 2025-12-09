using UnityEngine;

public class PanController : MonoBehaviour
{
    public Transform holdPanPoint;
    public Transform restPanPoint;
    
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

    public void SetPanOnBack()
    {
        transform.SetParent(restPanPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void HoldPanInHand()
    {
        transform.SetParent(holdPanPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
