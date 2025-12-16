using UnityEngine;

public class SFXManager: MonoBehaviour
{
    public static SFXManager Instance;
    public AudioSource audioSource;
    public enum SFXType
    {
        KnifeSwing,
        DamageTaken,
        Death,
        Heal,
        Shoot,
        CollectCoin
    }

    [Header("Player")]
    public AudioClip knifeSwing;
    public AudioClip damageTaken;
    public AudioClip death;
    public AudioClip heal;
    public AudioClip shoot;
    public AudioClip collectCoin;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (!audioSource)
        {
            audioSource = GetComponent<AudioSource>();
        }
        if (!audioSource)
        {
            Debug.LogError("SFXManager is missing an AudioSource!");
        }
    }

    public void Play(SFXType type)
    {
        if (!audioSource) return;

        switch (type)
        {
            case SFXType.KnifeSwing:
                audioSource.PlayOneShot(knifeSwing);
                break;
            
            case SFXType.DamageTaken:
                audioSource.PlayOneShot(damageTaken);
                break;

            case SFXType.Death:
                audioSource.PlayOneShot(death, 5f);
                break;

            case SFXType.Heal:
                audioSource.PlayOneShot(heal, 10f);
                break;

            case SFXType.Shoot:
                audioSource.PlayOneShot(shoot);
                break;
            
            case SFXType.CollectCoin:
                audioSource.PlayOneShot(collectCoin);
                break;
        }
    }
}
