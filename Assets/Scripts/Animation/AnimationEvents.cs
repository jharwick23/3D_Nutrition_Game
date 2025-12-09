using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public KnifeController Knife;
    public PlayerControllerV2 PlayerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Knife == null)
        {
            Knife = FindFirstObjectByType<KnifeController>();
        }
        if(PlayerController == null)
        {
            PlayerController = FindFirstObjectByType<PlayerControllerV2>();
        }
    }

    public void ResetOnHit()
    {
        if(Knife != null)
        {
            Knife.ResetHit();
        }
    }
}
