using UnityEngine;

public class PortalGate_Controller : MonoBehaviour
{
    [SerializeField] private GameObject portalVisual;
    [SerializeField] private Collider portalCollider;

    private void Awake()
    {
        F_TogglePortalGate(false);
    }

    public void F_TogglePortalGate(bool activate)
    {
        if (portalVisual != null)
            portalVisual.SetActive(activate);

        if (portalCollider != null)
            portalCollider.enabled = activate;
    }
}