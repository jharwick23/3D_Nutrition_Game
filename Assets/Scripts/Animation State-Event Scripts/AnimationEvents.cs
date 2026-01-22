using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public KnifeController Knife;
    public PlayerControllerV2 PlayerController;
    void Awake()
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

    public void PlayKnifeSlashSound()
    {
        if (!SFXManager.Instance)
        {
            Debug.LogError("SFXManager not found in scene");
            return;
        }

        if (!Knife) return;
        if (Knife.GetPlayedSound()) return;

        SFXManager.Instance.Play(SFXManager.SFXType.KnifeSwing);
    }
}
