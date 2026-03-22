using UnityEngine;

public class TurretManagement : MonoBehaviour
{
    [SerializeField] GameObject firstTurret;
    [SerializeField] GameObject secondTurret;
    private bool triggered = false;
    // Update is called once per frame
    void Update()
    {
        if (firstTurret == null && !triggered)
        {
            secondTurret.SetActive(true);
            triggered = true;
        }
    }
}
